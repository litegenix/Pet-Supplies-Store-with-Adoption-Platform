using Microsoft.EntityFrameworkCore;
using PetSuppliesStore.Models;

namespace PetSuppliesStore.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        
        // DbSets for all entities
        public DbSet<User> Users { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Seller> Sellers { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Shelter> Shelters { get; set; }
        public DbSet<AdoptionPet> AdoptionPets { get; set; }
        public DbSet<AdoptionApplication> AdoptionApplications { get; set; }
        public DbSet<ProductReview> ProductReviews { get; set; }
        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<ActivityLog> ActivityLogs { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Configure User entity
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
            
            modelBuilder.Entity<User>()
                .Property(u => u.UserRole)
                .HasMaxLength(20)
                .IsRequired();
            
            // Configure Product entity
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(10,2)");
            
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Seller)
                .WithMany()
                .HasForeignKey(p => p.SellerID)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany()
                .HasForeignKey(p => p.CategoryID)
                .OnDelete(DeleteBehavior.Restrict);
            
            // Configure OrderItem computed column
            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.UnitPrice)
                .HasColumnType("decimal(10,2)");
            
            // Configure Shopping Cart unique constraint
            modelBuilder.Entity<ShoppingCart>()
                .HasIndex(sc => new { sc.CustomerID, sc.ProductID })
                .IsUnique();
            
            // Configure Wishlist unique constraint
            modelBuilder.Entity<Wishlist>()
                .HasIndex(w => new { w.CustomerID, w.ProductID })
                .IsUnique();
            
            // Configure decimal columns
            modelBuilder.Entity<Order>()
                .Property(o => o.TotalAmount)
                .HasColumnType("decimal(10,2)");
            
            modelBuilder.Entity<AdoptionPet>()
                .Property(p => p.AdoptionFee)
                .HasColumnType("decimal(10,2)");
            
            modelBuilder.Entity<Seller>()
                .Property(s => s.Rating)
                .HasColumnType("decimal(3,2)");
            
            // Seed initial data
            SeedData(modelBuilder);
        }
        
        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Admin User
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserID = 1,
                    Email = "admin@petstore.com",
                    PasswordHash = "AQAAAAEAACcQAAAAEJ7JKqJ8bKxDcKF7p6zXm8GqxH5lGQj4QZ0KqZGqxH5lGQj4QZ0K=", // Hash for "Admin@123"
                    UserRole = "Admin",
                    IsActive = true,
                    CreatedDate = DateTime.Now
                }
            );
            
            modelBuilder.Entity<Admin>().HasData(
                new Admin
                {
                    AdminID = 1,
                    UserID = 1,
                    FullName = "System Administrator",
                    Phone = "555-0100"
                }
            );
            
            // Seed Categories
            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryID = 1, CategoryName = "Dog Food", Description = "Premium dog food and treats", IsActive = true },
                new Category { CategoryID = 2, CategoryName = "Cat Food", Description = "Nutritious cat food and snacks", IsActive = true },
                new Category { CategoryID = 3, CategoryName = "Dog Toys", Description = "Fun and interactive toys for dogs", IsActive = true },
                new Category { CategoryID = 4, CategoryName = "Cat Toys", Description = "Engaging toys for cats", IsActive = true },
                new Category { CategoryID = 5, CategoryName = "Pet Accessories", Description = "Collars, leashes, bowls, and more", IsActive = true },
                new Category { CategoryID = 6, CategoryName = "Pet Health", Description = "Vitamins, supplements, and health products", IsActive = true },
                new Category { CategoryID = 7, CategoryName = "Bird Supplies", Description = "Food, cages, and accessories for birds", IsActive = true },
                new Category { CategoryID = 8, CategoryName = "Small Pet Supplies", Description = "Products for rabbits, hamsters, and guinea pigs", IsActive = true }
            );
        }
    }
}
