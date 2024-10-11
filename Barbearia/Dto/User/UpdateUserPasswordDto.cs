using System.ComponentModel.DataAnnotations;

namespace Barbearia.Dto.User
{
    public class UpdateUserPasswordDto
    {
        [Required]
        public string NewPassword { get; set; }
        [Required]
        public string PasswordConfirmation { get; set; }
    }
}
