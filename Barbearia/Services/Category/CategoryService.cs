using Barbearia.Data;
using Barbearia.Dto.Category;
using Barbearia.Models;
using Microsoft.EntityFrameworkCore;

namespace Barbearia.Services.Category
{
    public class CategoryService : ICategoryInterface
    {
        private readonly AppDbContext _context;
        public CategoryService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<ResponseModel<CategoryModel>> CategoryById(int id)
        {
            ResponseModel<CategoryModel> response = new ResponseModel<CategoryModel>();

            try
            {
                var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
                if (category == null)
                {
                    response.Message = "Nenhuma categoria encontrada.";
                    return response;
                }

                response.Dados = category;
                response.Message = "Categoria encontrada com sucesso!";
                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<CategoryModel>> CreateCategory(CreateCategoryDto createCategoryDto)
        {
            ResponseModel<CategoryModel> response = new ResponseModel<CategoryModel>();

            try
            {
                var newCategory = new CategoryModel()
                {
                    Name = createCategoryDto.Name,
                };
                _context.Add(newCategory);
                await _context.SaveChangesAsync();

                response.Dados = newCategory;
                response.Message = "Categoria criada com sucesso!";
                return response;
            }
            catch (Exception ex)
            {
                response.Message += ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<CategoryModel>> DeleteCategory(int id)
        {
            ResponseModel<CategoryModel> response = new ResponseModel<CategoryModel>();

            try
            {
                var deleteCategory = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
                if (deleteCategory == null)
                {
                    response.Message = "Nenhuma categoria encontrada.";
                    return response;
                }
                _context.Remove(deleteCategory);
                await _context.SaveChangesAsync();

                response.Dados = deleteCategory;
                response.Message = "Categoria excluida com sucesso!";
                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<List<CategoryModel>>> ListCategories()
        {
            ResponseModel<List<CategoryModel>> response = new ResponseModel<List<CategoryModel>>();

            try
            {
                var categories = await _context.Categories.ToListAsync();

                response.Dados = categories;
                response.Message = "Todas as categorias coletadas.";
                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<CategoryModel>> UpdateCategory(UpdateCategoryDto updateCategoryDto)
        {
            ResponseModel<CategoryModel> response = new ResponseModel<CategoryModel>();

            try
            {
                var updateCategory = await _context.Categories.FirstOrDefaultAsync(c => c.Id == updateCategoryDto.Id);
                if (updateCategory == null)
                {
                    response.Message = "Categoria não existe.";
                    return response;
                }
                updateCategory.Name = updateCategoryDto.Name;

                _context.Update(updateCategory);
                await _context.SaveChangesAsync();

                response.Dados = updateCategory;
                response.Message = "Categoria atualizada com sucesso!";
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
