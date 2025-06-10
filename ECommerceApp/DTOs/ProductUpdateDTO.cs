using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace ECommerceApp.DTOs
{
    //Update a product (PUT/PATCH)
    public class ProductUpdateDTO
    {
        public int ProductId { get; set; } // Needed to identify the product to update

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Stock { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public IFormFile? Image { get; set; } // Optional for updating the image

        [Required]
        [BindNever] // Exclude from model binding to prevent validation failure
        public string ImageUrl { get; set; } // Existing or new image URL
    }

}

