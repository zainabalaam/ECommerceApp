using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceApp.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; } // Reference to the user placing the order
        public DateTime OrderDate { get; set; } // Date when the order is placed
        public decimal TotalAmount { get; set; } // Total price of the order

        public ApplicationUser User { get; set; }

    }
}
