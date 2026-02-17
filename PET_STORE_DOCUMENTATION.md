# Pet Supplies Store with Adoption Platform
## Complete ASP.NET Core Web Application Guide

---

## 📋 Project Overview

Full-featured e-commerce platform for pet supplies with integrated pet adoption system. Built with **ASP.NET Core 8.0**, **C#**, **Entity Framework Core**, and **SQL Server**.

### Core Features
✅ Multi-role system (Visitors, Customers, Sellers, Admin)
✅ Product management with approval workflow
✅ Shopping cart and checkout
✅ Multiple payment methods
✅ Pet adoption with shelter partnerships
✅ Product reviews and ratings
✅ Blog/Resource center
✅ Social media sharing
✅ Admin dashboard

---

## 💻 Quick Start

### Prerequisites
- Visual Studio 2022
- .NET 8.0 SDK
- SQL Server 2019+
- SQL Server Management Studio (optional)

### Installation Steps

**1. Create Database**
```sql
-- Run the PetSuppliesStore_Database.sql file in SSMS
-- This creates all tables, stored procedures, and sample data
```

**2. Create ASP.NET Core Project**
```bash
dotnet new webapi -n PetSuppliesStore
cd PetSuppliesStore
```

**3. Install NuGet Packages**
```bash
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
dotnet add package Swashbuckle.AspNetCore
```

**4. Configure Connection String**
Edit `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=PetSuppliesStoreDB;Trusted_Connection=True;"
  }
}
```

**5. Add Files to Project**
- Copy `Models.cs` to `Models/` folder
- Copy `ApplicationDbContext.cs` to `Data/` folder
- Copy `ProductsController.cs` to `Controllers/` folder

**6. Configure Program.cs**
```csharp
using Microsoft.EntityFrameworkCore;
using PetSuppliesStore.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
```

**7. Run Application**
```bash
dotnet run
```

Navigate to: `https://localhost:5001/swagger`

---

## 📁 Complete Project Structure

```
PetSuppliesStore/
│
├── Controllers/
│   ├── AccountController.cs          # Authentication
│   ├── ProductsController.cs         # Product management
│   ├── CartController.cs             # Shopping cart
│   ├── OrdersController.cs           # Orders
│   ├── AdoptionController.cs         # Pet adoption
│   ├── ReviewsController.cs          # Reviews
│   └── AdminController.cs            # Admin functions
│
├── Models/
│   ├── User.cs                       # Base user model
│   ├── Customer.cs                   # Customer details
│   ├── Seller.cs                     # Seller details
│   ├── Product.cs                    # Product model
│   ├── Order.cs                      # Order model
│   ├── AdoptionPet.cs               # Pet for adoption
│   └── [18 total model files]
│
├── Data/
│   └── ApplicationDbContext.cs       # EF Core context
│
├── DTOs/
│   ├── LoginDto.cs
│   ├── RegisterDto.cs
│   ├── ProductDto.cs
│   └── OrderDto.cs
│
├── Services/
│   ├── AuthService.cs
│   ├── ProductService.cs
│   ├── EmailService.cs
│   └── PaymentService.cs
│
├── wwwroot/
│   └── images/
│       ├── products/
│       ├── pets/
│       └── blog/
│
├── appsettings.json
└── Program.cs
```

---

## 👥 User Roles & Capabilities

### 1. Visitors (No Account)
- Browse products
- View product details
- Read reviews
- View adoption listings
- Search products

### 2. Customers
- All visitor features
- Purchase products
- Write reviews
- Shopping cart
- Order tracking
- Wishlist
- Apply for pet adoption

### 3. Sellers
- Add/edit/delete products
- Manage inventory
- View sales reports
- Update product status
- Upload product images

### 4. Admin
- Approve/reject products
- Manage users
- Manage orders
- Verify shelters
- Create blog posts
- System analytics

---

## 🔌 API Endpoints Reference

### Products
```
GET    /api/products                    # Get all products
GET    /api/products/{id}               # Get product by ID
POST   /api/products                    # Create product (Seller)
PUT    /api/products/{id}               # Update product (Seller)
DELETE /api/products/{id}               # Delete product
POST   /api/products/{id}/approve       # Approve (Admin)
GET    /api/products/categories         # Get categories
```

### Authentication
```
POST   /api/account/register-customer   # Register customer
POST   /api/account/register-seller     # Register seller
POST   /api/account/login               # Login
GET    /api/account/profile             # Get profile
```

### Shopping Cart
```
GET    /api/cart                        # Get cart
POST   /api/cart/add                    # Add to cart
DELETE /api/cart/remove/{id}            # Remove item
```

### Orders
```
POST   /api/orders/create               # Create order
GET    /api/orders                      # Get user orders
GET    /api/orders/{id}                 # Get order details
```

### Adoption
```
GET    /api/adoption/pets               # Get all pets
POST   /api/adoption/apply              # Submit application
GET    /api/adoption/applications       # Get applications
```

---

## 🗄️ Database Schema Overview

