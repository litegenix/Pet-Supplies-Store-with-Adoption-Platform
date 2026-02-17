using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetSuppliesStore.Models
{
    // =============================================
    // USER MODELS
    // =============================================
    
    public class User
    {
        [Key]
        public int UserID { get; set; }
        
        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }
        
        [Required]
        [StringLength(255)]
        public string PasswordHash { get; set; }
        
        [Required]
        [StringLength(20)]
        public string UserRole { get; set; } // Admin, Customer, Seller
        
        public bool IsActive { get; set; } = true;
        
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        
        public DateTime? LastLoginDate { get; set; }
    }
    
    public class Customer
    {
        [Key]
        public int CustomerID { get; set; }
        
        public int UserID { get; set; }
        
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }
        
        [Required]
        [StringLength(50)]
        public string LastName { get; set; }
        
        [StringLength(20)]
        public string Phone { get; set; }
        
        [StringLength(255)]
        public string Address { get; set; }
        
        [StringLength(50)]
        public string City { get; set; }
        
        [StringLength(50)]
        public string State { get; set; }
        
        [StringLength(10)]
        public string ZipCode { get; set; }
        
        [StringLength(50)]
        public string Country { get; set; } = "USA";
        
        [ForeignKey("UserID")]
        public virtual User User { get; set; }
        
        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";
    }
    
    public class Seller
    {
        [Key]
        public int SellerID { get; set; }
        
        public int UserID { get; set; }
        
        [Required]
        [StringLength(100)]
        public string BusinessName { get; set; }
        
        [Required]
        [StringLength(100)]
        public string ContactPerson { get; set; }
        
        [Required]
        [StringLength(20)]
        public string Phone { get; set; }
        
        [StringLength(255)]
        public string Address { get; set; }
        
        [StringLength(50)]
        public string City { get; set; }
        
        [StringLength(50)]
        public string State { get; set; }
        
        [StringLength(10)]
        public string ZipCode { get; set; }
        
        [StringLength(50)]
        public string Country { get; set; } = "USA";
        
        [StringLength(50)]
        public string TaxID { get; set; }
        
        public bool IsVerified { get; set; } = false;
        
        public decimal Rating { get; set; } = 0.00m;
        
        public int TotalSales { get; set; } = 0;
        
        [ForeignKey("UserID")]
        public virtual User User { get; set; }
    }
    
    public class Admin
    {
        [Key]
        public int AdminID { get; set; }
        
        public int UserID { get; set; }
        
        [Required]
        [StringLength(100)]
        public string FullName { get; set; }
        
        [StringLength(20)]
        public string Phone { get; set; }
        
        [ForeignKey("UserID")]
        public virtual User User { get; set; }
    }
    
    // =============================================
    // PRODUCT MODELS
    // =============================================
    
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }
        
        [Required]
        [StringLength(50)]
        public string CategoryName { get; set; }
        
        [StringLength(255)]
        public string Description { get; set; }
        
        [StringLength(255)]
        public string ImageURL { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
    
    public class Product
    {
        [Key]
        public int ProductID { get; set; }
        
        public int SellerID { get; set; }
        
        public int CategoryID { get; set; }
        
        [Required]
        [StringLength(100)]
        public string ProductName { get; set; }
        
        public string Description { get; set; }
        
        [StringLength(50)]
        public string Brand { get; set; }
        
        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }
        
        [Required]
        [Range(0, int.MaxValue)]
        public int StockQuantity { get; set; }
        
        [StringLength(255)]
        public string ImageURL { get; set; }
        
        [StringLength(20)]
        public string Status { get; set; } = "Pending"; // Pending, Approved, Rejected, Sold, OutOfStock
        
        public bool IsActive { get; set; } = true;
        
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        
        public DateTime UpdatedDate { get; set; } = DateTime.Now;
        
        public int ViewCount { get; set; } = 0;
        
        [ForeignKey("SellerID")]
        public virtual Seller Seller { get; set; }
        
        [ForeignKey("CategoryID")]
        public virtual Category Category { get; set; }
        
        public virtual ICollection<ProductImage> Images { get; set; }
        public virtual ICollection<ProductReview> Reviews { get; set; }
    }
    
    public class ProductImage
    {
        [Key]
        public int ImageID { get; set; }
        
        public int ProductID { get; set; }
        
        [Required]
        [StringLength(255)]
        public string ImageURL { get; set; }
        
        public bool IsPrimary { get; set; } = false;
        
        public int DisplayOrder { get; set; } = 0;
        
        public DateTime UploadedDate { get; set; } = DateTime.Now;
        
        [ForeignKey("ProductID")]
        public virtual Product Product { get; set; }
    }
    
    // =============================================
    // SHOPPING CART & ORDERS
    // =============================================
    
    public class ShoppingCart
    {
        [Key]
        public int CartID { get; set; }
        
        public int CustomerID { get; set; }
        
        public int ProductID { get; set; }
        
        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
        
        public DateTime AddedDate { get; set; } = DateTime.Now;
        
        [ForeignKey("CustomerID")]
        public virtual Customer Customer { get; set; }
        
        [ForeignKey("ProductID")]
        public virtual Product Product { get; set; }
        
        [NotMapped]
        public decimal Subtotal => Product?.Price * Quantity ?? 0;
    }
    
    public class Order
    {
        [Key]
        public int OrderID { get; set; }
        
        public int CustomerID { get; set; }
        
        public DateTime OrderDate { get; set; } = DateTime.Now;
        
        [Required]
        public decimal TotalAmount { get; set; }
        
        [StringLength(30)]
        public string PaymentMethod { get; set; } // CreditCard, DebitCard, PayPal, CashOnDelivery
        
        [StringLength(20)]
        public string PaymentStatus { get; set; } = "Pending"; // Pending, Completed, Failed, Refunded
        
        [StringLength(20)]
        public string OrderStatus { get; set; } = "Processing"; // Processing, Shipped, Delivered, Cancelled
        
        [StringLength(255)]
        public string ShippingAddress { get; set; }
        
        [StringLength(50)]
        public string ShippingCity { get; set; }
        
        [StringLength(50)]
        public string ShippingState { get; set; }
        
        [StringLength(10)]
        public string ShippingZipCode { get; set; }
        
        [StringLength(50)]
        public string TrackingNumber { get; set; }
        
        public DateTime? DeliveryDate { get; set; }
        
        public string Notes { get; set; }
        
        [ForeignKey("CustomerID")]
        public virtual Customer Customer { get; set; }
        
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
    
    public class OrderItem
    {
        [Key]
        public int OrderItemID { get; set; }
        
        public int OrderID { get; set; }
        
        public int ProductID { get; set; }
        
        public int SellerID { get; set; }
        
        [Required]
        public int Quantity { get; set; }
        
        [Required]
        public decimal UnitPrice { get; set; }
        
        [NotMapped]
        public decimal TotalPrice => Quantity * UnitPrice;
        
        [ForeignKey("OrderID")]
        public virtual Order Order { get; set; }
        
        [ForeignKey("ProductID")]
        public virtual Product Product { get; set; }
        
        [ForeignKey("SellerID")]
        public virtual Seller Seller { get; set; }
    }
    
    // =============================================
    // PET ADOPTION MODELS
    // =============================================
    
    public class Shelter
    {
        [Key]
        public int ShelterID { get; set; }
        
        [Required]
        [StringLength(100)]
        public string ShelterName { get; set; }
        
        [Required]
        [StringLength(100)]
        public string ContactPerson { get; set; }
        
        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }
        
        [Required]
        [StringLength(20)]
        public string Phone { get; set; }
        
        [StringLength(255)]
        public string Address { get; set; }
        
        [StringLength(50)]
        public string City { get; set; }
        
        [StringLength(50)]
        public string State { get; set; }
        
        [StringLength(10)]
        public string ZipCode { get; set; }
        
        [StringLength(255)]
        public string Website { get; set; }
        
        public string Description { get; set; }
        
        public bool IsVerified { get; set; } = false;
        
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
    
    public class AdoptionPet
    {
        [Key]
        public int PetID { get; set; }
        
        public int ShelterID { get; set; }
        
        [Required]
        [StringLength(50)]
        public string PetName { get; set; }
        
        [Required]
        [StringLength(30)]
        public string Species { get; set; } // Dog, Cat, Bird, Rabbit, Other
        
        [StringLength(50)]
        public string Breed { get; set; }
        
        public int? Age { get; set; }
        
        [StringLength(10)]
        public string Gender { get; set; } // Male, Female, Unknown
        
        [StringLength(20)]
        public string Size { get; set; } // Small, Medium, Large, Extra Large
        
        [StringLength(50)]
        public string Color { get; set; }
        
        public string Description { get; set; }
        
        public string HealthStatus { get; set; }
        
        public bool Vaccinated { get; set; } = false;
        
        public bool Neutered { get; set; } = false;
        
        public bool GoodWithKids { get; set; } = false;
        
        public bool GoodWithPets { get; set; } = false;
        
        public decimal AdoptionFee { get; set; } = 0;
        
        [StringLength(255)]
        public string ImageURL { get; set; }
        
        [StringLength(20)]
        public string Status { get; set; } = "Available"; // Available, Pending, Adopted
        
        public DateTime ListedDate { get; set; } = DateTime.Now;
        
        public DateTime? AdoptedDate { get; set; }
        
        [ForeignKey("ShelterID")]
        public virtual Shelter Shelter { get; set; }
    }
    
    public class AdoptionApplication
    {
        [Key]
        public int ApplicationID { get; set; }
        
        public int PetID { get; set; }
        
        public int CustomerID { get; set; }
        
        public DateTime ApplicationDate { get; set; } = DateTime.Now;
        
        [StringLength(20)]
        public string Status { get; set; } = "Pending"; // Pending, Approved, Rejected
        
        [StringLength(50)]
        public string HomeType { get; set; }
        
        public bool HasYard { get; set; } = false;
        
        public bool HasOtherPets { get; set; } = false;
        
        public string Experience { get; set; }
        
        public string Reason { get; set; }
        
        public string Notes { get; set; }
        
        public DateTime? ReviewedDate { get; set; }
        
        public int? ReviewedBy { get; set; }
        
        [ForeignKey("PetID")]
        public virtual AdoptionPet Pet { get; set; }
        
        [ForeignKey("CustomerID")]
        public virtual Customer Customer { get; set; }
    }
    
    // =============================================
    // REVIEWS & BLOG
    // =============================================
    
    public class ProductReview
    {
        [Key]
        public int ReviewID { get; set; }
        
        public int ProductID { get; set; }
        
        public int CustomerID { get; set; }
        
        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }
        
        public string ReviewText { get; set; }
        
        public DateTime ReviewDate { get; set; } = DateTime.Now;
        
        public bool IsVerifiedPurchase { get; set; } = false;
        
        public int HelpfulCount { get; set; } = 0;
        
        [ForeignKey("ProductID")]
        public virtual Product Product { get; set; }
        
        [ForeignKey("CustomerID")]
        public virtual Customer Customer { get; set; }
    }
    
    public class BlogPost
    {
        [Key]
        public int PostID { get; set; }
        
        public int AuthorID { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Title { get; set; }
        
        [Required]
        public string Content { get; set; }
        
        [StringLength(50)]
        public string Category { get; set; }
        
        [StringLength(255)]
        public string ImageURL { get; set; }
        
        [StringLength(255)]
        public string Tags { get; set; }
        
        public int ViewCount { get; set; } = 0;
        
        public bool IsPublished { get; set; } = false;
        
        public DateTime? PublishedDate { get; set; }
        
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        
        public DateTime UpdatedDate { get; set; } = DateTime.Now;
        
        [ForeignKey("AuthorID")]
        public virtual Admin Author { get; set; }
    }
    
    // =============================================
    // WISHLIST & NOTIFICATIONS
    // =============================================
    
    public class Wishlist
    {
        [Key]
        public int WishlistID { get; set; }
        
        public int CustomerID { get; set; }
        
        public int ProductID { get; set; }
        
        public DateTime AddedDate { get; set; } = DateTime.Now;
        
        [ForeignKey("CustomerID")]
        public virtual Customer Customer { get; set; }
        
        [ForeignKey("ProductID")]
        public virtual Product Product { get; set; }
    }
    
    public class Notification
    {
        [Key]
        public int NotificationID { get; set; }
        
        public int UserID { get; set; }
        
        [StringLength(30)]
        public string NotificationType { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Title { get; set; }
        
        [Required]
        public string Message { get; set; }
        
        public bool IsRead { get; set; } = false;
        
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        
        [ForeignKey("UserID")]
        public virtual User User { get; set; }
    }
    
    // =============================================
    // ACTIVITY LOG
    // =============================================
    
    public class ActivityLog
    {
        [Key]
        public int LogID { get; set; }
        
        public int? UserID { get; set; }
        
        [Required]
        [StringLength(50)]
        public string ActionType { get; set; }
        
        public string ActionDescription { get; set; }
        
        [StringLength(50)]
        public string IPAddress { get; set; }
        
        public DateTime Timestamp { get; set; } = DateTime.Now;
        
        [ForeignKey("UserID")]
        public virtual User User { get; set; }
    }
}
