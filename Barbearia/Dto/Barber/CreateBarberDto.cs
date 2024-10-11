using System.ComponentModel.DataAnnotations;

namespace Barbearia.Dto.Barber
{
    public class CreateBarberDto
    {
        [Required]
        public string Name { get; set; }
    }
}
