using ECommerceApp.Models;

namespace ECommerceApp.Repository
{
    public interface ICartItemRepository
    {
        Task<CartItem> GetByIdAsync(int id);
        Task<IEnumerable<CartItem>> GetAllAsync();
        Task AddAsync(CartItem cartItem);
        Task UpdateAsync(CartItem cartItem);
        Task DeleteAsync(int id);
        Task Save();
    }
}
