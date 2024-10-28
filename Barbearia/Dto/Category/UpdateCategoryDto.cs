using System.ComponentModel.DataAnnotations;

namespace Barbearia.Dto.Category
{
    public class UpdateCategoryDto
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
