using System.ComponentModel.DataAnnotations;

namespace Barbearia.Dto.User
{
    public class UserPointsDto
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public int PointsAmount { get; set; }
    }
}