### Core Tables
1. **Users** - Base authentication (Email, Password, Role)
2. **Customers** - Customer details and shipping info
3. **Sellers** - Seller business information
4. **Admins** - Admin user details

### Product Tables
5. **Categories** - Product categories
6. **Products** - Main product table
7. **ProductImages** - Multiple images per product
8. **ProductReviews** - Customer reviews

### Order Tables
9. **ShoppingCart** - Cart items
10. **Orders** - Order header
11. **OrderItems** - Order line items

### Adoption Tables
12. **Shelters** - Rescue organizations
13. **AdoptionPets** - Pets available for adoption
14. **AdoptionApplications** - Adoption requests

### Supporting Tables
15. **Wishlist** - Customer wishlists
16. **Notifications** - System notifications
17. **BlogPosts** - Blog articles
18. **ActivityLogs** - System audit trail

---

## 🔒 Security Implementation

### Password Hashing
```csharp
using Microsoft.AspNetCore.Identity;

public class PasswordHelper
{
    public static string HashPassword(string password)
    {
        var hasher = new PasswordHasher<object>();
        return hasher.HashPassword(null, password);
    }
    
    public static bool VerifyPassword(string hash, string password)
    {
        var hasher = new PasswordHasher<object>();
        var result = hasher.VerifyHashedPassword(null, hash, password);
        return result == PasswordVerificationResult.Success;
    }
}
```

### JWT Authentication
```csharp
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

public class JwtTokenGenerator
{
    public string GenerateToken(User user, string secret)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(secret);
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.UserRole),
                new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString())
            }),
            Expires = DateTime.UtcNow.AddDays(30),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };
        
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
```

### Authorization
```csharp
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    // Admin-only endpoints
}

[Authorize(Roles = "Seller")]
public class SellerController : ControllerBase
{
    // Seller endpoints
}

[Authorize] // Any authenticated user
public class ProfileController : ControllerBase
{
    // Authenticated user endpoints
}
```

---

## 💳 Payment Integration

### Stripe Setup
```csharp
using Stripe;

public class PaymentService
{
    public async Task<PaymentIntent> CreatePaymentIntent(decimal amount)
    {
        StripeConfiguration.ApiKey = "sk_test_YOUR_KEY";
        
        var options = new PaymentIntentCreateOptions
        {
            Amount = (long)(amount * 100), // Convert to cents
            Currency = "usd",
            PaymentMethodTypes = new List<string> { "card" }
        };
        
        var service = new PaymentIntentService();
        return await service.CreateAsync(options);
    }
}
```

### Cash on Delivery
```csharp
public async Task<Order> CreateCODOrder(int customerId, decimal total)
{
    var order = new Order
    {
        CustomerID = customerId,
        TotalAmount = total,
        PaymentMethod = "CashOnDelivery",
        PaymentStatus = "Pending",
        OrderStatus = "Processing"
    };
    
    _context.Orders.Add(order);
    await _context.SaveChangesAsync();
    
    return order;
}
```

---

## 📧 Email Notifications

### Email Service
```csharp
using System.Net;
using System.Net.Mail;

public class EmailService
{
    private readonly IConfiguration _config;
    
    public async Task SendOrderConfirmation(string toEmail, Order order)
    {
        var smtpClient = new SmtpClient(_config["AppSettings:SmtpHost"])
        {
            Port = int.Parse(_config["AppSettings:SmtpPort"]),
            Credentials = new NetworkCredential(
                _config["AppSettings:SmtpUsername"],
                _config["AppSettings:SmtpPassword"]),
            EnableSsl = true,
        };
        
        var mailMessage = new MailMessage
        {
            From = new MailAddress(_config["AppSettings:SupportEmail"]),
            Subject = $"Order Confirmation - Order #{order.OrderID}",
            Body = $"Your order has been confirmed. Total: ${order.TotalAmount}",
            IsBodyHtml = true,
        };
        
        mailMessage.To.Add(toEmail);
        
        await smtpClient.SendMailAsync(mailMessage);
    }
}
```

---

## 🧪 Testing

### Unit Test Example
```csharp
using Xunit;
using Moq;

public class ProductServiceTests
{
    [Fact]
    public async Task GetProduct_ValidId_ReturnsProduct()
    {
        // Arrange
        var mockRepo = new Mock<IProductRepository>();
        mockRepo.Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(new Product { ProductID = 1, ProductName = "Test" });
        
        var service = new ProductService(mockRepo.Object);
        
        // Act
        var result = await service.GetProductAsync(1);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test", result.ProductName);
    }
}
```

---

## 🚀 Deployment to IIS

### Publishing Steps
1. Right-click project → **Publish**
2. Choose **Folder** target
3. Configuration: **Release**
4. Target Runtime: **win-x64**
5. Click **Publish**

### IIS Configuration
1. Install **IIS** via Windows Features
2. Install **.NET 8 Hosting Bundle**
3. Create new site in IIS Manager
4. Set physical path to published folder
5. Configure app pool (No Managed Code)
6. Start website

