using System.ComponentModel.DataAnnotations;

namespace Barbearia.Dto.Category
{
    public class CreateCategoryDto
    {
        [Required]
        public string Name { get; set; }
    }
}
