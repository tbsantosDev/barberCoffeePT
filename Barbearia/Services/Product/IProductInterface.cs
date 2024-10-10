using Barbearia.Dto.Product;
using Barbearia.Models;

namespace Barbearia.Services.Product
{
    public interface IProductInterface
    {
        Task<ResponseModel<List<ProductModel>>> ListProducts();
        Task<ResponseModel<ProductModel>> ProductsById(int id);
        Task<ResponseModel<ProductModel>> CreateProduct(CreateProductDto createProductDto);
        Task<ResponseModel<ProductModel>> UpdateProduct(UpdateProductDto updateProductDto);
        Task<ResponseModel<ProductModel>> DeleteProduct(int id);

    }
}
