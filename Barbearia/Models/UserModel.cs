using Barbearia.Models.Enums;
using System.Text.Json.Serialization;

namespace Barbearia.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public RoleEnums Role { get; set; }
        public bool EmailConfirmed { get; set; } = false;
        public string EmailConfirmationToken { get; set; }
        [JsonIgnore]
        public ICollection<ScheduleModel> Schedules { get; set; }
        [JsonIgnore]
        public ICollection<PointModel> Points { get; set; }
    }
}
