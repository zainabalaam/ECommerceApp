using AutoMapper;
using ECommerceApp.DTO;
using ECommerceApp.DTOs;
using ECommerceApp.Models;
using ECommerceApp.Repository;
using Microsoft.AspNetCore.Http.HttpResults;
using X.PagedList;

namespace ECommerceApp.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper mapper;

        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            this.mapper = mapper;
            Console.WriteLine("I'M Working!!!!😁");
        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            return await _productRepository.GetAllAsync();
        }

        public async Task<ProductDto> GetByIdAsync(int id)
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(id);
                return product ?? throw new Exception("Product not found.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving product: {ex.Message}");
            }
        }
        public async Task AddAsync(ProductCreateDTO productDto)
        {
            var product = mapper.Map<Product>(productDto);
            await _productRepository.AddAsync(product);
            await _productRepository.SaveAsync();
        }

        public async Task UpdateAsync(int id, ProductUpdateDTO product)
        {
            var existingProduct = await _productRepository.GetByIdAsync(id);
            if (existingProduct == null)
            {
                throw new Exception("Product not found.");
            }

            await _productRepository.UpdateAsync(id, product);
            await _productRepository.SaveAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var existingProduct = await _productRepository.GetByIdAsync(id);
            if (existingProduct == null)
            {
                throw new Exception("Product not found.");
            }

            await _productRepository.DeleteAsync(id);
            await _productRepository.SaveAsync();
        }
        public async Task<IEnumerable<ProductDto>> GetByCategoryIdAsync(int categoryId)
        {
            return await _productRepository.GetByCategoryIdAsync(categoryId);

        }
        public async Task<IPagedList<ProductDto>> GetPagedProductsAsync(int pageNumber, int pageSize, string? sortBy = null)
        {
            return await _productRepository.GetPagedProductsAsync(pageNumber, pageSize, sortBy);
        }
        public async Task<IPagedList<ProductDto>> GetPagedProductsByCategoryAsync(int categoryId, int pageNumber, int pageSize, string? sortBy = null)
        {
            return await _productRepository.GetPagedProductsByCategoryAsync(categoryId, pageNumber, pageSize, sortBy);
        }

        public async Task<IPagedList<ProductDto>> SearchProductsAsync(string? query, int pageNumber, int pageSize, string? sortBy = null)
        {
            return await _productRepository.SearchProductsAsync(query, pageNumber, pageSize, sortBy);
        }

        public async Task<IPagedList<ProductDto>> GetFilteredProductsAsync(string? query, int? categoryId, int pageNumber, int pageSize, string? sortBy = null)
        {
            return await _productRepository.GetFilteredProductsAsync(query, categoryId, pageNumber, pageSize, sortBy);
        }
    }


}
