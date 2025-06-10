using ECommerceApp.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApp.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext() : base()
        {
        }

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Call base configuration first to set up Identity tables
            base.OnModelCreating(modelBuilder);

            // Your existing entity configurations
            modelBuilder.Entity<CartItem>()
                .HasOne(e => e.Product)
                .WithMany(c => c.CartItems)
                .HasForeignKey(p => p.ProductId);

            modelBuilder.Entity<CartItem>()
                .HasOne(e => e.ShoppingCart)
                .WithMany(c => c.CartItems)
                .HasForeignKey(f => f.ShoppingCartId);

            modelBuilder.Entity<Order>()
                    .Property(p => p.TotalAmount)
                     .HasColumnType("decimal(18,2)"); // Setting precision to 18 and scale to 2
             modelBuilder.Entity<Product>()
                    .Property(p => p.Price)
                     .HasColumnType("decimal(18,2)"); // Setting precision to 18 and scale to 2



        }

        // Your DbSet properties
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
    }
}