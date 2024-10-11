using Barbearia.Models;
using System.ComponentModel.DataAnnotations;

namespace Barbearia.Dto.Schedule
{
    public class CreateScheduleDto
    {
        [Required(ErrorMessage = "A data do agendamento é obrigatória.")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime DateTime { get; set; }
        [Required]
        public int BarberId { get; set; }
    }
}
