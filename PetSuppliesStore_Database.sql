-- =============================================
-- PET SUPPLIES STORE DATABASE SCHEMA
-- SQL Server 2019+
-- =============================================

-- Create Database
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'PetSuppliesStoreDB')
BEGIN
    CREATE DATABASE PetSuppliesStoreDB;
END
GO

USE PetSuppliesStoreDB;
GO

-- =============================================
-- USER MANAGEMENT TABLES
-- =============================================

-- Users Table (Base authentication)
CREATE TABLE Users (
    UserID INT PRIMARY KEY IDENTITY(1,1),
    Email NVARCHAR(100) UNIQUE NOT NULL,
    PasswordHash NVARCHAR(255) NOT NULL,
    UserRole NVARCHAR(20) NOT NULL CHECK (UserRole IN ('Admin', 'Customer', 'Seller')),
    IsActive BIT DEFAULT 1,
    CreatedDate DATETIME DEFAULT GETDATE(),
    LastLoginDate DATETIME NULL,
    CONSTRAINT CK_Email CHECK (Email LIKE '%@%')
);
GO

-- Customers Table
CREATE TABLE Customers (
    CustomerID INT PRIMARY KEY IDENTITY(1,1),
    UserID INT UNIQUE NOT NULL,
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    Phone NVARCHAR(20),
    Address NVARCHAR(255),
    City NVARCHAR(50),
    State NVARCHAR(50),
    ZipCode NVARCHAR(10),
    Country NVARCHAR(50) DEFAULT 'USA',
    FOREIGN KEY (UserID) REFERENCES Users(UserID) ON DELETE CASCADE
);
GO

-- Sellers Table
CREATE TABLE Sellers (
    SellerID INT PRIMARY KEY IDENTITY(1,1),
    UserID INT UNIQUE NOT NULL,
    BusinessName NVARCHAR(100) NOT NULL,
    ContactPerson NVARCHAR(100) NOT NULL,
    Phone NVARCHAR(20) NOT NULL,
    Address NVARCHAR(255),
    City NVARCHAR(50),
    State NVARCHAR(50),
    ZipCode NVARCHAR(10),
    Country NVARCHAR(50) DEFAULT 'USA',
    TaxID NVARCHAR(50),
    IsVerified BIT DEFAULT 0,
    Rating DECIMAL(3,2) DEFAULT 0.00,
    TotalSales INT DEFAULT 0,
    FOREIGN KEY (UserID) REFERENCES Users(UserID) ON DELETE CASCADE
);
GO

-- Admins Table
CREATE TABLE Admins (
    AdminID INT PRIMARY KEY IDENTITY(1,1),
    UserID INT UNIQUE NOT NULL,
    FullName NVARCHAR(100) NOT NULL,
    Phone NVARCHAR(20),
    FOREIGN KEY (UserID) REFERENCES Users(UserID) ON DELETE CASCADE
);
GO

-- =============================================
-- PRODUCT MANAGEMENT TABLES
-- =============================================

-- Categories Table
CREATE TABLE Categories (
    CategoryID INT PRIMARY KEY IDENTITY(1,1),
    CategoryName NVARCHAR(50) NOT NULL UNIQUE,
    Description NVARCHAR(255),
    ImageURL NVARCHAR(255),
    IsActive BIT DEFAULT 1,
    CreatedDate DATETIME DEFAULT GETDATE()
);
GO

-- Products Table
CREATE TABLE Products (
    ProductID INT PRIMARY KEY IDENTITY(1,1),
    SellerID INT NOT NULL,
    CategoryID INT NOT NULL,
    ProductName NVARCHAR(100) NOT NULL,
    Description NVARCHAR(MAX),
    Brand NVARCHAR(50),
    Price DECIMAL(10,2) NOT NULL CHECK (Price > 0),
    StockQuantity INT NOT NULL DEFAULT 0 CHECK (StockQuantity >= 0),
    ImageURL NVARCHAR(255),
    Status NVARCHAR(20) DEFAULT 'Pending' CHECK (Status IN ('Pending', 'Approved', 'Rejected', 'Sold', 'OutOfStock')),
    IsActive BIT DEFAULT 1,
    CreatedDate DATETIME DEFAULT GETDATE(),
    UpdatedDate DATETIME DEFAULT GETDATE(),
    ViewCount INT DEFAULT 0,
    FOREIGN KEY (SellerID) REFERENCES Sellers(SellerID) ON DELETE CASCADE,
    FOREIGN KEY (CategoryID) REFERENCES Categories(CategoryID)
);
GO

