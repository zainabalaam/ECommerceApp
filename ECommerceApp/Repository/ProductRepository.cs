using AutoMapper;
using ECommerceApp.Models;
using ECommerceApp.DTOs;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using ECommerceApp.DTO;
using X.PagedList.Extensions;
using X.PagedList;

namespace ECommerceApp.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper; // Inject AutoMapper

        public ProductRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ProductDto> GetByIdAsync(int id)
        {
            var product = await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.ProductId == id);
            return product == null ? null : _mapper.Map<ProductDto>(product);
        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            var products = await _context.Products.Include(p => p.Category).ToListAsync();
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task AddAsync(Product product)
        {
            await _context.Products.AddAsync(product);
        }

        public async Task UpdateAsync(int id, ProductUpdateDTO productDto)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return;

            _mapper.Map(productDto, product); // Update existing product
            _context.Products.Update(product);
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);

                // Delete the image file if it exists
                if (!string.IsNullOrEmpty(product.ImageUrl))
                {
                    string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", product.ImageUrl.TrimStart('/'));
                    if (File.Exists(imagePath))
                    {
                        File.Delete(imagePath);
                    }
                }
            }
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ProductDto>> GetByCategoryIdAsync(int categoryId)
        {
            var products = await _context.Products.Where(p => p.CategoryId == categoryId).ToListAsync();
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task<IPagedList<ProductDto>> GetPagedProductsAsync(int pageNumber, int pageSize, string? sortBy = null)
        {
            IQueryable<Product> query = _context.Products.Include(p => p.Category);

            // Apply sorting (default to Name, or handle sortBy)
            query = sortBy?.ToLower() switch
            {
                "price" => query.OrderBy(p => p.Price),
                "price_desc" => query.OrderByDescending(p => p.Price),
                _ => query.OrderBy(p => p.Name)
            };

            // Materialize the query
            var products = await query.ToListAsync();

            // Paginate with ToPagedList (synchronous, since ToPagedListAsync isn’t available for List<T>)
            var pagedProducts = products.ToPagedList(pageNumber, pageSize);

            // Map to ProductDto and create a new StaticPagedList
            var productDtos = pagedProducts.Select(p => _mapper.Map<ProductDto>(p)).ToList();
            return new StaticPagedList<ProductDto>(productDtos, pagedProducts.PageNumber, pagedProducts.PageSize, pagedProducts.TotalItemCount);
        }

        public async Task<IPagedList<ProductDto>> GetPagedProductsByCategoryAsync(int categoryId, int pageNumber, int pageSize, string? sortBy = null)
        {
            IQueryable<Product> query = _context.Products.Include(p => p.Category).Where(p => p.CategoryId == categoryId);

            query = sortBy?.ToLower() switch
            {
                "price" => query.OrderBy(p => p.Price),
                "price_desc" => query.OrderByDescending(p => p.Price),
                _ => query.OrderBy(p => p.Name)
            };

            var products = await query.ToListAsync();
            var pagedProducts = products.ToPagedList(pageNumber, pageSize);
            var productDtos = pagedProducts.Select(p => _mapper.Map<ProductDto>(p)).ToList();
            return new StaticPagedList<ProductDto>(productDtos, pagedProducts.PageNumber, pagedProducts.PageSize, pagedProducts.TotalItemCount);
        }

        public async Task<IPagedList<ProductDto>> SearchProductsAsync(string? query, int pageNumber, int pageSize, string? sortBy = null)
        {
            IQueryable<Product> queryable = _context.Products.Include(p => p.Category);

            if (!string.IsNullOrEmpty(query))
            {
                queryable = queryable.Where(p => p.Name.Contains(query) || p.Description.Contains(query));
            }

            queryable = sortBy?.ToLower() switch
            {
                "price" => queryable.OrderBy(p => p.Price),
                "price_desc" => queryable.OrderByDescending(p => p.Price),
                _ => queryable.OrderBy(p => p.Name)
            };

            var products = await queryable.ToListAsync();
            var pagedProducts = products.ToPagedList(pageNumber, pageSize);
            var productDtos = pagedProducts.Select(p => _mapper.Map<ProductDto>(p)).ToList();
            return new StaticPagedList<ProductDto>(productDtos, pagedProducts.PageNumber, pagedProducts.PageSize, pagedProducts.TotalItemCount);
        }
        public async Task<IPagedList<ProductDto>> GetFilteredProductsAsync(string? query, int? categoryId, int pageNumber, int pageSize, string? sortBy = null)
        {
            IQueryable<Product> queryable = _context.Products.Include(p => p.Category);

            if (!string.IsNullOrEmpty(query))
            {
                queryable = queryable.Where(p => p.Name.Contains(query) || p.Description.Contains(query));
            }

            if (categoryId.HasValue)
            {
                queryable = queryable.Where(p => p.CategoryId == categoryId.Value);
            }

            queryable = sortBy?.ToLower() switch
            {
                "price" => queryable.OrderBy(p => p.Price),
                "price_desc" => queryable.OrderByDescending(p => p.Price),
                _ => queryable.OrderBy(p => p.Name)
            };

            var products = await queryable.ToListAsync();
            var pagedProducts = products.ToPagedList(pageNumber, pageSize);
            var productDtos = pagedProducts.Select(p => _mapper.Map<ProductDto>(p)).ToList();
            return new StaticPagedList<ProductDto>(productDtos, pagedProducts.PageNumber, pagedProducts.PageSize, pagedProducts.TotalItemCount);
        }

    }
}