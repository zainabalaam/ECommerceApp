using ECommerceApp.DTOs;
using ECommerceApp.Models;
using ECommerceApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApp.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IProductService productService;
        private readonly IShoppingCartService shoppingCartService;

        public ShoppingCartController(IProductService productService, IShoppingCartService shoppingCartService)
        {
            this.productService = productService;
            this.shoppingCartService = shoppingCartService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var cartItems = await shoppingCartService.GetAllAsync();
            return View(cartItems);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int id)
        {
            try
            {
                var product = await productService.GetByIdAsync(id);
                if (product == null)
                {
                    return NotFound($"Product with ID {id} not found.");
                }

                var cartItem = new CartItemDto
                {
                    ProductId = product.ProductId,
                    Name = product.Name,
                    Price = product.Price,
                    ImageUrl = product.ImageUrl,
                    Quantity = 1
                };
                await shoppingCartService.AddAsync(cartItem);
                return RedirectToAction("Index", "ShoppingCart");
            }
            catch (InvalidOperationException ex) // E.g., user not logged in
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Index", "Product");
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx && sqlEx.Number == 547) // Foreign key violation
            {
                TempData["ErrorMessage"] = "Failed to add item to cart: Invalid shopping cart.";
                return RedirectToAction("Index", "Product");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
                return RedirectToAction("Index", "Product");
            }
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int id)
        {
            await shoppingCartService.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}


