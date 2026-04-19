using Microsoft.EntityFrameworkCore;
using RazorDemo.Models;

namespace RazorDemo.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Order> Orders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seed data
        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Phone", Description = "Điện thoại" },
            new Category { Id = 2, Name = "Laptop", Description = "Máy tính xách tay" }
        );

        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "Iphone 14 Pro", Description = "Điện thoại của Apple", Price = 1000, Stock = 10, CategoryId = 1, Images = "[\"/images/products/sample1.jpg\"]" },
            new Product { Id = 2, Name = "Samsung Galaxy", Description = "Điện thoại của Samsung", Price = 500, Stock = 5, CategoryId = 1, Images = "[\"/images/products/sample2.jpg\"]" },
            new Product { Id = 3, Name = "Dell XPS", Description = "Laptop của Dell", Price = 1200, Stock = 3, CategoryId = 2, Images = "[\"/images/products/sample3.jpg\"]" },
            new Product { Id = 11, Name = "Sản phẩm 11", Description = "Mô tả sản phẩm 11", Price = 11000, Stock = 100, CategoryId = 1, Images = "[]" },
            new Product { Id = 12, Name = "Sản phẩm 12", Description = "Mô tả sản phẩm 12", Price = 12000, Stock = 100, CategoryId = 1, Images = "[]" },
            new Product { Id = 13, Name = "Sản phẩm 13", Description = "Mô tả sản phẩm 13", Price = 13000, Stock = 100, CategoryId = 1, Images = "[]" },
            new Product { Id = 14, Name = "Sản phẩm 14", Description = "Mô tả sản phẩm 14", Price = 14000, Stock = 100, CategoryId = 1, Images = "[]" },
            new Product { Id = 15, Name = "Sản phẩm 15", Description = "Mô tả sản phẩm 15", Price = 15000, Stock = 100, CategoryId = 1, Images = "[]" },
            new Product { Id = 16, Name = "Sản phẩm 16", Description = "Mô tả sản phẩm 16", Price = 16000, Stock = 100, CategoryId = 1, Images = "[]" },
            new Product { Id = 17, Name = "Sản phẩm 17", Description = "Mô tả sản phẩm 17", Price = 17000, Stock = 100, CategoryId = 1, Images = "[]" },
            new Product { Id = 18, Name = "Sản phẩm 18", Description = "Mô tả sản phẩm 18", Price = 18000, Stock = 100, CategoryId = 1, Images = "[]" },
            new Product { Id = 19, Name = "Sản phẩm 19", Description = "Mô tả sản phẩm 19", Price = 19000, Stock = 100, CategoryId = 1, Images = "[]" },
            new Product { Id = 20, Name = "Sản phẩm 20", Description = "Mô tả sản phẩm 20", Price = 20000, Stock = 100, CategoryId = 1, Images = "[]" },
            new Product { Id = 21, Name = "Sản phẩm 21", Description = "Mô tả sản phẩm 21", Price = 21000, Stock = 100, CategoryId = 1, Images = "[]" },
            new Product { Id = 22, Name = "Sản phẩm 22", Description = "Mô tả sản phẩm 22", Price = 22000, Stock = 100, CategoryId = 1, Images = "[]" },
            new Product { Id = 23, Name = "Sản phẩm 23", Description = "Mô tả sản phẩm 23", Price = 23000, Stock = 100, CategoryId = 1, Images = "[]" },
            new Product { Id = 24, Name = "Sản phẩm 24", Description = "Mô tả sản phẩm 24", Price = 24000, Stock = 100, CategoryId = 1, Images = "[]" },
            new Product { Id = 25, Name = "Sản phẩm 25", Description = "Mô tả sản phẩm 25", Price = 25000, Stock = 100, CategoryId = 1, Images = "[]" },
            new Product { Id = 26, Name = "Sản phẩm 26", Description = "Mô tả sản phẩm 26", Price = 26000, Stock = 100, CategoryId = 1, Images = "[]" },
            new Product { Id = 27, Name = "Sản phẩm 27", Description = "Mô tả sản phẩm 27", Price = 27000, Stock = 100, CategoryId = 1, Images = "[]" },
            new Product { Id = 28, Name = "Sản phẩm 28", Description = "Mô tả sản phẩm 28", Price = 28000, Stock = 100, CategoryId = 1, Images = "[]" },
            new Product { Id = 29, Name = "Sản phẩm 29", Description = "Mô tả sản phẩm 29", Price = 29000, Stock = 100, CategoryId = 1, Images = "[]" },
            new Product { Id = 30, Name = "Sản phẩm 30", Description = "Mô tả sản phẩm 30", Price = 30000, Stock = 100, CategoryId = 1, Images = "[]" },
            new Product { Id = 31, Name = "Sản phẩm 31", Description = "Mô tả sản phẩm 31", Price = 31000, Stock = 100, CategoryId = 1, Images = "[]" },
            new Product { Id = 32, Name = "Sản phẩm 32", Description = "Mô tả sản phẩm 32", Price = 32000, Stock = 100, CategoryId = 1, Images = "[]" },
            new Product { Id = 33, Name = "Sản phẩm 33", Description = "Mô tả sản phẩm 33", Price = 33000, Stock = 100, CategoryId = 1, Images = "[]" },
            new Product { Id = 34, Name = "Sản phẩm 34", Description = "Mô tả sản phẩm 34", Price = 34000, Stock = 100, CategoryId = 1, Images = "[]" },
            new Product { Id = 35, Name = "Sản phẩm 35", Description = "Mô tả sản phẩm 35", Price = 35000, Stock = 100, CategoryId = 1, Images = "[]" },
            new Product { Id = 36, Name = "Sản phẩm 36", Description = "Mô tả sản phẩm 36", Price = 36000, Stock = 100, CategoryId = 1, Images = "[]" },
            new Product { Id = 37, Name = "Sản phẩm 37", Description = "Mô tả sản phẩm 37", Price = 37000, Stock = 100, CategoryId = 1, Images = "[]" },
            new Product { Id = 38, Name = "Sản phẩm 38", Description = "Mô tả sản phẩm 38", Price = 38000, Stock = 100, CategoryId = 1, Images = "[]" },
            new Product { Id = 39, Name = "Sản phẩm 39", Description = "Mô tả sản phẩm 39", Price = 39000, Stock = 100, CategoryId = 1, Images = "[]" },
            new Product { Id = 40, Name = "Sản phẩm 40", Description = "Mô tả sản phẩm 40", Price = 40000, Stock = 100, CategoryId = 1, Images = "[]" }
        );

        modelBuilder.Entity<User>().HasData(
            new User { Id = 1, Username = "admin", Email = "admin@example.com", Password = "admin123", Role = "admin" },
            new User { Id = 2, Username = "customer", Email = "customer@example.com", Password = "customer123", Role = "customer" }
        );
    }
}