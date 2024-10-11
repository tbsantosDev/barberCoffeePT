using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Barbearia.Models
{
    public class PointModel
    {
        public int Id { get; set; }
        [Required]
        public int Amount { get; set; }
        [Required(ErrorMessage = "A data da inserção dos pontos é obrigatória.")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime DateTime { get; set; }
        //Chave estrangeira para o cliente
        public int UserId { get; set; }
        public UserModel User { get; set; }
    }
}