-- Product Images Table (Multiple images per product)
CREATE TABLE ProductImages (
    ImageID INT PRIMARY KEY IDENTITY(1,1),
    ProductID INT NOT NULL,
    ImageURL NVARCHAR(255) NOT NULL,
    IsPrimary BIT DEFAULT 0,
    DisplayOrder INT DEFAULT 0,
    UploadedDate DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID) ON DELETE CASCADE
);
GO

-- =============================================
-- SHOPPING CART & ORDERS
-- =============================================

-- Shopping Cart Table
CREATE TABLE ShoppingCart (
    CartID INT PRIMARY KEY IDENTITY(1,1),
    CustomerID INT NOT NULL,
    ProductID INT NOT NULL,
    Quantity INT NOT NULL DEFAULT 1 CHECK (Quantity > 0),
    AddedDate DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID) ON DELETE CASCADE,
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID),
    CONSTRAINT UC_CustomerProduct UNIQUE(CustomerID, ProductID)
);
GO

-- Orders Table
CREATE TABLE Orders (
    OrderID INT PRIMARY KEY IDENTITY(1,1),
    CustomerID INT NOT NULL,
    OrderDate DATETIME DEFAULT GETDATE(),
    TotalAmount DECIMAL(10,2) NOT NULL CHECK (TotalAmount >= 0),
    PaymentMethod NVARCHAR(30) CHECK (PaymentMethod IN ('CreditCard', 'DebitCard', 'PayPal', 'CashOnDelivery')),
    PaymentStatus NVARCHAR(20) DEFAULT 'Pending' CHECK (PaymentStatus IN ('Pending', 'Completed', 'Failed', 'Refunded')),
    OrderStatus NVARCHAR(20) DEFAULT 'Processing' CHECK (OrderStatus IN ('Processing', 'Shipped', 'Delivered', 'Cancelled')),
    ShippingAddress NVARCHAR(255),
    ShippingCity NVARCHAR(50),
    ShippingState NVARCHAR(50),
    ShippingZipCode NVARCHAR(10),
    TrackingNumber NVARCHAR(50),
    DeliveryDate DATETIME NULL,
    Notes NVARCHAR(MAX),
    FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID)
);
GO

-- Order Items Table
CREATE TABLE OrderItems (
    OrderItemID INT PRIMARY KEY IDENTITY(1,1),
    OrderID INT NOT NULL,
    ProductID INT NOT NULL,
    SellerID INT NOT NULL,
    Quantity INT NOT NULL CHECK (Quantity > 0),
    UnitPrice DECIMAL(10,2) NOT NULL CHECK (UnitPrice >= 0),
    TotalPrice AS (Quantity * UnitPrice) PERSISTED,
    FOREIGN KEY (OrderID) REFERENCES Orders(OrderID) ON DELETE CASCADE,
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID),
    FOREIGN KEY (SellerID) REFERENCES Sellers(SellerID)
);
GO

-- =============================================
-- PET ADOPTION SECTION
-- =============================================

-- Shelters/Rescue Organizations Table
CREATE TABLE Shelters (
    ShelterID INT PRIMARY KEY IDENTITY(1,1),
    ShelterName NVARCHAR(100) NOT NULL,
    ContactPerson NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    Phone NVARCHAR(20) NOT NULL,
    Address NVARCHAR(255),
    City NVARCHAR(50),
    State NVARCHAR(50),
    ZipCode NVARCHAR(10),
    Website NVARCHAR(255),
    Description NVARCHAR(MAX),
    IsVerified BIT DEFAULT 0,
    CreatedDate DATETIME DEFAULT GETDATE()
);
GO

-- Pets Available for Adoption
CREATE TABLE AdoptionPets (
    PetID INT PRIMARY KEY IDENTITY(1,1),
    ShelterID INT NOT NULL,
    PetName NVARCHAR(50) NOT NULL,
    Species NVARCHAR(30) NOT NULL CHECK (Species IN ('Dog', 'Cat', 'Bird', 'Rabbit', 'Other')),
    Breed NVARCHAR(50),
    Age INT CHECK (Age >= 0),
    Gender NVARCHAR(10) CHECK (Gender IN ('Male', 'Female', 'Unknown')),
    Size NVARCHAR(20) CHECK (Size IN ('Small', 'Medium', 'Large', 'Extra Large')),
    Color NVARCHAR(50),
    Description NVARCHAR(MAX),
    HealthStatus NVARCHAR(MAX),
    Vaccinated BIT DEFAULT 0,
    Neutered BIT DEFAULT 0,
    GoodWithKids BIT DEFAULT 0,
    GoodWithPets BIT DEFAULT 0,
    AdoptionFee DECIMAL(10,2) DEFAULT 0,
    ImageURL NVARCHAR(255),
    Status NVARCHAR(20) DEFAULT 'Available' CHECK (Status IN ('Available', 'Pending', 'Adopted')),
    ListedDate DATETIME DEFAULT GETDATE(),
    AdoptedDate DATETIME NULL,
    FOREIGN KEY (ShelterID) REFERENCES Shelters(ShelterID) ON DELETE CASCADE
);
GO

