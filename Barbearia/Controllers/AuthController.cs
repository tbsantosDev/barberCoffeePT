using Barbearia.Data;
using Barbearia.Dto.User;
using Barbearia.Models;
using Barbearia.Services.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Crypto.Generators;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace Barbearia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel model)
        {
            var user = _context.Users.SingleOrDefault(u => u.Email == model.Email);
            // Aqui, você verificaria as credenciais do usuário
            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
            {
                return Unauthorized(); // Retorna 401 se o usuário não for encontrado ou se a senha estiver incorreta
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtSecretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY")
                ?? "a_super_secret_key_with_at_least_32_characters";
            var key = Encoding.ASCII.GetBytes(jwtSecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, model.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new { Token = tokenString });
        }

        [HttpPost("register")]
        public async Task<ResponseModel<UserModel>> CreateUser(CreateUserDto createUserClientDto)
        {
            ResponseModel<UserModel> response = new ResponseModel<UserModel>();

            try
            {
                // Validar se o e-mail já está em uso
                var existingUser = await _context.Users.SingleOrDefaultAsync(u => u.Email == createUserClientDto.Email);
                if (existingUser != null)
                {
                    response.Message = "E-mail já está em uso.";
                    response.Status = false;
                    return response;
                }

                // Hash da senha
                var passwordHash = BCrypt.Net.BCrypt.HashPassword(createUserClientDto.Password);

                var user = new UserModel()
                {
                    FirstName = createUserClientDto.FirstName,
                    LastName = createUserClientDto.LastName,
                    Email = createUserClientDto.Email,
                    Password = passwordHash,
                    Role = (Models.Enums.RoleEnums.Client),
                    EmailConfirmationToken = Guid.NewGuid().ToString() // Gera um token de confirmação
                };

                await _context.AddAsync(user);
                await _context.SaveChangesAsync();
                await SendConfirmationEmail(user);

                response.Message = "Usuário cadastrado com sucesso! Verifique seu e-mail para confirmar o registro.";
                response.Status = true;
            }
            catch (DbUpdateException dbEx)
            {
                response.Message = "Erro ao atualizar o banco de dados: " + dbEx.Message;
                response.Status = false;
            }
            catch (Exception ex)
            {
                response.Message = "Erro interno: " + ex.Message;
                response.Status = false;
            }

            return response;
        }

        [HttpPatch("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string token)
        {

            var user = await _context.Users.SingleOrDefaultAsync(u => u.EmailConfirmationToken == token);
            if (user == null)
            {
                return BadRequest("Token inválido.");
            }

            user.EmailConfirmed = true;
            user.EmailConfirmationToken = null;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return Ok("E-mail confirmado com sucesso!");
        }


        public class ConfirmEmailRequest
        {
            public string Token { get; set; }
        }

        private async Task SendConfirmationEmail(UserModel user)
        {
            try
            {
                var confirmationLink = $"http://localhost:3000/confirmEmail?token={user.EmailConfirmationToken}";

                var appPassword = Environment.GetEnvironmentVariable("APP_PASSWORD_GOOGLE");
                if (string.IsNullOrEmpty(appPassword))
                {
                    Console.WriteLine("Senha APP não configurada");
                }

                // Configurar o cliente SMTP
                using (var smtpClient = new SmtpClient("smtp.gmail.com"))
                {
                    smtpClient.Port = 587; // Porta para envio SMTP, geralmente 587 ou 465
                    smtpClient.Credentials = new NetworkCredential("ago14santos98@gmail.com", appPassword);
                    smtpClient.EnableSsl = true;

                    // Configurar o e-mail a ser enviado
                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress("ago14santos98@gmail.com", "Café com barba"),
                        Subject = "Confirmação de E-mail",
                        Body = $"Olá {user.FirstName},\n\nPor favor, confirme seu e-mail clicando no link abaixo:\n{confirmationLink}",
                        IsBodyHtml = false
                    };
                    mailMessage.To.Add(user.Email);

                    // Enviar o e-mail
                    await smtpClient.SendMailAsync(mailMessage);
                }
            }
            catch (Exception ex)
            {
                // Tratamento de erro no envio de e-mail (registrar erro, etc)
                Console.WriteLine($"Erro ao enviar e-mail: {ex.Message}");
            }
        }

    }
}
