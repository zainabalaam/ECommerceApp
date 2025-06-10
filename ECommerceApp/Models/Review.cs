using ECommerceApp.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceApp.Models
{
    public class Review
    {
        public int Id { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime DateCreated { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }

        public ApplicationUser User { get; set; }
        public Product Product { get; set; }

    }
}