-- Adoption Applications
CREATE TABLE AdoptionApplications (
    ApplicationID INT PRIMARY KEY IDENTITY(1,1),
    PetID INT NOT NULL,
    CustomerID INT NOT NULL,
    ApplicationDate DATETIME DEFAULT GETDATE(),
    Status NVARCHAR(20) DEFAULT 'Pending' CHECK (Status IN ('Pending', 'Approved', 'Rejected')),
    HomeType NVARCHAR(50),
    HasYard BIT DEFAULT 0,
    HasOtherPets BIT DEFAULT 0,
    Experience NVARCHAR(MAX),
    Reason NVARCHAR(MAX),
    Notes NVARCHAR(MAX),
    ReviewedDate DATETIME NULL,
    ReviewedBy INT NULL,
    FOREIGN KEY (PetID) REFERENCES AdoptionPets(PetID),
    FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID),
    FOREIGN KEY (ReviewedBy) REFERENCES Admins(AdminID)
);
GO

-- =============================================
-- REVIEWS & RATINGS
-- =============================================

-- Product Reviews
CREATE TABLE ProductReviews (
    ReviewID INT PRIMARY KEY IDENTITY(1,1),
    ProductID INT NOT NULL,
    CustomerID INT NOT NULL,
    Rating INT NOT NULL CHECK (Rating BETWEEN 1 AND 5),
    ReviewText NVARCHAR(MAX),
    ReviewDate DATETIME DEFAULT GETDATE(),
    IsVerifiedPurchase BIT DEFAULT 0,
    HelpfulCount INT DEFAULT 0,
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID) ON DELETE CASCADE,
    FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID)
);
GO

-- =============================================
-- BLOG/RESOURCE CENTER
-- =============================================

-- Blog Posts Table
CREATE TABLE BlogPosts (
    PostID INT PRIMARY KEY IDENTITY(1,1),
    AuthorID INT NOT NULL,
    Title NVARCHAR(200) NOT NULL,
    Content NVARCHAR(MAX) NOT NULL,
    Category NVARCHAR(50),
    ImageURL NVARCHAR(255),
    Tags NVARCHAR(255),
    ViewCount INT DEFAULT 0,
    IsPublished BIT DEFAULT 0,
    PublishedDate DATETIME NULL,
    CreatedDate DATETIME DEFAULT GETDATE(),
    UpdatedDate DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (AuthorID) REFERENCES Admins(AdminID)
);
GO

-- =============================================
-- WISHLIST & NOTIFICATIONS
-- =============================================

-- Wishlist Table
CREATE TABLE Wishlist (
    WishlistID INT PRIMARY KEY IDENTITY(1,1),
    CustomerID INT NOT NULL,
    ProductID INT NOT NULL,
    AddedDate DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID) ON DELETE CASCADE,
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID) ON DELETE CASCADE,
    CONSTRAINT UC_CustomerWishlist UNIQUE(CustomerID, ProductID)
);
GO

-- Notifications Table
CREATE TABLE Notifications (
    NotificationID INT PRIMARY KEY IDENTITY(1,1),
    UserID INT NOT NULL,
    NotificationType NVARCHAR(30),
    Title NVARCHAR(100) NOT NULL,
    Message NVARCHAR(MAX) NOT NULL,
    IsRead BIT DEFAULT 0,
    CreatedDate DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (UserID) REFERENCES Users(UserID) ON DELETE CASCADE
);
GO

-- =============================================
-- SYSTEM LOGS & AUDIT
-- =============================================

-- Activity Logs
CREATE TABLE ActivityLogs (
    LogID INT PRIMARY KEY IDENTITY(1,1),
    UserID INT NULL,
    ActionType NVARCHAR(50) NOT NULL,
    ActionDescription NVARCHAR(MAX),
    IPAddress NVARCHAR(50),
    Timestamp DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (UserID) REFERENCES Users(UserID)
);
GO

