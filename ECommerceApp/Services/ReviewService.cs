using ECommerceApp.Models;
using ECommerceApp.Repository;

namespace ECommerceApp.Services
{
    public class ReviewService:IReviewService
    {
        private readonly IReviewRepository _ReviewRepository;

        public ReviewService(IReviewRepository ReviewRepository)
        {
            _ReviewRepository = ReviewRepository;
        }

        public async Task<IEnumerable<Review>> GetAllAsync()
        {
            return await _ReviewRepository.GetAllAsync();
        }

        public async Task<Review> GetByIdAsync(int id)
        {
            return await _ReviewRepository.GetByIdAsync(id);
        }

        public async Task AddAsync(Review Review)
        {
            await _ReviewRepository.AddAsync(Review);
        }

        public async Task UpdateAsync(Review Review)
        {
            await _ReviewRepository.UpdateAsync(Review);
        }

        public async Task DeleteAsync(int id)
        {
            await _ReviewRepository.DeleteAsync(id);
        }

        public async Task Save()
        {
            await _ReviewRepository.Save();
        }
    }
}
