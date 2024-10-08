using Barbearia.Dto.Schedule;
using Barbearia.Models;

namespace Barbearia.Services.Schedule
{
    public interface IScheduleInterface
    {
        Task<ResponseModel<List<ScheduleModel>>> ListSchedules(DateTime dateIni, DateTime dateFim, int barberId);
        Task<ResponseModel<List<DateTime>>> GetAvailableSlots(DateTime date, int barberId);
        Task<bool> IsSlotAvailable(DateTime dateTime, int barberId );
        Task<ResponseModel<List<ScheduleModel>>> CreateSchedule(CreateScheduleDto createScheduleDto);
        Task<ResponseModel<ScheduleModel>> UpdateSchedule(UpdateScheduleDto updateScheduleDto);
        Task<ResponseModel<ScheduleModel>> DeleteSchedule(int id);
    }
}
