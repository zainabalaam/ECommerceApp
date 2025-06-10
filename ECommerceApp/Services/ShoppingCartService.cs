using ECommerceApp.DTOs;
using ECommerceApp.Models;
using ECommerceApp.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApp.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext _context;

        public ShoppingCartService(
            IShoppingCartRepository shoppingCartRepository,
            IProductService productService,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            ApplicationDbContext context)
        {
            _shoppingCartRepository = shoppingCartRepository;
            _productService = productService;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        public async Task AddAsync(CartItemDto cartItem)
        {
            if (cartItem.Price < 0 || cartItem.Quantity < 1)
            {
                throw new ArgumentException("Cart item price and quantity must be positive.");
            }

            var product = await _productService.GetByIdAsync(cartItem.ProductId);
            if (product == null)
            {
                throw new KeyNotFoundException($"Product with ID {cartItem.ProductId} not found.");
            }

            // Get or create the user's shopping cart
            var userId = _httpContextAccessor.HttpContext.User.Identity.Name; // Requires logged-in user
            if (string.IsNullOrEmpty(userId))
            {
                throw new InvalidOperationException("User must be logged in to add items to the cart.");
            }

            var shoppingCart = await _context.ShoppingCarts
                .Include(sc => sc.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(sc => sc.UserId == userId);
            if (shoppingCart == null)
            {
                shoppingCart = new ShoppingCart { UserId = userId };
                _context.ShoppingCarts.Add(shoppingCart);
                await _context.SaveChangesAsync();
            }

            var existingCartItem = shoppingCart.CartItems.FirstOrDefault(ci => ci.ProductId == cartItem.ProductId);
            if (existingCartItem != null)
            {
                existingCartItem.Quantity += cartItem.Quantity;
                await _shoppingCartRepository.UpdateAsync(existingCartItem);
            }
            else
            {
                var newCartItem = _mapper.Map<CartItem>(cartItem);
                newCartItem.ShoppingCartId = shoppingCart.Id;
                newCartItem.Quantity = cartItem.Quantity;
                await _shoppingCartRepository.AddAsync(newCartItem);
            }
            await Save();
        }

        public async Task<IEnumerable<CartItemDto>> GetAllAsync()
        {
            var cartItems = await _shoppingCartRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<CartItemDto>>(cartItems);
        }

        public async Task DeleteAsync(int productId)
        {
            var cartItem = (await _shoppingCartRepository.GetAllAsync())
                .FirstOrDefault(ci => ci.ProductId == productId);
            if (cartItem != null)
            {
                await _shoppingCartRepository.DeleteAsync(cartItem.Id);
                await Save();
            }
        }

        public async Task Save()
        {
            await _shoppingCartRepository.Save();
        }
    }
}