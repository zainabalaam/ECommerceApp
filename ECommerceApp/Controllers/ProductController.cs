using ECommerceApp.DTO;
using ECommerceApp.DTOs;
using ECommerceApp.Models;
using ECommerceApp.Repository;
using ECommerceApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using X.PagedList;

namespace ECommerceApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService productService;
        private readonly ICategoryService categoryService;

        public ProductController(IProductService productService, ICategoryService categoryService)
        {
            this.productService = productService;
            this.categoryService = categoryService;
        }
        [HttpGet]
        public async Task<IActionResult> Index(string? query, int? categoryId, int? page, string? sortBy)
        {
            int pageNumber = page ?? 1;
            int pageSize = 10; // Adjust as needed
            var products = await productService.GetFilteredProductsAsync(query, categoryId, pageNumber, pageSize, sortBy);
            if (!products.Any())
            {
                ViewBag.NoResults = string.IsNullOrEmpty(query) && !categoryId.HasValue ? "No products found." :
                                    $"No products found matching your filters (Query: '{query}', Category: {categoryId}).";
            }
            ViewBag.Query = query;
            ViewBag.CategoryId = categoryId;
            ViewBag.SortBy = sortBy;
            var categories = await categoryService.GetAllAsync(); // Fetch categories as IEnumerable<Category>
            ViewBag.CategoryList = new SelectList(categories, "Id", "Name", categoryId); // Create SelectList with Id as Value, Name as Text
            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var product = await productService.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        public async Task<IActionResult> ByCategory(int categoryId, int? page, string? sortBy)
        {
            int pageNumber = page ?? 1;
            int pageSize = 10;
            var products = await productService.GetPagedProductsByCategoryAsync(categoryId, pageNumber, pageSize, sortBy);
            if (!products.Any())
            {
                return NotFound("No products found for this category.");
            }
            ViewBag.CategoryId = categoryId;
            ViewBag.SortBy = sortBy;
            return View(products);
        }

        [HttpGet]//("Search")]
        public async Task<IActionResult> Search(string? query, int? page, string? sortBy)
        {
            int pageNumber = page ?? 1;
            int pageSize = 10; // Adjust as needed
            var products = await productService.SearchProductsAsync(query, pageNumber, pageSize, sortBy);
            if (!products.Any())
            {
                ViewBag.NoResults = string.IsNullOrEmpty(query) ? "No products found." : $"No products found matching '{query}'.";
            }
            ViewBag.Query = query; // Pass query to maintain it in pagination and sorting
            ViewBag.SortBy = sortBy; // Pass sortBy to maintain sorting
            return View(products);
        }

        // GET: /Product/Add
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add()
        {
            ViewBag.Categories = await categoryService.GetAllAsync(); // For the category dropdown
            return View();
        }

        // POST: /Product/Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add(ProductCreateDTO productDto)
        {
            if (ModelState.IsValid)
            {
                if (productDto.Image != null)
                {
                    string imagePath = await SaveImageAsync(productDto.Image);
                    productDto.ImageUrl = imagePath; // Set the image path
                }
                await productService.AddAsync(productDto);
                return RedirectToAction("Index");
            }
            ViewBag.Categories = await categoryService.GetAllAsync(); // Repopulate categories if form is invalid
            return View(productDto);
        }

        // GET: /Product/Update?id=<productId>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id)
        {
            var product = await productService.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound($"Product with ID {id} not found.");
            }

            var productDto = new ProductUpdateDTO
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                CategoryId = product.CategoryId,
                ImageUrl = product.ImageUrl // Pre-populate with existing image URL
            };

            ViewBag.Categories = await categoryService.GetAllAsync(); // For the category dropdown
            return View(productDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, ProductUpdateDTO productDto)
        {
            // Fetch the existing product to get the current ImageUrl if no new image is uploaded
            var existingProduct = await productService.GetByIdAsync(id);
            if (existingProduct == null)
            {
                return NotFound($"Product with ID {id} not found.");
            }

            // Set ImageUrl based on new image (if uploaded) or existing value
            if (productDto.Image != null)
            {
                string imagePath = await SaveImageAsync(productDto.Image);
                productDto.ImageUrl = imagePath; // Update with new image path
            }
            else
            {
                productDto.ImageUrl = existingProduct.ImageUrl ?? "/images/default-product.png"; // Use default if null
            }

            ModelState.Remove("ImageUrl");
            if (ModelState.IsValid) // Now ImageUrl is set, so ModelState should be valid for other fields
            {
                await productService.UpdateAsync(id, productDto);
                return RedirectToAction("Index");
            }
            ViewBag.Categories = await categoryService.GetAllAsync(); // Repopulate categories if form is invalid
            return View(productDto);
        }


        private async Task<string> SaveImageAsync(IFormFile image)
        {
            try
            {
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/products");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                string filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                return $"images/products/{fileName}";
            }
            catch (IOException ex)
            {
                ModelState.AddModelError("", $"Error saving image: {ex.Message}");
                throw; // Or log and return a default/error path
            }
        }



        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await productService.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound($"Product with ID {id} not found.");
            }

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int ProductId)
        {
            await productService.DeleteAsync(ProductId);
            return RedirectToAction("Index");
        }


    }

}
