using System.ComponentModel.DataAnnotations;

namespace ECommerceApp.DTOs
{
    /// <summary>
    /// Represents a data transfer object for a cart item.
    /// </summary>
    public class CartItemDto
    {
        public int ProductId { get; set; }

        [Required]
        public string Name { get; set; }


        [Required]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }


        [Required]
        public string ImageUrl { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }
}