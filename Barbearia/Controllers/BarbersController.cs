using Barbearia.Dto.Barber;
using Barbearia.Models;
using Barbearia.Services.Barber;
using Barbearia.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Barbearia.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BarbersController : ControllerBase
    {
        private readonly IBarberInterface _barberInterface;
        public BarbersController(IBarberInterface barberInterface)
        {
            _barberInterface = barberInterface;
        }

        [HttpGet("ListBarbers")]
        public async Task<ActionResult<ResponseModel<List<BarberModel>>>> ListBarbers()
        {
            var barbers = await _barberInterface.ListBarbers();
            return Ok(barbers);
        }

        [HttpGet("FindBarberById/{id}")]
        public async Task<ActionResult<ResponseModel<BarberModel>>> FindBarberById(int id)
        {
            var barber = await _barberInterface.FindBarberById(id);
            return Ok(barber);
        }

        [HttpPost("CreateBarber")]
        public async Task<ActionResult<ResponseModel<List<BarberModel>>>> CreateBarber(CreateBarberDto createBarberDto)
        {
            var barber = await _barberInterface.CreateBarber(createBarberDto);
            return Ok(barber);
        }

        [HttpDelete("DeleteBarber")]
        public async Task<ActionResult<ResponseModel<List<BarberModel>>>> DeleteBarber(int id)
        {
            var barber = await _barberInterface.DeleteBarber(id);
            return Ok(barber);
        }

        [HttpPut("UpdateBarber")]
        public async Task<ActionResult<ResponseModel<List<BarberModel>>>> UpdateBarber(UpdateBarberDto updateBarberDto)
        {
            var barber = await _barberInterface.UpdateBarber(updateBarberDto);
            return Ok(barber);
        }
    }
}
