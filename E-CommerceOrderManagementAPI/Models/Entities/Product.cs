using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_CommerceOrderManagementAPI.Models.Entities
{
    public class Product
    {
        public int ProductId { get; set; }
        [ForeignKey("CategoryId")]
        public int CategoryId { get; set; }
        public Category Cateory { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public int Stock { get; set; }
        public int Price { get; set; }

    }
}
