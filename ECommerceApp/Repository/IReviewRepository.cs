using ECommerceApp.Models;

namespace ECommerceApp.Repository
{
    public interface IReviewRepository
    {
        Task<Review> GetByIdAsync(int id);
        Task<IEnumerable<Review>> GetAllAsync();
        Task AddAsync(Review review);
        Task UpdateAsync(Review review);
        Task DeleteAsync(int id);
        Task Save();
    }
}
