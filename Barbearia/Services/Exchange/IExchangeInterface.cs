using Barbearia.Models;

namespace Barbearia.Services.Exchange
{
    public interface IExchangeInterface
    {
        Task<ResponseModel<List<ExchangeModel>>> ListExchangesByDate(DateTime dateIni, DateTime dateFim);
        Task<ResponseModel<List<ExchangeModel>>> GetExchangeByCurrentUser(DateTime dateIni, DateTime dateFim);
        Task<ResponseModel<ExchangeModel>> GetExchangeByToken(string token);
        Task<ResponseModel<ExchangeModel>> CreateExchange(int userId, int productId);
        Task<ResponseModel<ExchangeModel>> ConfirmExchange(int exchangeId);
    }
}
