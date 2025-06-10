using ECommerceApp.Models;

namespace ECommerceApp.Services
{
    public interface IOrderItemService
    {
        Task<IEnumerable<OrderItem>> GetAllAsync();
        Task<OrderItem> GetByIdAsync(int id);
        Task AddAsync(OrderItem orderitem);
        Task UpdateAsync(OrderItem orderitem);
        Task DeleteAsync(int id);
        Task Save();
    }
}
