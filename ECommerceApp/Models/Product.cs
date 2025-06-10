using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceApp.Models
{
    public class Product
    {
        public int ProductId { get; set; } 
        public string Name { get; set; } 
        public string Description { get; set; }
        public decimal Price { get; set; } 
        public string ImageUrl { get; set; }
        public int Stock { get; set; } 
        [ForeignKey("Category")]
        public int CategoryId { get; set; } 
        public Category Category { get; set; }

        public List<OrderItem> orderItems { get; set; }
        public List<CartItem> CartItems { get; set; }
    }
}
