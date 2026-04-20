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

    public async Task<List<Product>> GetProductsAsync(string sortOrder = "asc")
        => await GetProductsQueryable(sortOrder: sortOrder).ToListAsync();

    public async Task<Product?> GetProductByIdAsync(int id)
        => await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);

    public async Task<bool> DeleteProductImageAsync(int productId, string imageUrl)
    {
        Console.WriteLine($"[DeleteProductImageAsync] productId={productId}, imageUrl={imageUrl}");
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);
        if (product == null)
        {
            Console.WriteLine($"[DeleteProductImageAsync] Không tìm thấy sản phẩm với Id={productId}");
            return false;
        }
        var urls = product.ImageUrls;
        Console.WriteLine($"[DeleteProductImageAsync] Danh sách ảnh hiện tại: {string.Join(", ", urls)}");
        var toRemove = urls.FirstOrDefault(u => u == imageUrl || u.EndsWith(imageUrl, StringComparison.OrdinalIgnoreCase));
        Console.WriteLine($"[DeleteProductImageAsync] Ảnh tìm thấy để xóa: {toRemove}");
        if (toRemove != null)
        {
            urls.Remove(toRemove);
            product.ImageUrls = urls; // cập nhật lại chuỗi Images
            await _context.SaveChangesAsync();
            var wwwroot = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var filePath = wwwroot + toRemove.Replace("/", Path.DirectorySeparatorChar.ToString());
            if (System.IO.File.Exists(filePath))
                System.IO.File.Delete(filePath);
            Console.WriteLine($"[DeleteProductImageAsync] Đã xóa thành công ảnh: {toRemove}");
            return true;
        }
        Console.WriteLine($"[DeleteProductImageAsync] Không tìm thấy ảnh phù hợp để xóa!");
        return false;
    }

    public async Task<bool> DeleteAllProductImagesAsync(int productId)
    {
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);
        if (product == null) return false;
        var urls = product.ImageUrls.ToList();
        bool any = false;
        var wwwroot = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        foreach (var imageUrl in urls)
        {
            var filePath = wwwroot + imageUrl.Replace("/", Path.DirectorySeparatorChar.ToString());
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
                any = true;
            }
        }
        product.ImageUrls = new List<string>();
        await _context.SaveChangesAsync();
        return any;
    }

    public async Task<List<Product>> SearchProductsAsync(string? keyword = null, int? categoryId = null, string sortOrder = "asc")
        => await GetProductsQueryable(keyword, categoryId, sortOrder).ToListAsync();

    private IQueryable<Product> GetProductsQueryable(string? keyword = null, int? categoryId = null, string sortOrder = "asc")
    {
        var query = _context.Products.Include(p => p.Category).AsQueryable();
        if (!string.IsNullOrWhiteSpace(keyword))
            query = query.Where(p => p.Name.Contains(keyword) || p.Id.ToString() == keyword);
        if (categoryId.HasValue)
            query = query.Where(p => p.CategoryId == categoryId.Value);
        query = sortOrder == "desc"
            ? query.OrderByDescending(p => (double)p.Price)
            : query.OrderBy(p => (double)p.Price);
        return query;
    }

    public async Task AddProductAsync(Product product)
    {
        ArgumentNullException.ThrowIfNull(product);
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateProductAsync(Product product)
    {
        ArgumentNullException.ThrowIfNull(product);
        var existing = await _context.Products.FirstOrDefaultAsync(p => p.Id == product.Id);
        if (existing is null)
            throw new InvalidOperationException($"Product with Id={product.Id} not found.");

        existing.Name = product.Name;
        existing.Description = product.Description;
        existing.Price = product.Price;
        existing.Stock = product.Stock;
        existing.CategoryId = product.CategoryId;
        existing.ImageUrls = product.ImageUrls;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteProductAsync(int id)
    {
        var existing = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
        if (existing is null)
            throw new InvalidOperationException($"Product with Id={id} not found.");
        _context.Products.Remove(existing);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAllAsync()
    {
        _context.Products.RemoveRange(_context.Products);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Category>> GetCategoriesAsync()
        => await _context.Categories.ToListAsync();
}
