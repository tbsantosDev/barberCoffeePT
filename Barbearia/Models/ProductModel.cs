namespace Barbearia.Models
{
    public class ProductModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int AmountInPoints { get; set; }
        public ExchangeModel Exchanges { get; set; }
    }
}
