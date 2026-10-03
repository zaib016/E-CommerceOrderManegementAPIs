using E_CommerceOrderManagementAPI.Migrations;
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
    public class ProductController : ControllerBase
    {
        private IGenericRepository<Product> _productRepo;

        public ProductController(IGenericRepository<Product> genericRepository)
        {
            _productRepo = genericRepository;
        }

        [HttpGet("GetPaginatedListProducts")]
        public async Task<IActionResult> GetAll([FromQuery]PaginationAndSortingDTOs pagination)
        {
            var query = await _productRepo.GetListAsync();
            var result = PaginatedListDTOs<Product>.FromIQueryable(query.AsQueryable(), pagination);

            return Ok(result);
        }
        [HttpGet("GetProductById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _productRepo.GetByIdAsync(id);
            if (product == null) return NotFound();

            return Ok(product);
        }
        [HttpPost("AddProduct")]
        public async Task<IActionResult> AddProduct(ProductDTOs productDTOs)
        {
            var product = new Product
            {
                CategoryId = productDTOs.CategoryId,
                ProductName = productDTOs.ProductName,
                ProductDescription = productDTOs.ProductDescription,
                Stock = productDTOs.Stock,
                Price = productDTOs.Price,
            };

            await _productRepo.AddAsync(product);
            return Ok(productDTOs);
        }
        [HttpPut("UpdateProduct/{id}")]
        public async Task<IActionResult> updateProduct(ProductDTOs productDTOs, int id)
        {
            var product = await _productRepo.GetByIdAsync(id);
            if (product == null) return NotFound();

            product.CategoryId = productDTOs.CategoryId;
            product.ProductName = productDTOs.ProductName;
            product.ProductDescription = productDTOs.ProductDescription;
            product.Stock = productDTOs.Stock;
            product.Price = productDTOs.Price;

            await _productRepo.UpdateAsync(product);
            return Ok(productDTOs);
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> deleteProduct(int id)
        {
            var product = await _productRepo.GetByIdAsync(id);
            if (product == null) return NotFound();

            return Ok("Product Deleted!!");
        }
    }
}
