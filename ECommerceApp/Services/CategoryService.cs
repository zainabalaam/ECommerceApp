using ECommerceApp.Models;
using ECommerceApp.Repository;

namespace ECommerceApp.Services
{
    public class CategoryService: ICategoryService
    {
        private readonly ICategoryRepository _CategoryRepository;

        public CategoryService(ICategoryRepository CategoryRepository)
        {
            _CategoryRepository = CategoryRepository;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _CategoryRepository.GetAllAsync();
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            return await _CategoryRepository.GetByIdAsync(id);
        }

        public async Task AddAsync(Category Category)
        {
            await _CategoryRepository.AddAsync(Category);
        }

        public async Task UpdateAsync(Category Category)
        {
            await _CategoryRepository.UpdateAsync(Category);
        }

        public async Task DeleteAsync(int id)
        {
            await _CategoryRepository.DeleteAsync(id);
        }

        public async Task Save()
        {
            await _CategoryRepository.Save();
        }
    }
}
