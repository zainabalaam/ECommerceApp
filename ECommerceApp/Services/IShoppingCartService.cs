using ECommerceApp.DTOs;

namespace ECommerceApp.Services
{
    public interface IShoppingCartService
    {
        Task<IEnumerable<CartItemDto>> GetAllAsync();
        Task AddAsync(CartItemDto cartItem);
        Task DeleteAsync(int productId);
        Task Save();
    }
}