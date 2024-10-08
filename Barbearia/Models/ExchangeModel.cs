﻿namespace Barbearia.Models
{
    public class ExchangeModel
    {
        public int Id { get; set; }
        public DateTime ExchangeDate { get; set; }
        //Chave estrangeira para o cliente
        public int UserId { get; set; }
        public UserModel User { get; set; }
        //Chave estrangeira para o produto
        public int ProductId { get; set; }
        public ProductModel Product { get; set; }
    }
}