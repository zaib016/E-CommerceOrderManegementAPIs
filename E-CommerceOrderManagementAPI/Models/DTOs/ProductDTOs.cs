using E_CommerceOrderManagementAPI.Models.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_CommerceOrderManagementAPI.Models.DTOs
{
    public class ProductDTOs
    {
        [ForeignKey("CategoryId")]
        public int CategoryId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public int Stock { get; set; }
        public int Price { get; set; }
    }
}
