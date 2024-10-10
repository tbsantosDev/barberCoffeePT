using Barbearia.Models;
using Barbearia.Services.Exchange;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Barbearia.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ExchangesController : ControllerBase
    {
        private readonly IExchangeInterface _exchangeInterface;
        public ExchangesController(IExchangeInterface exchangeInterface)
        {
            _exchangeInterface = exchangeInterface;
        }

        [HttpGet("ListExchangesByDate")]
        public async Task<ActionResult<ResponseModel<List<ExchangeModel>>>> ListExchangesByDate(DateTime dateIni, DateTime dateFim)
        {
            var exchanges = await _exchangeInterface.ListExchangesByDate(dateIni, dateFim);
            return Ok(exchanges);
        }

        [HttpGet("GetExchangeByUserId")]
        public async Task<ActionResult<ResponseModel<List<ExchangeModel>>>> GetExchangeByUserId(DateTime dateIni, DateTime dateFim, int userId)
        {
            var exchanges = await _exchangeInterface.GetExchangeByUserId(dateIni, dateFim, userId);
            return Ok(exchanges);
        }

        [HttpPost("CreateExchange")]
        public async Task<ActionResult<ResponseModel<List<ExchangeModel>>>> CreateExchange(int userId, int productId)
        {
            var exchange = await _exchangeInterface.CreateExchange(userId, productId);
            return Ok(exchange);
        }
    }
}
