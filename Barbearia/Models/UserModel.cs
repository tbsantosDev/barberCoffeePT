using Barbearia.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Barbearia.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        [Required]
        public string Password { get; set; }
        public RoleEnums Role { get; set; }
        public bool EmailConfirmed { get; set; } = false;
        public string? EmailConfirmationToken { get; set; }
        public string? PasswordResetToken { get; set; }
        public DateTime? PasswordResetTokenExpires { get; set; }
        [JsonIgnore]
        public ICollection<ScheduleModel> Schedules { get; set; }
        [JsonIgnore]
        public ICollection<PointModel> Points { get; set; }
    }
}
