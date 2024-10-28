using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Barbearia.Models
{
    public class CategoryModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [JsonIgnore]
        public ICollection<ProductModel> Products { get; set; }
    }
}
