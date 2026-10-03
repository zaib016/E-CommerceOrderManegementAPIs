using E_CommerceOrderManagementAPI.Models.DTOs;
using E_CommerceOrderManagementAPI.Models.Entities;
using E_CommerceOrderManagementAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_CommerceOrderManagementAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private IGenericRepository<Category> _CategoryRepo;

        public CategoryController(IGenericRepository<Category> genericRepository)
        {
            _CategoryRepo = genericRepository;
        }
        [HttpGet("GetCategories")]
        public async Task<IActionResult> GetList()
        {
            return Ok(await _CategoryRepo.GetListAsync());
        }
        [HttpGet("GetCategoryById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _CategoryRepo.GetByIdAsync(id);
            if (category == null) return NotFound();

            return Ok(category);
        }
        [HttpPost("AddCategory")]
        public async Task<IActionResult> AddCategory(CategoryDTOs categoryDTOs)
        {
            var category = new Category
            {
                CategoryName = categoryDTOs.CategoryName,
                Description = categoryDTOs.Description,
            };

            await _CategoryRepo.AddAsync(category);
            return Ok(category);
        }
        [HttpPut("UpdateCategory/{id}")]
        public async Task<IActionResult> updateCategory(CategoryDTOs categoryDTOs, int id)
        {
            var category = await _CategoryRepo.GetByIdAsync(id);
            if (category == null) return NotFound();

            category.CategoryName = categoryDTOs.CategoryName;
            category.Description = categoryDTOs.Description;

            await _CategoryRepo.UpdateAsync(category);
            return Ok(category);
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> delete(int id)
        {
            var category = await _CategoryRepo.DeleteAsync(id);
            if (category == false) return NotFound();

            return Ok("Category Deleted!!");
        }
    }
}
