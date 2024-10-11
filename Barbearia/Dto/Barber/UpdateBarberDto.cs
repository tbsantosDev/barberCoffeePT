using System.ComponentModel.DataAnnotations;

namespace Barbearia.Dto.Barber
{
    public class UpdateBarberDto
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
