using System.Collections.Generic;

namespace Barbearia.Models
{
    public class PointModel
    {
        public int Id { get; set; }
        public int Amount { get; set; }
        public DateTime DateTime { get; set; }
        //Chave estrangeira para o cliente
        public int UserId { get; set; }
        public UserModel User { get; set; }
    }
}
