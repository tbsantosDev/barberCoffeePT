using System.Text.Json.Serialization;

namespace Barbearia.Models
{
    public class BarberModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [JsonIgnore]
        public ICollection<ScheduleModel> Schedules { get; set; }
    }
}
