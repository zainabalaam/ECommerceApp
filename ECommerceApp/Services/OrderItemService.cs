using ECommerceApp.Models;
using ECommerceApp.Repository;

namespace ECommerceApp.Services
{
    public class OrderItemService:IOrderItemService
    {
        private readonly IOrderItemRepository _OrderItemRepository;

        public OrderItemService(IOrderItemRepository OrderItemRepository)
        {
            _OrderItemRepository = OrderItemRepository;
        }

        public async Task<IEnumerable<OrderItem>> GetAllAsync()
        {
            return await _OrderItemRepository.GetAllAsync();
        }

        public async Task<OrderItem> GetByIdAsync(int id)
        {
            return await _OrderItemRepository.GetByIdAsync(id);
        }

        public async Task AddAsync(OrderItem OrderItem)
        {
            await _OrderItemRepository.AddAsync(OrderItem);
        }

        public async Task UpdateAsync(OrderItem OrderItem)
        {
            await _OrderItemRepository.UpdateAsync(OrderItem);
        }

        public async Task DeleteAsync(int id)
        {
            await _OrderItemRepository.DeleteAsync(id);
        }

        public async Task Save()
        {
            await _OrderItemRepository.Save();
        }
    }
}
