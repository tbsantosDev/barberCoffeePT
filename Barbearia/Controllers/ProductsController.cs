using Barbearia.Dto.Product;
using Barbearia.Models;
using Barbearia.Services.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Barbearia.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductInterface _productInterface;

        public ProductsController(IProductInterface productInterface)
        {
            _productInterface = productInterface;
        }

        [HttpGet("ListProducts")]
        public async Task<ActionResult<ResponseModel<List<ProductModel>>>> ListProducts() { 
            var products = await _productInterface.ListProducts();
            return Ok(products);
        }

        [HttpGet("ProductsById/{id}")]
        public async Task<ActionResult<ResponseModel<ProductModel>>> ProductsById(int id)
        {
            var product = await _productInterface.ProductsById(id);
            return Ok(product);
        }
        [HttpGet("ListProductsByCategoryId/{id}")]
        public async Task<ActionResult<ResponseModel<List<ProductModel>>>> ListProductsByCategoryId(int id)
        {
            var products = await _productInterface.ListProductsByCategoryId(id);
            return Ok(products);
        }
        [HttpPost("CreateProduct")]
        public async Task<ActionResult<ResponseModel<ProductModel>>> CreateProduct(CreateProductDto createProductDto)
        {
            var product = await _productInterface.CreateProduct(createProductDto);
            return Ok(product);
        }
        [HttpPut("UpdateProduct")]
        public async Task<ActionResult<ResponseModel<ProductModel>>> UpdateProduct(UpdateProductDto updateProductDto)
        {
            var product = await _productInterface.UpdateProduct(updateProductDto);
            return Ok(product);
        }
        [HttpDelete("DeleteProduct/{id}")]
        public async Task<ActionResult<ResponseModel<ProductModel>>> DeleteProduct(int id)
        {
            var product = await _productInterface.DeleteProduct(id);
            return Ok(product);
        }
    }
}
