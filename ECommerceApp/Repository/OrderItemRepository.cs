using ECommerceApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApp.Repository
{
    public class OrderItemRepository: IOrderItemRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderItemRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<OrderItem> GetByIdAsync(int id)
        {
            return await _context.OrderItems.FindAsync(id);
        }
        public async Task<IEnumerable<OrderItem>> GetAllAsync()// IEnumerable is better than using a list 
        {
            return await _context.OrderItems.ToListAsync();
        }

        public async Task AddAsync(OrderItem orderItem)
        {
            await _context.OrderItems.AddAsync(orderItem);
        }

        public async Task UpdateAsync(OrderItem orderItem)
        {
            _context.Update(orderItem);
        }

        public async Task DeleteAsync(int id)
        {
            var orderItem = await _context.OrderItems.FindAsync(id);
            if (orderItem != null)
            {
                _context.Remove(orderItem);
            }
        }
        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
