using ECommerceApp.Models;
using ECommerceApp.Repository;

namespace ECommerceApp.Services
{
    public class OrderService:IOrderService
    {
        private readonly IOrderRepository _OrderRepository;

        public OrderService(IOrderRepository OrderRepository)
        {
            _OrderRepository = OrderRepository;
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _OrderRepository.GetAllAsync();
        }

        public async Task<Order> GetByIdAsync(int id)
        {
            return await _OrderRepository.GetByIdAsync(id);
        }

        public async Task AddAsync(Order Order)
        {
            await _OrderRepository.AddAsync(Order);
        }

        public async Task UpdateAsync(Order Order)
        {
            await _OrderRepository.UpdateAsync(Order);
        }

        public async Task DeleteAsync(int id)
        {
            await _OrderRepository.DeleteAsync(id);
        }

        public async Task Save()
        {
            await _OrderRepository.Save();
        }
    }

}
