using System.ComponentModel.DataAnnotations;

namespace Barbearia.Dto.Product
{
    public class CreateProductDto
    {
        [Required]
        public string Name { get; set; }
        public IFormFile Image { get; set; }
        [Required]
        public int AmountInPoints { get; set; }
        [Required]
        public int CategoryId { get; set; }
    }
}
