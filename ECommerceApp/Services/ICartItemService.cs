using ECommerceApp.Models;

namespace ECommerceApp.Services
{
    public interface ICartItemService
    {
        Task<IEnumerable<CartItem>> GetAllAsync();
        Task<CartItem> GetByIdAsync(int id);
        Task AddAsync(CartItem cartItem);
        Task UpdateAsync(CartItem cartItem);
        Task DeleteAsync(int id);
        Task Save();
    }
}
