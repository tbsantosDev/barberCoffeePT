using Barbearia.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Barbearia.Models
{
    public class ExchangeModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "A data da troca é obrigatória.")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime ExchangeDate { get; set; }
        [Required]
        public string Token { get; set; }
        [Required]
        public ExchangeEnums Status { get; set; }
        public DateTime? ConfirmedAt { get; set; }

        // Chave estrangeira para o cliente
        [Required(ErrorMessage = "O Id do usuário é obrigatório.")]
        public int UserId { get; set; }

        [JsonIgnore]
        public UserModel User { get; set; }

        // Chave estrangeira para o produto
        [Required(ErrorMessage = "O Id do produto é obrigatório.")]
        public int ProductId { get; set; }

        [JsonIgnore]
        public ProductModel Product { get; set; }
    }
}
