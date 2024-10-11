using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;
using System.Text.Json.Serialization;

namespace Barbearia.Models
{
    public class ProductModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public byte[] Image  { get; set; }
        [Required]
        public int AmountInPoints { get; set; }
        [JsonIgnore]
        public ExchangeModel Exchanges { get; set; }
    }
}
