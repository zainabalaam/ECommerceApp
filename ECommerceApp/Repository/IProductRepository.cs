using ECommerceApp.Models;
using ECommerceApp.DTOs;
using X.PagedList;
using ECommerceApp.DTO;

namespace ECommerceApp.Repository
{
    public interface IProductRepository
    {
        Task<ProductDto> GetByIdAsync(int id);
        Task<IEnumerable<ProductDto>> GetAllAsync();
        Task<IPagedList<ProductDto>> GetPagedProductsAsync(int pageNumber, int pageSize, string? sortBy = null);
        Task<IEnumerable<ProductDto>> GetByCategoryIdAsync(int categoryId);
        Task<IPagedList<ProductDto>> GetPagedProductsByCategoryAsync(int categoryId, int pageNumber, int pageSize, string? sortBy = null);
        Task<IPagedList<ProductDto>> GetFilteredProductsAsync(string? query, int? categoryId, int pageNumber, int pageSize, string? sortBy = null);
        Task<IPagedList<ProductDto>> SearchProductsAsync(string? query, int pageNumber, int pageSize, string? sortBy = null);
        Task AddAsync(Product product);
        Task UpdateAsync(int id, ProductUpdateDTO productDto);
        Task DeleteAsync(int id);
        Task SaveAsync();
    }
}
