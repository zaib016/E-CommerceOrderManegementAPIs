using System.ComponentModel.DataAnnotations.Schema;

namespace E_CommerceOrderManagementAPI.Models.Entities
{
    public class Order
    {
        public int OrderId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        public int UserId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
        public int ProductId { get; set; }
        public string OrderItem { get; set; }
        public int Quantity { get; set; }
        public string OrderAmount { get; set; }
        public string Status { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    }
}
