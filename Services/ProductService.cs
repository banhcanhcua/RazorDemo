using Microsoft.EntityFrameworkCore;
using RazorDemo.Data;
using RazorDemo.Models;

namespace RazorDemo.Services;

public class ProductService
{
    private readonly ApplicationDbContext _context;

    public ProductService(ApplicationDbContext context)
    {
        _context = context;
    }

    public List<Product> GetProducts(int page = 1, int pageSize = 15)
    {
        if (page < 1) page = 1;
        return _context.Products.Include(p => p.Category)
            .OrderBy(p => p.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();
    }

    public Product? GetProductById(int id) => _context.Products.Include(p => p.Category).FirstOrDefault(p => p.Id == id);

    public List<Product> SearchProducts(string? keyword, int? categoryId)
    {
        var query = _context.Products.Include(p => p.Category).AsQueryable();

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            query = query.Where(p => p.Name.Contains(keyword) || p.Id.ToString() == keyword);
        }

        if (categoryId.HasValue)
        {
            query = query.Where(p => p.CategoryId == categoryId.Value);
        }

        return query.ToList();
    }

    public void AddProduct(Product product)
    {
        ArgumentNullException.ThrowIfNull(product);
        _context.Products.Add(product);
        _context.SaveChanges();
    }

    public bool UpdateProduct(Product product)
    {
        ArgumentNullException.ThrowIfNull(product);
        var existing = GetProductById(product.Id);
        if (existing is null)
        {
            return false;
        }

        existing.Name = product.Name;
        existing.Description = product.Description;
        existing.Price = product.Price;
        existing.Stock = product.Stock;
        existing.CategoryId = product.CategoryId;
        existing.ImageUrls = product.ImageUrls;
        _context.SaveChanges();
        return true;
    }

    public bool DeleteProduct(int id)
    {
        var existing = GetProductById(id);
        if (existing is null)
        {
            return false;
        }

        _context.Products.Remove(existing);
        _context.SaveChanges();
        return true;
    }

    public void DeleteAll()
    {
        _context.Products.RemoveRange(_context.Products);
        _context.SaveChanges();
    }

    public List<Category> GetCategories() => _context.Categories.ToList();
}
