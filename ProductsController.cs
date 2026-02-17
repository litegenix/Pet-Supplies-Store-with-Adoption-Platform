using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using PetSuppliesStore.Data;
using PetSuppliesStore.Models;
using System.Security.Claims;

namespace PetSuppliesStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;
        
        public ProductsController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }
        
        // GET: api/Products
        // Get all products (with optional filters)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts(
            [FromQuery] int? categoryId,
            [FromQuery] string status = "Approved",
            [FromQuery] string searchTerm = "",
            [FromQuery] decimal? minPrice = null,
            [FromQuery] decimal? maxPrice = null,
            [FromQuery] string brand = "",
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 12)
        {
            var query = _context.Products
                .Include(p => p.Category)
                .Include(p => p.Seller)
                .Include(p => p.Images)
                .Include(p => p.Reviews)
                .Where(p => p.IsActive);
            
            // Apply filters
            if (categoryId.HasValue)
            {
                query = query.Where(p => p.CategoryID == categoryId.Value);
            }
            
            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(p => p.Status == status);
            }
            
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(p => p.ProductName.Contains(searchTerm) || 
                                        p.Description.Contains(searchTerm) ||
                                        p.Brand.Contains(searchTerm));
            }
            
            if (minPrice.HasValue)
            {
                query = query.Where(p => p.Price >= minPrice.Value);
            }
            
            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= maxPrice.Value);
            }
            
            if (!string.IsNullOrEmpty(brand))
            {
                query = query.Where(p => p.Brand == brand);
            }
            
            // Pagination
            var totalItems = await query.CountAsync();
            var products = await query
                .OrderByDescending(p => p.CreatedDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            
            return Ok(new
            {
                TotalItems = totalItems,
                Page = page,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize),
                Products = products
            });
        }
        
        // GET: api/Products/5
        // Get single product by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Seller)
                .Include(p => p.Images)
                .Include(p => p.Reviews)
                    .ThenInclude(r => r.Customer)
                .FirstOrDefaultAsync(p => p.ProductID == id);
            
            if (product == null)
            {
                return NotFound(new { message = "Product not found" });
            }
            
            // Increment view count
            product.ViewCount++;
            await _context.SaveChangesAsync();
            
            return Ok(product);
        }
        
        // POST: api/Products
        // Create new product (Sellers only)
        [HttpPost]
        [Authorize(Roles = "Seller")]
        public async Task<ActionResult<Product>> CreateProduct([FromForm] ProductCreateDto dto)
        {
            var sellerEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            var seller = await _context.Sellers
                .FirstOrDefaultAsync(s => s.User.Email == sellerEmail);
            
            if (seller == null)
            {
                return Unauthorized(new { message = "Seller not found" });
            }
            
            var product = new Product
            {
                SellerID = seller.SellerID,
                CategoryID = dto.CategoryID,
                ProductName = dto.ProductName,
                Description = dto.Description,
                Brand = dto.Brand,
                Price = dto.Price,
                StockQuantity = dto.StockQuantity,
                Status = "Pending", // Requires admin approval
                IsActive = true,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now
            };
            
            // Handle image upload
            if (dto.Image != null)
            {
                var imagePath = await SaveImage(dto.Image);
                product.ImageURL = imagePath;
            }
            
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            
            // Log activity
            await LogActivity("Product Created", $"Product '{product.ProductName}' created", seller.UserID);
            
            return CreatedAtAction(nameof(GetProduct), new { id = product.ProductID }, product);
        }
        
        // PUT: api/Products/5
        // Update product (Sellers can update their own products)
        [HttpPut("{id}")]
        [Authorize(Roles = "Seller")]
        public async Task<IActionResult> UpdateProduct(int id, [FromForm] ProductUpdateDto dto)
        {
            var product = await _context.Products.FindAsync(id);
            
            if (product == null)
            {
                return NotFound(new { message = "Product not found" });
            }
            
            var sellerEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            var seller = await _context.Sellers
                .FirstOrDefaultAsync(s => s.User.Email == sellerEmail);
            
            // Verify seller owns this product
            if (product.SellerID != seller.SellerID)
            {
                return Forbid();
            }
            
            // Update fields
            product.ProductName = dto.ProductName ?? product.ProductName;
            product.Description = dto.Description ?? product.Description;
            product.Brand = dto.Brand ?? product.Brand;
            product.Price = dto.Price ?? product.Price;
            product.StockQuantity = dto.StockQuantity ?? product.StockQuantity;
            product.UpdatedDate = DateTime.Now;
            
            // Handle image update
            if (dto.Image != null)
            {
                var imagePath = await SaveImage(dto.Image);
                product.ImageURL = imagePath;
            }
            
            await _context.SaveChangesAsync();
            
            await LogActivity("Product Updated", $"Product '{product.ProductName}' updated", seller.UserID);
            
            return Ok(new { message = "Product updated successfully", product });
        }
        
        // DELETE: api/Products/5
        // Delete product (soft delete)
        [HttpDelete("{id}")]
        [Authorize(Roles = "Seller,Admin")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            
            if (product == null)
            {
                return NotFound(new { message = "Product not found" });
            }
            
            // Soft delete
            product.IsActive = false;
            await _context.SaveChangesAsync();
            
            await LogActivity("Product Deleted", $"Product '{product.ProductName}' deleted", null);
            
            return Ok(new { message = "Product deleted successfully" });
        }
        
        // POST: api/Products/5/approve
        // Approve product (Admin only)
        [HttpPost("{id}/approve")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ApproveProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            
            if (product == null)
            {
                return NotFound(new { message = "Product not found" });
            }
            
            product.Status = "Approved";
            await _context.SaveChangesAsync();
            
            // Send notification to seller
            await CreateNotification(
                product.SellerID,
                "Product Approved",
                $"Your product '{product.ProductName}' has been approved and is now live on the store."
            );
            
            await LogActivity("Product Approved", $"Product '{product.ProductName}' approved", null);
            
            return Ok(new { message = "Product approved successfully" });
        }
        
        // POST: api/Products/5/reject
        // Reject product (Admin only)
        [HttpPost("{id}/reject")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RejectProduct(int id, [FromBody] string reason)
        {
            var product = await _context.Products.FindAsync(id);
            
            if (product == null)
            {
                return NotFound(new { message = "Product not found" });
            }
            
            product.Status = "Rejected";
            await _context.SaveChangesAsync();
            
            // Send notification to seller
            await CreateNotification(
                product.SellerID,
                "Product Rejected",
                $"Your product '{product.ProductName}' has been rejected. Reason: {reason}"
            );
            
            await LogActivity("Product Rejected", $"Product '{product.ProductName}' rejected", null);
            
            return Ok(new { message = "Product rejected successfully" });
        }
        
        // GET: api/Products/categories
        // Get all categories
        [HttpGet("categories")]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            var categories = await _context.Categories
                .Where(c => c.IsActive)
                .ToListAsync();
            
            return Ok(categories);
        }
        
        // Helper Methods
        private async Task<string> SaveImage(IFormFile image)
        {
            var uploadsFolder = Path.Combine(_environment.WebRootPath, "images", "products");
            Directory.CreateDirectory(uploadsFolder);
            
            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);
            
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }
            
            return $"/images/products/{uniqueFileName}";
        }
        
        private async Task CreateNotification(int sellerID, string title, string message)
        {
            var seller = await _context.Sellers.FindAsync(sellerID);
            if (seller != null)
            {
                var notification = new Notification
                {
                    UserID = seller.UserID,
                    NotificationType = "Product",
                    Title = title,
                    Message = message,
                    IsRead = false,
                    CreatedDate = DateTime.Now
                };
                
                _context.Notifications.Add(notification);
                await _context.SaveChangesAsync();
            }
        }
        
        private async Task LogActivity(string actionType, string description, int? userId)
        {
            var log = new ActivityLog
            {
                UserID = userId,
                ActionType = actionType,
                ActionDescription = description,
                IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                Timestamp = DateTime.Now
            };
            
            _context.ActivityLogs.Add(log);
            await _context.SaveChangesAsync();
        }
    }
    
    // DTOs for Product operations
    public class ProductCreateDto
    {
        public int CategoryID { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public string Brand { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public IFormFile Image { get; set; }
    }
    
    public class ProductUpdateDto
    {
        public string ProductName { get; set; }
        public string Description { get; set; }
        public string Brand { get; set; }
        public decimal? Price { get; set; }
        public int? StockQuantity { get; set; }
        public IFormFile Image { get; set; }
    }
}
