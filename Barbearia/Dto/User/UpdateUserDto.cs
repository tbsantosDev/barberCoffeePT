using Barbearia.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Barbearia.Dto.User
{
    public class UpdateUserDto
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
