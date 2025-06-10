using ECommerceApp.DTO;
using ECommerceApp.DTOs;
using ECommerceApp.Models;
using X.PagedList;

namespace ECommerceApp.Services
{
    public interface IProductService
    {
        Task<ProductDto> GetByIdAsync(int id);
        Task<IEnumerable<ProductDto>> GetAllAsync();
        Task<IPagedList<ProductDto>> GetPagedProductsAsync(int pageNumber, int pageSize, string? sortBy = null);
        Task<IPagedList<ProductDto>> GetPagedProductsByCategoryAsync(int categoryId, int pageNumber, int pageSize, string? sortBy = null);
        Task<IPagedList<ProductDto>> GetFilteredProductsAsync(string? query, int? categoryId, int pageNumber, int pageSize, string? sortBy = null);
        Task<IPagedList<ProductDto>> SearchProductsAsync(string? query, int pageNumber, int pageSize, string? sortBy = null);
        Task<IEnumerable<ProductDto>> GetByCategoryIdAsync(int categoryId);
        Task AddAsync(ProductCreateDTO product);
        Task UpdateAsync(int id, ProductUpdateDTO productDto);
        Task DeleteAsync(int id);

    }
}
