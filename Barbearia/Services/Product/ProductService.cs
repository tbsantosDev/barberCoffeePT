using Barbearia.Data;
using Barbearia.Dto.Product;
using Barbearia.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Barbearia.Services.Product
{
    public class ProductService : IProductInterface
    {
        private readonly AppDbContext _context;
        public ProductService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<ResponseModel<ProductModel>> CreateProduct([FromForm]CreateProductDto createProductDto)
        {
            ResponseModel<ProductModel> response = new ResponseModel<ProductModel>();

            try
            {
                if (createProductDto.Image != null && createProductDto.Image.Length > 0)
                {
                    using var memoryStream = new MemoryStream();
                    await createProductDto.Image.CopyToAsync(memoryStream);
                    var imageBytes = memoryStream.ToArray();

                    var product = new ProductModel
                    {
                        Name = createProductDto.Name,
                        Image = imageBytes,
                        AmountInPoints = createProductDto.AmountInPoints
                    };

                    _context.Products.Add(product);
                    await _context.SaveChangesAsync();

                    response.Message = "Produto criado com sucesso!";
                    response.Dados = product;
                    return response;
                }
                else
                {
                    response.Message = "Imagem é obrigatória";
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<ProductModel>> DeleteProduct(int id)
        {
            ResponseModel<ProductModel> response = new ResponseModel<ProductModel>();

            try
            {
                var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
                if (product == null)
                {
                    response.Message = "Produto não encontrado!";
                    return response;
                }
                _context.Remove(product);
                await _context.SaveChangesAsync();

                response.Dados = product;
                response.Message = "Produto removido com sucesso!";
                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<List<ProductModel>>> ListProducts()
        {
            ResponseModel<List<ProductModel>> response = new ResponseModel<List<ProductModel>>();

            try
            {
                var products = await _context.Products.ToListAsync();

                response.Dados = products;
                response.Message = "Todos os produtos coletados!";
                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<ProductModel>> ProductsById(int id)
        {
            ResponseModel<ProductModel> response = new ResponseModel<ProductModel>();

            try
            {
                var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
                if (product == null)
                {
                    response.Message = "Produto não encontrado!";
                    return response;
                }
                response.Dados = product;
                response.Message = "Produto coletado com sucesso!";
                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<ProductModel>> UpdateProduct(UpdateProductDto updateProductDto)
        {
            ResponseModel<ProductModel> response = new ResponseModel<ProductModel>();

            try
            {
                var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == updateProductDto.Id);
                if (product == null)
                {
                    response.Message = "Produto não existe.";
                    return response;
                }

                product.Name = updateProductDto.Name;
                product.AmountInPoints = updateProductDto.AmountInPoints;

                if (updateProductDto.Image != null && updateProductDto.Image.Length > 0)
                {
                    using var memoryStream = new MemoryStream();
                    await updateProductDto.Image.CopyToAsync(memoryStream);
                    product.Image = memoryStream.ToArray();
                }

                _context.Update(product);
                await _context.SaveChangesAsync();

                response.Dados = product;
                response.Message = "Produto atualizado com sucesso!";
                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = false;
                return response;
            }
        }
    }
}
