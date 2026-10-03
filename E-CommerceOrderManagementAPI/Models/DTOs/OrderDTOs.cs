using E_CommerceOrderManagementAPI.Models.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_CommerceOrderManagementAPI.Models.DTOs
{
    public class OrderDTOs
    {
        [ForeignKey("UserId")]
        public int UserId { get; set; }
        [ForeignKey("ProductId")]
        public int ProductId { get; set; }
        public string OrderItem { get; set; }
        public int Quantity { get; set; }
        public string OrderAmount { get; set; }
        public string Status { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
    }
}
