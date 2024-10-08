using Barbearia.Models;

namespace Barbearia.Dto.Schedule
{
    public class CreateScheduleDto
    {
        public DateTime DateTime { get; set; }
        public int BarberId { get; set; }
    }
}
