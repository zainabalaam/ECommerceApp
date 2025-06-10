using ECommerceApp.Models;
using System.ComponentModel.DataAnnotations;

namespace ECommerceApp.DTOs
{
    //Read data (GET)
    public class ProductDto
    {
        public int ProductId { get; set; }  // Unique ID
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        [Display(Name = "Image")]
        public string ImageUrl { get; set; }
        public int Stock { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } // Extra field to display category name
    }
}
