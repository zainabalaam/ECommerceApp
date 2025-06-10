using ECommerceApp.Models;
using ECommerceApp.Repository;

namespace ECommerceApp.Services
{
    public class CartItemService: ICartItemService
    {
        private readonly ICartItemRepository _CartItemRepository;

        public CartItemService(ICartItemRepository CartItemRepository)
        {
            _CartItemRepository = CartItemRepository;
        }

        public async Task<IEnumerable<CartItem>> GetAllAsync()
        {
            return await _CartItemRepository.GetAllAsync();
        }

        public async Task<CartItem> GetByIdAsync(int id)
        {
            return await _CartItemRepository.GetByIdAsync(id);
        }

        public async Task AddAsync(CartItem CartItem)
        {
            await _CartItemRepository.AddAsync(CartItem);
        }

        public async Task UpdateAsync(CartItem CartItem)
        {
            await _CartItemRepository.UpdateAsync(CartItem);
        }

        public async Task DeleteAsync(int id)
        {
            await _CartItemRepository.DeleteAsync(id);
        }

        public async Task Save()
        {
            await _CartItemRepository.Save();
        }
    }
}
