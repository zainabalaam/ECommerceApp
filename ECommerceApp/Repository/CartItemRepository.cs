using ECommerceApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApp.Repository
{
    public class CartItemRepository: ICartItemRepository
    {
        private readonly ApplicationDbContext _context;

        public CartItemRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<CartItem> GetByIdAsync(int id)
        {
            return await _context.CartItems.FindAsync(id);
        }
        public async Task<IEnumerable<CartItem>> GetAllAsync()// IEnumerable is better than using a list 
        {
            return await _context.CartItems.ToListAsync();
        }

        public async Task AddAsync(CartItem cartItem)
        {
            await _context.CartItems.AddAsync(cartItem);
        }

        public async Task UpdateAsync(CartItem cartItem)
        {
            _context.Update(cartItem);
        }

        public async Task DeleteAsync(int id)
        {
            var cartItem = await _context.CartItems.FindAsync(id);
            if (cartItem != null)
            {
                _context.Remove(cartItem);
            }
        }
        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
