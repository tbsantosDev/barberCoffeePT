namespace Barbearia.Dto.Product
{
    public class CreateProductDto
    {
        public string Name { get; set; }
        public IFormFile Image { get; set; }
        public int AmountInPoints { get; set; }
    }
}
