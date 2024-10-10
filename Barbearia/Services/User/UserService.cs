using Barbearia.Data;
using Barbearia.Dto.User;
using Barbearia.Models;
using Barbearia.Models.Enums;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Barbearia.Services.User
{
    public class UserService : IUserInterface
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseModel<List<UserModel>>> CreateAdminUser(CreateUserDto createUserAdminDto)
        {
                ResponseModel<List<UserModel>> response = new ResponseModel<List<UserModel>>();

                try
                {
                    // Validar se o e-mail já está em uso
                    var existingUser = await _context.Users.SingleOrDefaultAsync(u => u.Email == createUserAdminDto.Email);
                    if (existingUser != null)
                    {
                        response.Message = "E-mail já está em uso.";
                        response.Status = false;
                        return response;
                    }

                    // Hash da senha
                    var passwordHash = BCrypt.Net.BCrypt.HashPassword(createUserAdminDto.Password);

                    var user = new UserModel()
                    {
                        Name = createUserAdminDto.Name,
                        Email = createUserAdminDto.Email,
                        Password = passwordHash,
                        Role = (RoleEnums.Admin),
                        EmailConfirmationToken = Guid.NewGuid().ToString(),
                        EmailConfirmed = true,
                    };

                    _context.Add(user);
                    await _context.SaveChangesAsync();

                    response.Message = "Usuário cadastrado com sucesso!";
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
        

        public async Task<ResponseModel<List<UserModel>>> DeleteUser(int id)
        {
            ResponseModel<List<UserModel>> response = new ResponseModel<List<UserModel>>();

            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

                if(user == null)
                {
                    response.Message = "Nenhum usuário localizado!";
                    return response;
                }
                _context.Remove(user);
                await _context.SaveChangesAsync();

                response.Dados = await _context.Users.ToListAsync();
                response.Message = "Usuário excluido com sucesso!";
                return response;
            }
            catch(Exception ex) 
            {
                response.Message = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<UserModel>> FindUserById(int id)
        {
            ResponseModel<UserModel> response = new ResponseModel<UserModel>();
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(uDataBase => uDataBase.Id == id);

                if (user == null)
                {
                    response.Message = "Nenhum cliente localizado.";
                    return response;
                }

                response.Dados = user;
                response.Message = "Cliente localizado!";
                return response;

            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<UserModel>> FindUserByIdSchedule(int id)
        {
            ResponseModel<UserModel> response = new ResponseModel<UserModel>();
            try
            {
                var schedule = await _context.Schedules
                    .Include(u => u.User)
                    .FirstOrDefaultAsync(SDataBase => SDataBase.Id == id);

                if (schedule == null)
                {
                    response.Message = "Nenhum registro localizado.";
                    return response;
                }
                response.Dados = schedule.User;
                response.Message = "Sucesso! Usuario localizado.";
                return response;

            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<List<UserModel>>> ListUsersClient()
        {
            ResponseModel<List<UserModel>> response = new ResponseModel<List<UserModel>>();
            try
            {
                var users = await _context.Users.Where(u => u.Role == RoleEnums.Client).ToListAsync();
                response.Dados = users;
                response.Message = "Sucesso! Todos os clientes coletados.";
                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = false;
                return response;
            }
        }
        public async Task<ResponseModel<List<UserModel>>> ListUsersAdmin()
        {
            ResponseModel<List<UserModel>> response = new ResponseModel<List<UserModel>>();
            try
            {
                var users = await _context.Users.Where(u => u.Role == RoleEnums.Admin).ToListAsync();
                response.Dados = users;
                response.Message = "Sucesso! Todos os Admins coletados.";
                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<List<UserModel>>> UpdateUser(UpdateUserDto updateUserDto)
        {
            ResponseModel<List<UserModel>> response = new ResponseModel<List<UserModel>> ();
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == updateUserDto.Id);
                if (user == null)
                {
                    response.Message = "Nenhum usuário localizado!";
                    return response;
                }
                user.Name = updateUserDto.Name;
                user.Email = updateUserDto.Email;
                user.Role = updateUserDto.Role;

                _context.Update(user);
                await _context.SaveChangesAsync();

                response.Dados = await _context.Users.ToListAsync();
                response.Message = "Dados do Usuário editado com sucesso!";
                return response;

            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<UserPointsDto>> ListPointsByUserId(int userId)
        {
            ResponseModel<UserPointsDto> response = new ResponseModel<UserPointsDto>();

            try
            {
                var user = await _context.Users
                    .Where(u => u.Id == userId)
                    .Include(u => u.Points)
                    .FirstOrDefaultAsync();

                if (user == null)
                {
                    response.Message = "Usuário não encontrado.";
                    response.Status = false;
                    return response;
                }

                var pointsAmount = user.Points?.Sum(p => p.Amount) ?? 0;

                var userPointsDto = new UserPointsDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    PointsAmount = pointsAmount
                };

                response.Dados = userPointsDto;
                response.Message = "Pontos do usuário coletados com sucesso.";
                response.Status = true;
                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = false;
                return response;
            }
        }
    }
}
