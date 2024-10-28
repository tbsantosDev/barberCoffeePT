using Barbearia.Dto.Category;
using Barbearia.Dto.Product;
using Barbearia.Models;

namespace Barbearia.Services.Category
{
    public interface ICategoryInterface
    {
        Task<ResponseModel<List<CategoryModel>>> ListCategories();
        Task<ResponseModel<CategoryModel>> CategoryById(int id);
        Task<ResponseModel<CategoryModel>> CreateCategory(CreateCategoryDto createCategoryDto);
        Task<ResponseModel<CategoryModel>> UpdateCategory(UpdateCategoryDto updateCategoryDto);
        Task<ResponseModel<CategoryModel>> DeleteCategory(int id);
    }
}
