using ECommerceApp.Models;

namespace ECommerceApp.Repository
{
    public interface IOrderItemRepository
    {
        Task<OrderItem> GetByIdAsync(int id);
        Task<IEnumerable<OrderItem>> GetAllAsync();
        Task AddAsync(OrderItem orderItem);
        Task UpdateAsync(OrderItem orderItem);
        Task DeleteAsync(int id);
        Task Save();
    }
}