-- =============================================
-- INDEXES FOR PERFORMANCE
-- =============================================

CREATE INDEX IX_Products_CategoryID ON Products(CategoryID);
CREATE INDEX IX_Products_SellerID ON Products(SellerID);
CREATE INDEX IX_Products_Status ON Products(Status);
CREATE INDEX IX_Orders_CustomerID ON Orders(CustomerID);
CREATE INDEX IX_Orders_OrderDate ON Orders(OrderDate);
CREATE INDEX IX_AdoptionPets_Status ON AdoptionPets(Status);
CREATE INDEX IX_ProductReviews_ProductID ON ProductReviews(ProductID);
GO

-- =============================================
-- STORED PROCEDURES
-- =============================================

-- Register Customer
CREATE PROCEDURE sp_RegisterCustomer
    @Email NVARCHAR(100),
    @PasswordHash NVARCHAR(255),
    @FirstName NVARCHAR(50),
    @LastName NVARCHAR(50),
    @Phone NVARCHAR(20),
    @Address NVARCHAR(255),
    @City NVARCHAR(50),
    @State NVARCHAR(50),
    @ZipCode NVARCHAR(10)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;
    BEGIN TRY
        DECLARE @UserID INT;
        
        INSERT INTO Users (Email, PasswordHash, UserRole)
        VALUES (@Email, @PasswordHash, 'Customer');
        
        SET @UserID = SCOPE_IDENTITY();
        
        INSERT INTO Customers (UserID, FirstName, LastName, Phone, Address, City, State, ZipCode)
        VALUES (@UserID, @FirstName, @LastName, @Phone, @Address, @City, @State, @ZipCode);
        
        COMMIT TRANSACTION;
        SELECT @UserID AS UserID;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

-- Register Seller
CREATE PROCEDURE sp_RegisterSeller
    @Email NVARCHAR(100),
    @PasswordHash NVARCHAR(255),
    @BusinessName NVARCHAR(100),
    @ContactPerson NVARCHAR(100),
    @Phone NVARCHAR(20),
    @Address NVARCHAR(255),
    @City NVARCHAR(50),
    @State NVARCHAR(50),
    @ZipCode NVARCHAR(10),
    @TaxID NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;
    BEGIN TRY
        DECLARE @UserID INT;
        
        INSERT INTO Users (Email, PasswordHash, UserRole)
        VALUES (@Email, @PasswordHash, 'Seller');
        
        SET @UserID = SCOPE_IDENTITY();
        
        INSERT INTO Sellers (UserID, BusinessName, ContactPerson, Phone, Address, City, State, ZipCode, TaxID)
        VALUES (@UserID, @BusinessName, @ContactPerson, @Phone, @Address, @City, @State, @ZipCode, @TaxID);
        
        COMMIT TRANSACTION;
        SELECT @UserID AS UserID;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

-- Add Product to Cart
CREATE PROCEDURE sp_AddToCart
    @CustomerID INT,
    @ProductID INT,
    @Quantity INT
AS
BEGIN
    SET NOCOUNT ON;
    
    IF EXISTS (SELECT 1 FROM ShoppingCart WHERE CustomerID = @CustomerID AND ProductID = @ProductID)
    BEGIN
        UPDATE ShoppingCart 
        SET Quantity = Quantity + @Quantity
        WHERE CustomerID = @CustomerID AND ProductID = @ProductID;
    END
    ELSE
    BEGIN
        INSERT INTO ShoppingCart (CustomerID, ProductID, Quantity)
        VALUES (@CustomerID, @ProductID, @Quantity);
    END
END
GO

