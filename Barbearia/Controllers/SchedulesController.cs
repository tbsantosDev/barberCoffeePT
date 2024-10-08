using Barbearia.Data;
using Barbearia.Dto.Schedule;
using Barbearia.Models;
using Barbearia.Services.Schedule;
using Barbearia.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Barbearia.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SchedulesController : ControllerBase
    {
        private readonly IScheduleInterface _scheduleInterface;
        public SchedulesController(IScheduleInterface scheduleInterface)
        {
            _scheduleInterface = scheduleInterface;
        }

        [HttpGet("ListSchedules")]
        public async Task<ActionResult<ResponseModel<List<ScheduleModel>>>> ListSchedules(DateTime dateIni, DateTime dateFim, int baberId)
        {
            var schedules = await _scheduleInterface.ListSchedules(dateIni, dateFim, baberId);
            return Ok(schedules);
        }

        [HttpGet("AvailableSlots")]
        public async Task<ActionResult<ResponseModel<List<TimeSpan>>>> GetAvailableSlots(DateTime date, int barberId)
        {
            var availableSlots = await _scheduleInterface.GetAvailableSlots(date, barberId);
            return Ok(availableSlots);
        }

        [HttpPost("CreateSchedule")]
        public async Task<ActionResult<ResponseModel<List<ScheduleModel>>>> CreateSchedule(CreateScheduleDto createScheduleDto)
        {
            var schedule = await _scheduleInterface.CreateSchedule(createScheduleDto);
            return Ok(schedule);
        }

        [HttpPut("UpdateSchedule")]
        public async Task<ActionResult<ResponseModel<List<ScheduleModel>>>> UpdateSchedule(UpdateScheduleDto updateScheduleDto)
        {
            var schedule = await _scheduleInterface.UpdateSchedule(updateScheduleDto);
            return Ok(schedule);
        }

        [HttpDelete("DeleteSchedule")]
        public async Task<ActionResult<ResponseModel<List<ScheduleModel>>>> DeleteSchedule(int id)
        {
            var schedule = await _scheduleInterface.DeleteSchedule(id);
            return Ok(schedule);
        }
    }
}
