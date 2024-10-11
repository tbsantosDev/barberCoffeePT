using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Barbearia.Models
{
    public class ScheduleModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "A data do agendamento é obrigatória.")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime DateTime { get; set; }
        [Required]
        public bool CutTheHair { get; set; } = false;
        //Chave estrangeira para o cliente
        public int UserId { get; set; }
        [JsonIgnore]
        public UserModel User { get; set; }
        //Chave estrangeira para o barbeiro
        public int BarberId { get; set; }
        [JsonIgnore]
        public BarberModel Barber { get; set; }
    }
}
