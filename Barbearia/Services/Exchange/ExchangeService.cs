using Barbearia.Data;
using Barbearia.Models;
using Microsoft.EntityFrameworkCore;

namespace Barbearia.Services.Exchange
{
    public class ExchangeService : IExchangeInterface
    {
        private readonly AppDbContext _context;
        public ExchangeService(AppDbContext context)
        {
            _context = context;
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

                var exchange = new ExchangeModel
                {
                    ExchangeDate = DateTime.Now,
                    UserId = userId,
                    ProductId = productId,
                    User = user,
                    Product = product
                };

                _context.Exchanges.Add(exchange);

                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                response.Dados = exchange;
                response.Message = "Troca realizada com sucesso!";
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

        public async Task<ResponseModel<List<ExchangeModel>>> GetExchangeByUserId(DateTime dateIni, DateTime dateFim, int userId)
        {
            ResponseModel<List<ExchangeModel>> response = new ResponseModel<List<ExchangeModel>>();

            try
            {
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
                response.Message = "Todos os registros de trocas deste cliente no determinado periodo coletados.";
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
    }
}
