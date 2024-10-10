namespace Barbearia.Dto.Product
{
    public class UpdateProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IFormFile Image { get; set; }
        public int AmountInPoints { get; set; }
    }
}
