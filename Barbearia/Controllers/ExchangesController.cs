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

        [HttpGet("GetExchangeByCurrentUser")]
        public async Task<ActionResult<ResponseModel<List<ExchangeModel>>>> GetExchangeByCurrentUser(DateTime dateIni, DateTime dateFim)
        {
            var exchanges = await _exchangeInterface.GetExchangeByCurrentUser(dateIni, dateFim);
            return Ok(exchanges);
        }

        [HttpGet("GetExchangeByToken")]
        public async Task<ActionResult<ResponseModel<ExchangeModel>>> GetExchangeByToken(string token)
        {
            Console.WriteLine($"Parametro de localizar token: {token}");
            var exchange = await _exchangeInterface.GetExchangeByToken(token);
            return Ok(exchange);
        }

        [HttpPost("CreateExchange")]
        public async Task<ActionResult<ResponseModel<List<ExchangeModel>>>> CreateExchange([FromBody] ExchangeRequestModel request)
        {
            var exchange = await _exchangeInterface.CreateExchange(request.UserId, request.ProductId);
            return Ok(exchange);
        }

        [HttpPut("ConfirmExchange")]
        public async Task<ActionResult<ResponseModel<List<ExchangeModel>>>> ConfirmExchange(int exchangeId)
        {
            var exchange = await _exchangeInterface.ConfirmExchange(exchangeId);
            return Ok(exchange);
        }

        public class ExchangeRequestModel
        {
            public int UserId { get; set; }
            public int ProductId { get; set; }
        }
    }
}
