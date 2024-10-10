using Barbearia.Models;
using Barbearia.Services.Point;
using Barbearia.Services.Schedule;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Barbearia.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PointsController : ControllerBase
    {
        private readonly IPointInterface _pointInterface;
        public PointsController(IPointInterface pointInterface)
        {
            _pointInterface = pointInterface;
        }

        [HttpGet("AmountPoints")]
        public async Task<ActionResult<ResponseModel<int>>> AmountPoints()
        {
            var points = await _pointInterface.AmountPoints();
            return Ok(points);
        }

        [HttpPost("CreatePoint")]
        public async Task<ActionResult<ResponseModel<List<PointModel>>>> CreatePoint(int scheduleId)
        {
            var point = await _pointInterface.CreatePoint(scheduleId);
            return Ok(point);
        }

        [HttpDelete("DeletePoint/{id}")]
        public async Task<ActionResult<ResponseModel<List<PointModel>>>> DeletePoint(int id)
        {
            var point = await _pointInterface.DeletePoint(id);
            return Ok(point);
        }

    }
}
