namespace Barbearia.Dto.Schedule
{
    public class UpdateScheduleDto
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public int BarberId { get; set; }
    }
}