-- Create Order from Cart
CREATE PROCEDURE sp_CreateOrder
    @CustomerID INT,
    @PaymentMethod NVARCHAR(30),
    @ShippingAddress NVARCHAR(255),
    @ShippingCity NVARCHAR(50),
    @ShippingState NVARCHAR(50),
    @ShippingZipCode NVARCHAR(10)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;
    BEGIN TRY
        DECLARE @OrderID INT;
        DECLARE @TotalAmount DECIMAL(10,2);
        
        -- Calculate total amount
        SELECT @TotalAmount = SUM(p.Price * sc.Quantity)
        FROM ShoppingCart sc
        INNER JOIN Products p ON sc.ProductID = p.ProductID
        WHERE sc.CustomerID = @CustomerID;
        
        -- Create order
        INSERT INTO Orders (CustomerID, TotalAmount, PaymentMethod, ShippingAddress, ShippingCity, ShippingState, ShippingZipCode)
        VALUES (@CustomerID, @TotalAmount, @PaymentMethod, @ShippingAddress, @ShippingCity, @ShippingState, @ShippingZipCode);
        
        SET @OrderID = SCOPE_IDENTITY();
        
        -- Create order items from cart
        INSERT INTO OrderItems (OrderID, ProductID, SellerID, Quantity, UnitPrice)
        SELECT @OrderID, sc.ProductID, p.SellerID, sc.Quantity, p.Price
        FROM ShoppingCart sc
        INNER JOIN Products p ON sc.ProductID = p.ProductID
        WHERE sc.CustomerID = @CustomerID;
        
        -- Update product stock
        UPDATE p
        SET StockQuantity = p.StockQuantity - sc.Quantity
        FROM Products p
        INNER JOIN ShoppingCart sc ON p.ProductID = sc.ProductID
        WHERE sc.CustomerID = @CustomerID;
        
        -- Clear cart
        DELETE FROM ShoppingCart WHERE CustomerID = @CustomerID;
        
        COMMIT TRANSACTION;
        SELECT @OrderID AS OrderID;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

-- =============================================
-- VIEWS FOR REPORTING
-- =============================================

-- Product Summary View
CREATE VIEW vw_ProductSummary AS
SELECT 
    p.ProductID,
    p.ProductName,
    p.Price,
    p.StockQuantity,
    p.Status,
    c.CategoryName,
    s.BusinessName AS SellerName,
    p.ViewCount,
    ISNULL(AVG(CAST(pr.Rating AS FLOAT)), 0) AS AverageRating,
    COUNT(pr.ReviewID) AS ReviewCount
FROM Products p
INNER JOIN Categories c ON p.CategoryID = c.CategoryID
INNER JOIN Sellers s ON p.SellerID = s.SellerID
LEFT JOIN ProductReviews pr ON p.ProductID = pr.ProductID
GROUP BY p.ProductID, p.ProductName, p.Price, p.StockQuantity, p.Status, c.CategoryName, s.BusinessName, p.ViewCount;
GO

-- Order Summary View
CREATE VIEW vw_OrderSummary AS
SELECT 
    o.OrderID,
    o.OrderDate,
    c.FirstName + ' ' + c.LastName AS CustomerName,
    o.TotalAmount,
    o.PaymentMethod,
    o.PaymentStatus,
    o.OrderStatus,
    COUNT(oi.OrderItemID) AS ItemCount
FROM Orders o
INNER JOIN Customers c ON o.CustomerID = c.CustomerID
LEFT JOIN OrderItems oi ON o.OrderID = oi.OrderID
GROUP BY o.OrderID, o.OrderDate, c.FirstName, c.LastName, o.TotalAmount, o.PaymentMethod, o.PaymentStatus, o.OrderStatus;
GO

-- =============================================
-- INSERT SAMPLE DATA
-- =============================================

-- Insert Admin
INSERT INTO Users (Email, PasswordHash, UserRole) VALUES ('admin@petstore.com', 'AQAAAAEAACcQAAAAEJ7JKqJ8bKxDcKF7p6zXm8GqxH5lGQj4QZ0KqZGqxH5lGQj4QZ0K=', 'Admin');
INSERT INTO Admins (UserID, FullName, Phone) VALUES (SCOPE_IDENTITY(), 'System Administrator', '555-0100');
GO

-- Insert Sample Categories
INSERT INTO Categories (CategoryName, Description) VALUES 
('Dog Food', 'Premium dog food and treats'),
('Cat Food', 'Nutritious cat food and snacks'),
('Dog Toys', 'Fun and interactive toys for dogs'),
('Cat Toys', 'Engaging toys for cats'),
('Pet Accessories', 'Collars, leashes, bowls, and more'),
('Pet Health', 'Vitamins, supplements, and health products'),
('Bird Supplies', 'Food, cages, and accessories for birds'),
('Small Pet Supplies', 'Products for rabbits, hamsters, and guinea pigs');
GO

-- Insert Sample Shelter
INSERT INTO Shelters (ShelterName, ContactPerson, Email, Phone, Address, City, State, ZipCode, IsVerified)
VALUES ('Happy Paws Rescue', 'Jane Smith', 'info@happypaws.org', '555-0200', '123 Rescue Lane', 'Springfield', 'IL', '62701', 1);
GO

PRINT 'Database schema created successfully!';
GO
