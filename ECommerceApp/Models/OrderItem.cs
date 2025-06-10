using ECommerceApp.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceApp.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [ForeignKey("Order")]
        public int OrderId { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }

        public Order Order { get; set; }
        public Product Product { get; set; }



    }
}



