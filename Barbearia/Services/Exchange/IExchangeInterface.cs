using Barbearia.Models;

namespace Barbearia.Services.Exchange
{
    public interface IExchangeInterface
    {
        Task<ResponseModel<List<ExchangeModel>>> ListExchangesByDate(DateTime dateIni, DateTime dateFim);
        Task<ResponseModel<List<ExchangeModel>>> GetExchangeByUserId(DateTime dateIni, DateTime dateFim, int userId);
        Task<ResponseModel<ExchangeModel>> CreateExchange(int userId, int productId);
    }
}