### Production Connection String
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=PROD_SERVER;Database=PetSuppliesStoreDB;User Id=sa;Password=STRONG_PASSWORD;TrustServerCertificate=True;"
  }
}
```

---

## 📊 Sample Code Snippets

### Register Customer
```csharp
[HttpPost("register-customer")]
public async Task<IActionResult> RegisterCustomer([FromBody] CustomerRegisterDto dto)
{
    var user = new User
    {
        Email = dto.Email,
        PasswordHash = PasswordHelper.HashPassword(dto.Password),
        UserRole = "Customer"
    };
    
    _context.Users.Add(user);
    await _context.SaveChangesAsync();
    
    var customer = new Customer
    {
        UserID = user.UserID,
        FirstName = dto.FirstName,
        LastName = dto.LastName,
        Phone = dto.Phone,
        Address = dto.Address
    };
    
    _context.Customers.Add(customer);
    await _context.SaveChangesAsync();
    
    var token = _jwtGenerator.GenerateToken(user);
    
    return Ok(new { token, message = "Registration successful" });
}
```

### Add to Cart
```csharp
[HttpPost("cart/add")]
[Authorize(Roles = "Customer")]
public async Task<IActionResult> AddToCart([FromBody] AddToCartDto dto)
{
    var customerId = GetCustomerId(); // From JWT claims
    
    var cartItem = await _context.ShoppingCarts
        .FirstOrDefaultAsync(c => c.CustomerID == customerId && c.ProductID == dto.ProductID);
    
    if (cartItem != null)
    {
        cartItem.Quantity += dto.Quantity;
    }
    else
    {
        cartItem = new ShoppingCart
        {
            CustomerID = customerId,
            ProductID = dto.ProductID,
            Quantity = dto.Quantity
        };
        _context.ShoppingCarts.Add(cartItem);
    }
    
    await _context.SaveChangesAsync();
    return Ok(new { message = "Item added to cart" });
}
```

### Create Order
```csharp
[HttpPost("orders/create")]
[Authorize(Roles = "Customer")]
public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto dto)
{
    var customerId = GetCustomerId();
    
    var cartItems = await _context.ShoppingCarts
        .Include(c => c.Product)
        .Where(c => c.CustomerID == customerId)
        .ToListAsync();
    
    if (!cartItems.Any())
        return BadRequest("Cart is empty");
    
    var totalAmount = cartItems.Sum(c => c.Product.Price * c.Quantity);
    
    var order = new Order
    {
        CustomerID = customerId,
        TotalAmount = totalAmount,
        PaymentMethod = dto.PaymentMethod,
        ShippingAddress = dto.ShippingAddress,
        ShippingCity = dto.City,
        ShippingState = dto.State,
        ShippingZipCode = dto.ZipCode
    };
    
    _context.Orders.Add(order);
    await _context.SaveChangesAsync();
    
    foreach (var item in cartItems)
    {
        var orderItem = new OrderItem
        {
            OrderID = order.OrderID,
            ProductID = item.ProductID,
            SellerID = item.Product.SellerID,
            Quantity = item.Quantity,
            UnitPrice = item.Product.Price
        };
        _context.OrderItems.Add(orderItem);
    }
    
    _context.ShoppingCarts.RemoveRange(cartItems);
    await _context.SaveChangesAsync();
    
    return Ok(new { orderId = order.OrderID, message = "Order created successfully" });
}
```

---

## ✅ Implementation Checklist

### Phase 1: Foundation (Weeks 1-2)
- [x] Database schema created
- [x] Models defined
- [x] DbContext configured
- [ ] User registration API
- [ ] Login with JWT
- [ ] Password hashing

### Phase 2: Products (Weeks 3-4)
- [ ] Product CRUD endpoints
- [ ] Image upload
- [ ] Category management
- [ ] Product search/filter
- [ ] Admin approval workflow

### Phase 3: Shopping (Weeks 5-6)
- [ ] Shopping cart
- [ ] Cart operations
- [ ] Order creation
- [ ] Order history

### Phase 4: Payments (Weeks 7-8)
- [ ] Stripe integration
- [ ] PayPal integration
- [ ] Cash on delivery
- [ ] Email notifications

### Phase 5: Adoption (Weeks 9-10)
- [ ] Pet listings
- [ ] Application form
- [ ] Admin review
- [ ] Shelter management

### Phase 6: Features (Weeks 11-12)
- [ ] Reviews system
- [ ] Social sharing
- [ ] Blog section
- [ ] Wishlist

### Phase 7: Deploy (Weeks 13-14)
- [ ] Testing complete
- [ ] IIS deployment
- [ ] SSL certificate
- [ ] Production ready

---

## 📚 Resources

- **ASP.NET Core Docs**: https://docs.microsoft.com/en-us/aspnet/core/
- **Entity Framework**: https://docs.microsoft.com/en-us/ef/core/
- **Stripe .NET**: https://stripe.com/docs/api/net
- **JWT Auth**: https://jwt.io/

---

**Default Admin Credentials**:
- Email: admin@petstore.com
- Password: Admin@123

---

Good luck with your project! 🐾
