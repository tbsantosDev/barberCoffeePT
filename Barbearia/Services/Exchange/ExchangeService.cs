using Barbearia.Data;
using Barbearia.Models;
using Barbearia.Models.Enums;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Barbearia.Services.Exchange
{
    public class ExchangeService : IExchangeInterface
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ExchangeService(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResponseModel<ExchangeModel>> ConfirmExchange(int exchangeId)
        {
            ResponseModel<ExchangeModel> response = new ResponseModel<ExchangeModel>();

            try
            {
                // Carrega a troca junto com o usuário e pontos
                var exchange = await _context.Exchanges
                    .Include(e => e.User)
                    .ThenInclude(u => u.Points)
                    .Include(e => e.Product)
                    .FirstOrDefaultAsync(e => e.Id == exchangeId);

                if (exchange == null)
                {
                    response.Message = "Troca não encontrada.";
                    response.Status = false;
                    return response;
                }

                if (exchange.Status == ExchangeEnums.Confirmed)
                {
                    response.Message = "Esta troca já foi confirmada.";
                    response.Status = false;
                    return response;
                }

                var user = exchange.User;
                var product = exchange.Product;

                if (user == null || user.Points == null)
                {
                    response.Message = "Usuário ou pontos não encontrados.";
                    response.Status = false;
                    return response;
                }

                // Calcula o total de pontos
                var totalPoints = user.Points.Sum(p => p.Amount);
                if (totalPoints < product.AmountInPoints)
                {
                    response.Message = "Usuário não possui pontos suficientes.";
                    response.Status = false;
                    return response;
                }

                // Subtração dos pontos
                var pointsToDeduct = product.AmountInPoints;
                foreach (var point in user.Points.OrderBy(p => p.DateTime))
                {
                    if (pointsToDeduct <= 0) break;

                    if (point.Amount >= pointsToDeduct)
                    {
                        point.Amount -= pointsToDeduct;
                        pointsToDeduct = 0;
                    }
                    else
                    {
                        pointsToDeduct -= point.Amount;
                        point.Amount = 0;
                    }
                }

                // Atualiza o status da troca
                exchange.Status = ExchangeEnums.Confirmed;
                exchange.ConfirmedAt = DateTime.Now;

                // Salva as alterações no banco
                _context.Exchanges.Update(exchange);
                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                response.Dados = exchange;
                response.Message = "Troca confirmada e pontos subtraídos com sucesso!";
                response.Status = true;
                return response;
            }
            catch (Exception ex)
            {
                response.Message = $"Erro: {ex.Message}";
                response.Status = false;
                return response;
            }
        }


        public async Task<ResponseModel<ExchangeModel>> CreateExchange(int userId, int productId)
        {
            ResponseModel<ExchangeModel> response = new ResponseModel<ExchangeModel>();

            try
            {
                var user = await _context.Users.Include(u => u.Points).FirstOrDefaultAsync(u => u.Id == userId);
                if (user == null)
                {
                    response.Message = "Usuário não encontrado.";
                    response.Status = false;
                    return response;
                }

                var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);
                if (product == null)
                {
                    response.Message = "Produto não encontrado.";
                    response.Status = false;
                    return response;
                }

                var totalPoints = user.Points.Sum(p => p.Amount);
                if (totalPoints < product.AmountInPoints)
                {
                    response.Message = "Pontos insuficientes para realizar a troca.";
                    response.Status = false;
                    return response;
                }

                var hasPendingExchange = await _context.Exchanges
                    .AnyAsync(e => e.UserId == userId && e.ProductId == productId && e.Status == ExchangeEnums.Pending);

                if (hasPendingExchange)
                {
                    response.Message = "Você já tem uma troca pendente para este item.";
                    response.Status = false;
                    return response;
                }

                var token = GenerateToken();

                var exchange = new ExchangeModel
                {
                    ExchangeDate = DateTime.Now,
                    UserId = userId,
                    ProductId = productId,
                    User = user,
                    Product = product,
                    Token = token,
                    Status = ExchangeEnums.Pending
                };

                _context.Exchanges.Add(exchange);
                await _context.SaveChangesAsync();

                response.Dados = exchange;
                response.Message = "Troca Registrada, aguarde confirmação do Administrador, token gerado: " + token;
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


        public async Task<ResponseModel<List<ExchangeModel>>> GetExchangeByCurrentUser(DateTime dateIni, DateTime dateFim)
        {
            ResponseModel<List<ExchangeModel>> response = new ResponseModel<List<ExchangeModel>>();

            try
            {
                var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    response.Message = "Usuário não encontrado.";
                    response.Status = false;
                    return response;
                }

                var userId = int.Parse(userIdClaim.Value);

                var exchanges = await _context.Exchanges
                    .Where(d => d.ExchangeDate >= dateIni && d.ExchangeDate <= dateFim && d.UserId == userId)
                    .Include(d => d.User)
                    .OrderBy(d => d.ExchangeDate)
                    .ToListAsync();

                if (exchanges.Count == 0)
                {
                    response.Message = "Nenhuma troca de pontos registrada para este cliente.";
                }

                response.Dados = exchanges;
                response.Message = "Todos os registros de trocas coletados.";
                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<List<ExchangeModel>>> ListExchangesByDate(DateTime dateIni, DateTime dateFim)
        {
            ResponseModel<List<ExchangeModel>> response = new ResponseModel<List<ExchangeModel>>();

            try
            {
                var exchanges = await _context.Exchanges.Where(d => d.ExchangeDate.Date >= dateIni && d.ExchangeDate.Date <= dateFim).ToListAsync();
                if (exchanges.Count <= 0)
                {
                    response.Message = "Nenhuma troca de pontos realizada nesse periodo.";
                    return response;
                }
                response.Dados = exchanges;
                response.Message = "Todos as trocas de pontos deste periodo foi coletada.";
                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<ExchangeModel>> GetExchangeByToken(string token)
        {
            ResponseModel<ExchangeModel> response = new ResponseModel<ExchangeModel>();

            try
            {
                var getToken = await _context.Exchanges.FirstOrDefaultAsync(d => d.Token == token);
                if (token == null)
                {
                    response.Message = "Token não encontrado!";
                    return response;
                }

                response.Dados = getToken;
                response.Message = "Token localizado com sucesso!";
                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = false;
                return response;
            }
        }
        private static string GenerateToken(int length = 6)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
