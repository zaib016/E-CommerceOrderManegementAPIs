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
    public class OrderController : ControllerBase
    {
        private IGenericRepository<Order> _orderRepo;
        private ICacheRepository _cache;

        public OrderController(IGenericRepository<Order> genericRepository, ICacheRepository cacheRepository)
        {
            _orderRepo = genericRepository;
            _cache = cacheRepository;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("GetOrderList")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _orderRepo.GetListAsync());
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("GetOrderById/{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _orderRepo.GetByIdAsync(id);
            if (order == null) return NotFound();

            return Ok(order);
        }
        [HttpPost("AddOrder")]
        public async Task<IActionResult> AddOrder(OrderDTOs orderDTOs,
            [FromHeader(Name = "Idempotency-Key")] string idempotencyKey)
        {
            var key = $"idempotency:{idempotencyKey}";
            var existingOrder = _cache.Get<Order>(key);

            if(existingOrder != null)
            {
                return Ok(existingOrder);
            }

            var order = new Order
            {
                UserId = orderDTOs.UserId,
                ProductId = orderDTOs.ProductId,
                OrderItem = orderDTOs.OrderItem,
                Quantity = orderDTOs.Quantity,
                OrderAmount = orderDTOs.OrderAmount,
                Status = orderDTOs.Status,
            };

            var orderData = await _orderRepo.AddAsync(order);
            _cache.Set(key, orderData, 10);
            return Ok(orderData);
        }
        [HttpPut("UpdateOrder/{id}")]
        public async Task<IActionResult> UpdateOrder(OrderDTOs orderDTOs, int id)
        {
            var order = await _orderRepo.GetByIdAsync(id);
            if (order == null) return NotFound();

            order.UserId = orderDTOs.UserId;
            order.ProductId = orderDTOs.ProductId;
            order.OrderItem = orderDTOs.OrderItem;
            order.Quantity = orderDTOs.Quantity;
            order.OrderAmount = orderDTOs.OrderAmount;
            order.Status = orderDTOs.Status;

            await _orderRepo.UpdateAsync(order);
            return Ok(order);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _orderRepo.GetByIdAsync(id);
            if (order == null) return NotFound();

            return Ok("Order Deleted!!");
        }
    }
}
