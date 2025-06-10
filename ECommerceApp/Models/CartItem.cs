using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceApp.Models
{
    public class CartItem
    {
        public int Id { get; set; }

        [ForeignKey("ShoppingCart")]
        public int ShoppingCartId { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public ShoppingCart ShoppingCart { get; set; }
        public Product Product { get; set; }
    }
}