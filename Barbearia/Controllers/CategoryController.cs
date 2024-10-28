using Barbearia.Dto.Category;
using Barbearia.Models;
using Barbearia.Services.Category;
using Barbearia.Services.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Barbearia.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryInterface _categoryInterface;
        public CategoryController(ICategoryInterface categoryInterface)
        {
            _categoryInterface = categoryInterface;
        }

        [HttpGet("ListCategories")]
        public async Task<ActionResult<ResponseModel<List<CategoryModel>>>> ListCategories()
        {
            var categories = await _categoryInterface.ListCategories();
            return Ok(categories);
        }

        [HttpGet("CategoryById/{id}")]
        public async Task<ActionResult<ResponseModel<CategoryModel>>> CategoryById(int id)
        {
            var category = await _categoryInterface.CategoryById(id);
            return Ok(category);
        }

        [HttpPost("CreateCategory")]
        public async Task<ActionResult<ResponseModel<CategoryModel>>> CreateCategory(CreateCategoryDto createCategoryDto)
        {
            var category = await _categoryInterface.CreateCategory(createCategoryDto);
            return Ok(category);
        }

        [HttpPut("UpdateCategory")]
        public async Task<ActionResult<ResponseModel<CategoryModel>>> UpdateCategory(UpdateCategoryDto updateCategoryDto)
        {
            var category = await _categoryInterface.UpdateCategory(updateCategoryDto);
            return Ok(category);
        }

        [HttpDelete("DeleteCategory/{id}")]
        public async Task<ActionResult<ResponseModel<CategoryModel>>> DeleteCategory(int id)
        {
            var category = await _categoryInterface.DeleteCategory(id);
            return Ok(category);
        }
    }
}
