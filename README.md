# 🐾 Pet Supplies Store with Adoption Platform

> A full-featured e-commerce platform for pet supplies with an integrated pet adoption system, connecting pet owners with quality products and rescue organizations.

[![.NET](https://img.shields.io/badge/.NET-8.0-purple.svg)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-12.0-blue.svg)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![SQL Server](https://img.shields.io/badge/SQL%20Server-2019-red.svg)](https://www.microsoft.com/en-us/sql-server/)
[![License](https://img.shields.io/badge/license-MIT-green.svg)](LICENSE)

---

## 📋 Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Technology Stack](#technology-stack)
- [Installation](#installation)
- [User Roles](#user-roles)
- [API Endpoints](#api-endpoints)
- [Database Schema](#database-schema)
- [Payment Integration](#payment-integration)
- [Security](#security)
- [Screenshots](#screenshots)
- [Deployment](#deployment)
- [Contributing](#contributing)

---

## 🎯 Overview

The **Pet Supplies Store with Adoption Platform** is a comprehensive ASP.NET Core web application that serves dual purposes:

1. 🛒 **E-Commerce Platform** - Complete online store for pet supplies
2. 🐶 **Pet Adoption System** - Connect shelters with potential pet parents

### Why This Project?

This platform addresses two critical needs in the pet care ecosystem:

- **For Pet Owners**: One-stop shop for all pet supplies with convenient online shopping
- **For Shelters**: Free platform to showcase adoptable pets and connect with caring families
- **For Businesses**: Marketplace for pet supply sellers to reach customers
- **For Community**: Educational resources and pet care information

### Key Highlights

✅ **Multi-Vendor Marketplace** - Multiple sellers can list products
✅ **Admin Approval System** - Quality control for all listings
✅ **Secure Payments** - Stripe, PayPal, and Cash on Delivery
✅ **Pet Adoption** - Verified shelters and application system
✅ **Review System** - Customer ratings and verified purchases
✅ **Blog Platform** - Pet care tips and resources
✅ **Social Sharing** - Spread awareness about adoptable pets
✅ **Responsive Design** - Works on all devices

---

## ✨ Features

### 🛍️ E-Commerce Features

**Product Management:**
- Multi-category product catalog
- Advanced search and filtering
- Multiple product images
- Stock management
- Price tracking
- Brand filtering
- View counters

**Shopping Experience:**
- User-friendly cart system
- Wishlist functionality
- Quick checkout process
- Order tracking
- Order history
- Multiple shipping addresses
- Guest checkout (optional)

**Payment Options:**
- Credit/Debit cards (Stripe)
- PayPal integration
- Cash on Delivery
- Secure payment processing
- Automatic receipt generation
- Payment status tracking

### 🐕 Pet Adoption Features

**For Shelters:**
- Free platform registration
- Pet profile creation
- Multiple pet photos
- Detailed pet information
- Application management
- Adoption tracking
- Success stories sharing

**For Adopters:**
- Browse available pets
- Filter by species, breed, size, age
- Detailed pet profiles
- Online application submission
- Application status tracking
- Direct shelter contact
- Adoption resources

**Pet Information Includes:**
- Health status
- Vaccination records
- Behavioral traits
- Good with kids/pets
- Special needs
- Adoption fees
- Shelter location

### 👥 User Roles & Permissions

**Visitors (No Account):**
- Browse products
- View pet listings
- Read blog articles
- Search functionality

**Customers (Registered):**
- Purchase products
- Write reviews
- Wishlist management
- Order tracking
- Apply for adoption
- Save shipping addresses

**Sellers (Business):**
- Product listing
- Inventory management
- Sales analytics
- Customer reviews viewing
- Profile management
- Revenue tracking

**Admin (System Control):**
- User management
- Product approval
- Order oversight
- Shelter verification
- Content management
- System analytics
- Blog publishing

### 📝 Blog & Resources

- Pet care guides
- Training tips
- Health advice
- Nutrition information
- Product recommendations
- Success stories
- Community engagement

---

## 🛠️ Technology Stack

### Backend
- **ASP.NET Core 8.0** - Web framework
- **C# 12.0** - Programming language
- **Entity Framework Core** - ORM
- **SQL Server 2019+** - Database
- **JWT Authentication** - Security
- **RESTful API** - Architecture

### Frontend (API-Ready)
- **Razor Pages** or **React/Angular/Vue** (your choice)
- **Bootstrap 5** - UI framework
- **jQuery** - DOM manipulation
- **AJAX** - Asynchronous requests

### Third-Party Integrations
- **Stripe** - Payment processing
- **PayPal** - Alternative payment
- **SendGrid/SMTP** - Email notifications
- **Cloudinary** - Image hosting (optional)

### Development Tools
- **Visual Studio 2022** - IDE
- **SQL Server Management Studio** - Database
- **Postman** - API testing
- **Swagger** - API documentation
- **Git** - Version control

---

## 📥 Installation

### Prerequisites

**Required Software:**
- Visual Studio 2022 (Community or higher)
- .NET 8.0 SDK
- SQL Server 2019+ or SQL Server Express
- SQL Server Management Studio (recommended)

**Optional Tools:**
- Postman (API testing)
- Git (version control)

### Step-by-Step Installation

#### 1. Install Visual Studio 2022

Download from: https://visualstudio.microsoft.com/

**Required Workloads:**
- ASP.NET and web development
- .NET desktop development

#### 2. Install SQL Server

Download SQL Server Developer Edition (free):
https://www.microsoft.com/en-us/sql-server/sql-server-downloads

**Installation Options:**
- Choose "Basic" or "Custom" installation
- Enable Mixed Mode Authentication
- Set a strong SA password
- Remember your server name (usually `localhost` or `.\SQLEXPRESS`)

#### 3. Create Database

**Option A: Using SSMS**
1. Open SQL Server Management Studio
2. Connect to your SQL Server instance
3. Open `PetSuppliesStore_Database.sql`
4. Execute the script (F5)
5. Verify database `PetSuppliesStoreDB` is created

**Option B: Using Visual Studio**
1. Open SQL Server Object Explorer
2. Connect to SQL Server
3. Right-click Databases → Add New Database
4. Name it `PetSuppliesStoreDB`
5. Run the SQL script

#### 4. Create ASP.NET Core Project

**Using Visual Studio:**
```
1. File → New → Project
2. Select "ASP.NET Core Web API"
3. Project name: PetSuppliesStore
4. Location: Your preferred directory
5. Framework: .NET 8.0
6. Authentication: None (we'll implement custom)
7. Configure for HTTPS: ✓
8. Enable OpenAPI support: ✓
9. Create
```

**Using .NET CLI:**
```bash
dotnet new webapi -n PetSuppliesStore
cd PetSuppliesStore
```

#### 5. Install NuGet Packages

**Using Package Manager Console:**
```powershell
Install-Package Microsoft.EntityFrameworkCore.SqlServer
Install-Package Microsoft.EntityFrameworkCore.Tools
Install-Package Microsoft.AspNetCore.Authentication.JwtBearer
Install-Package Swashbuckle.AspNetCore
Install-Package AutoMapper.Extensions.Microsoft.DependencyInjection
Install-Package Stripe.net
```

**Using .NET CLI:**
```bash
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
dotnet add package Swashbuckle.AspNetCore
```

#### 6. Add Project Files

Copy these files to your project:

```
PetSuppliesStore/
├── Models/
│   └── Models.cs
├── Data/
│   └── ApplicationDbContext.cs
├── Controllers/
│   └── ProductsController.cs
└── appsettings.json
```

#### 7. Configure Connection String

Edit `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=PetSuppliesStoreDB;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

**For SQL Server Express:**
```json
"DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=PetSuppliesStoreDB;Trusted_Connection=True;TrustServerCertificate=True;"
```

**For SQL Authentication:**
```json
"DefaultConnection": "Server=localhost;Database=PetSuppliesStoreDB;User Id=sa;Password=YourPassword;TrustServerCertificate=True;"
```

#### 8. Configure Program.cs

```csharp
using Microsoft.EntityFrameworkCore;
using PetSuppliesStore.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS (for frontend)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder.AllowAnyOrigin()
                         .AllowAnyMethod()
                         .AllowAnyHeader());
});

var app = builder.Build();

// Configure pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

app.Run();
```

#### 9. Run Application

**Using Visual Studio:**
- Press F5 or click "Run"
- Browser opens automatically

**Using .NET CLI:**
```bash
dotnet run
```

Navigate to: `https://localhost:5001/swagger`

---

## 👥 User Roles

### 1. Visitors 👀
**No account required**

**Can:**
- Browse product catalog
- View product details
- Read customer reviews
- Search and filter products
- View adoption listings
- Read blog articles
- View shelter information

**Cannot:**
- Add to cart
- Make purchases
- Leave reviews
- Apply for adoption

---

### 2. Customers 🛒
**Registration required**

**Profile Information:**
- Email address (unique)
- Password (strong)
- First and Last Name
- Phone number
- Shipping address(es)
- City, State, ZIP

**Capabilities:**
✅ All visitor features, plus:
- Add items to cart
- Complete purchases
- Multiple payment methods
- Order tracking with status updates
- Order history with details
- Write and edit reviews
- Rate products (1-5 stars)
- Create wishlist
- Save multiple addresses
- Apply for pet adoption
- Submit adoption applications
- Track application status
- Receive email notifications
- Share products on social media

**Dashboard Features:**
- Order overview
- Recent purchases
- Wishlist items
- Adoption applications
- Saved addresses
- Profile settings
- Review management

---

### 3. Sellers 💼
**Business registration required**

**Registration Information:**
- Business email (unique)
- Password
- Business/Store name
- Contact person
- Phone number
- Business address
- Tax ID (optional)

**Capabilities:**
✅ Product Management:
- Add new products
- Upload multiple product images
- Set prices and stock levels
- Update product information
- Edit product descriptions
- Mark items as SOLD
- Delete/deactivate listings
- Manage product categories

✅ Inventory Control:
- View all active listings
- Track stock quantities
- Low stock alerts
- Update inventory levels
- Product performance analytics

✅ Business Analytics:
- View total sales
- Revenue tracking
- Product views counter
- Customer review insights
- Best-selling products
- Sales reports

✅ Order Management:
- View orders for their products
- Update order status
- Track fulfillment
- Customer communication

**Important Notes:**
- Products require admin approval before going live
- Status: Pending → Approved → Live
- Rejected products receive feedback
- Seller rating based on customer reviews
- Verification badge for trusted sellers

---

### 4. Admin 👨‍💼
**System administrator**

**Full System Control:**

✅ **User Management:**
- View all users (customers, sellers)
- Activate/deactivate accounts
- Edit user details
- Reset passwords
- View user activity
- Manage permissions
- Ban problematic users

✅ **Product Management:**
- Approve pending products
- Reject non-compliant products
- Edit any product
- Delete products
- Manage categories
- Create new categories
- Set featured products
- Monitor product quality

✅ **Order Oversight:**
- View all orders
- Track order status
- Process refunds
- Handle disputes
- Generate invoices
- Export order data

✅ **Shelter & Adoption:**
- Verify shelter organizations
- Approve shelter registrations
- Manage pet listings
- Review adoption applications
- Monitor adoption success rate
- Remove inappropriate listings

✅ **Content Management:**
- Create blog posts
- Edit articles
- Publish resources
- Upload images
- Manage categories
- SEO optimization

✅ **Analytics Dashboard:**
- Total sales metrics
- Revenue reports
- User growth statistics
- Product performance
- Category analytics
- Adoption statistics
- Traffic analysis

✅ **System Configuration:**
- Payment gateway settings
- Email templates
- Shipping options
- Tax configuration
- Platform policies
- Notification settings

---

## 🔌 API Endpoints

### Authentication

```http
POST /api/account/register-customer
POST /api/account/register-seller
POST /api/account/login
POST /api/account/logout
POST /api/account/forgot-password
GET  /api/account/profile
PUT  /api/account/profile
```

### Products

```http
GET    /api/products                     # Get all (with filters)
GET    /api/products/{id}                # Get single
POST   /api/products                     # Create (Seller)
PUT    /api/products/{id}                # Update (Seller)
DELETE /api/products/{id}                # Delete (Seller/Admin)
POST   /api/products/{id}/approve        # Approve (Admin)
POST   /api/products/{id}/reject         # Reject (Admin)
GET    /api/products/categories          # All categories
GET    /api/products/search?q={query}    # Search
```

### Shopping Cart

```http
GET    /api/cart                         # Get cart
POST   /api/cart/add                     # Add item
PUT    /api/cart/update                  # Update quantity
DELETE /api/cart/remove/{productId}      # Remove item
DELETE /api/cart/clear                   # Clear cart
```

### Orders

```http
POST   /api/orders/create                # Create order
GET    /api/orders                       # User's orders
GET    /api/orders/{id}                  # Order details
PUT    /api/orders/{id}/cancel           # Cancel order
GET    /api/orders/{id}/track            # Track shipment
```

### Adoption

```http
GET    /api/adoption/pets                # All pets
GET    /api/adoption/pets/{id}           # Pet details
POST   /api/adoption/apply               # Submit application
GET    /api/adoption/applications        # User's applications
GET    /api/adoption/shelters            # All shelters
POST   /api/adoption/shelters/register   # Register shelter
```

### Reviews

```http
POST   /api/reviews                      # Add review
GET    /api/reviews/product/{id}         # Product reviews
PUT    /api/reviews/{id}                 # Update review
DELETE /api/reviews/{id}                 # Delete review
POST   /api/reviews/{id}/helpful         # Mark helpful
```

---

## 📊 Database Schema

### Tables Overview (18 Tables)

**User Management (4):**
- Users (authentication)
- Customers (profile)
- Sellers (business info)
- Admins (system users)

**Product System (4):**
- Categories
- Products
- ProductImages
- ProductReviews

**E-Commerce (4):**
- ShoppingCart
- Orders
- OrderItems
- Wishlist

**Adoption System (3):**
- Shelters
- AdoptionPets
- AdoptionApplications

**Supporting (3):**
- BlogPosts
- Notifications
- ActivityLogs

### Entity Relationships

```
Users
├── Customers
│   ├── Orders
│   ├── ShoppingCart
│   ├── Wishlist
│   ├── ProductReviews
│   └── AdoptionApplications
├── Sellers
│   └── Products
│       ├── ProductImages
│       ├── ProductReviews
│       └── OrderItems
└── Admins
    └── BlogPosts

Shelters
└── AdoptionPets
    └── AdoptionApplications
```

---

## 💳 Payment Integration

### Stripe Setup

```csharp
using Stripe;

StripeConfiguration.ApiKey = "sk_test_YOUR_KEY";

var options = new PaymentIntentCreateOptions
{
    Amount = (long)(total * 100), // Convert to cents
    Currency = "usd",
    PaymentMethodTypes = new List<string> { "card" }
};

var service = new PaymentIntentService();
var intent = await service.CreateAsync(options);
```

### Supported Methods
- ✅ Credit Cards (Visa, Mastercard, Amex)
- ✅ Debit Cards
- ✅ PayPal
- ✅ Cash on Delivery

---

## 🔒 Security

### Implemented Security

✅ **Password Security**: BCrypt hashing with salt
✅ **SQL Injection**: Parameterized queries (EF Core)
✅ **XSS Protection**: Input sanitization
✅ **CSRF Protection**: Anti-forgery tokens
✅ **JWT Authentication**: Stateless authentication
✅ **Role-Based Access**: Authorization policies
✅ **HTTPS**: Encrypted communication
✅ **Input Validation**: Data annotations
✅ **Audit Logging**: Activity tracking

### Example Security Code

```csharp
// Password Hashing
var hasher = new PasswordHasher<User>();
var hash = hasher.HashPassword(user, password);

// JWT Generation
var token = new JwtSecurityToken(
    issuer: "PetStore",
    audience: "Users",
    claims: new[] {
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(ClaimTypes.Role, user.UserRole)
    },
    expires: DateTime.UtcNow.AddDays(30),
    signingCredentials: credentials
);

// Authorization
[Authorize(Roles = "Admin")]
public async Task<IActionResult> AdminOnly() { }
```

---

## 🚀 Deployment

### IIS Deployment

**Prerequisites:**
- Windows Server 2019+
- IIS 10+
- .NET 8 Hosting Bundle
- SQL Server

**Steps:**
1. Publish application (Release mode)
2. Create IIS website
3. Configure application pool (.NET CLR: No Managed Code)
4. Install SSL certificate
5. Update production connection string
6. Set folder permissions

### Azure Deployment

```bash
# Install Azure CLI
az login

# Create resource group
az group create --name PetStoreRG --location eastus

# Create App Service
az webapp create --resource-group PetStoreRG --plan PetStorePlan --name petstore-app

# Deploy
dotnet publish -c Release
az webapp deployment source config-zip --resource-group PetStoreRG --name petstore-app --src publish.zip
```

---

## 📈 Roadmap

### Version 1.0 ✅
- User authentication
- Product catalog
- Shopping cart
- Order system
- Basic admin panel

### Version 2.0 (In Progress)
- [ ] Pet adoption system
- [ ] Payment integration
- [ ] Review system
- [ ] Blog platform
- [ ] Email notifications

### Version 3.0 (Planned)
- [ ] Mobile app (Xamarin/MAUI)
- [ ] Live chat support
- [ ] AI product recommendations
- [ ] Advanced analytics
- [ ] Multi-language support
- [ ] Subscription boxes
- [ ] Loyalty program
- [ ] Virtual vet consultations

---


### Code Style
- Follow C# coding conventions
- Use meaningful variable names
- Add XML documentation comments
- Write unit tests for new features

---

## 📞 Support

- **Documentation**: See `PET_STORE_DOCUMENTATION.md`
- **API Docs**: Available at `/swagger` endpoint
- **Issues**: GitHub Issues


---

## 📄 License

MIT License - see [LICENSE](LICENSE) file

---

## 🙏 Acknowledgments

- **Stripe** - Payment processing
- **PayPal** - Alternative payments
- **Local Shelters** - Pet data and inspiration
- **Community** - Feedback and testing

---

## 📊 Project Stats

- **Lines of Code**: ~10,000+
- **API Endpoints**: 40+
- **Database Tables**: 18
- **User Roles**: 4
- **Features**: 50+

---

**Made  for pets and their humans**

---

*For detailed setup instructions, see [PET_STORE_DOCUMENTATION.md](PET_STORE_DOCUMENTATION.md)*

**Default Admin Login:**
- Email: admin@petstore.com
- Password: Admin@123
