using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorDemo.Models;
using RazorDemo.Services;

namespace RazorDemo.Pages;

public class ProductPageModel : PageModel
{
    private readonly ProductService _productService;

    public ProductPageModel(ProductService productService)
    {
        _productService = productService;
    }

    [BindProperty(SupportsGet = true)]
    public string? SearchKeyword { get; set; }

    [BindProperty(SupportsGet = true)]
    public int? CategoryId { get; set; }

    public List<Product> Products { get; set; } = new();
    public int CurrentPage { get; set; } = 1;
    public int PageSize { get; set; } = 15;
    public int TotalPages { get; set; } = 1;
    public Product? Product { get; set; }

    public Product ProductUpdate { get; set; } = new();

    public Product NewProduct { get; set; } = new();

    [BindProperty]
    public List<IFormFile> UploadedImages { get; set; } = new();

    [BindProperty]
    public int ProductId { get; set; }

    public List<Category> Categories { get; set; } = new();

    public string PageTitle { get; set; } = "Danh sách sản phẩm";

    public void OnGet(int? id)
    {
        int page = 1;
        if (Request.Query.ContainsKey("page"))
        {
            int.TryParse(Request.Query["page"], out page);
            if (page < 1) page = 1;
        }
        var allProducts = _productService.SearchProducts(SearchKeyword, CategoryId);
        int totalProducts = allProducts.Count;
        Products = allProducts.Skip((page - 1) * PageSize).Take(PageSize).ToList();
        CurrentPage = page;
        TotalPages = (int)Math.Ceiling((double)totalProducts / PageSize);
        Categories = _productService.GetCategories();

        if (id is not null)
        {
            ProductId = id.Value;
            Product = _productService.GetProductById(id.Value);
            PageTitle = Product is not null
                ? $"Thông tin sản phẩm (ID={id.Value})"
                : "Sản phẩm không tồn tại";

            if (Product is not null)
            {
                ProductUpdate = new Product
                {
                    Id = Product.Id,
                    Name = Product.Name,
                    Description = Product.Description,
                    Price = Product.Price,
                    Stock = Product.Stock,
                    CategoryId = Product.CategoryId,
                    ImageUrls = Product.ImageUrls
                };
            }
        }
        else
        {
            PageTitle = string.IsNullOrWhiteSpace(SearchKeyword) && !CategoryId.HasValue
                ? "Danh sách sản phẩm"
                : "Kết quả tìm kiếm";
        }
    }

    public IActionResult OnGetLastProduct()
    {
        Product = _productService.GetProducts().LastOrDefault();
        if (Product is null)
        {
            return NotFound();
        }

        Products = _productService.GetProducts();
        Categories = _productService.GetCategories();
        PageTitle = "Sản phẩm cuối";
        ProductUpdate = new Product
        {
            Id = Product.Id,
            Name = Product.Name,
            Description = Product.Description,
            Price = Product.Price,
            Stock = Product.Stock,
            CategoryId = Product.CategoryId,
            ImageUrls = Product.ImageUrls
        };
        return Page();
    }

    public IActionResult OnGetRemoveAll()
    {
        _productService.DeleteAll();
        return RedirectToPage("ProductPage");
    }

    public async Task<IActionResult> OnPostCreate([Bind(Prefix = "NewProduct")] Product newProduct)
    {
        if (!TryValidateModel(newProduct, "NewProduct"))
        {
            Products = _productService.SearchProducts(SearchKeyword, CategoryId);
            Categories = _productService.GetCategories();
            NewProduct = newProduct;
            return Page();
        }

        newProduct.ImageUrls = await SaveUploadedImagesAsync();

        _productService.AddProduct(newProduct);
        TempData["SuccessMessage"] = "Thêm sản phẩm thành công.";
        return RedirectToPage("ProductPage");
    }

    private async Task<List<string>> SaveUploadedImagesAsync()
    {
        var imageUrls = new List<string>();
        if (UploadedImages is null || UploadedImages.Count == 0)
        {
            return imageUrls;
        }

        var uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "products");
        Directory.CreateDirectory(uploadDirectory);

        foreach (var file in UploadedImages)
        {
            if (file is null || file.Length == 0)
            {
                continue;
            }

            var extension = Path.GetExtension(file.FileName);
            var fileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(uploadDirectory, fileName);

            await using var stream = System.IO.File.Create(filePath);
            await file.CopyToAsync(stream);

            imageUrls.Add($"/images/products/{fileName}");
        }

        return imageUrls;
    }

    public IActionResult OnPostDelete(int id)
    {
        _productService.DeleteProduct(id);
        return RedirectToPage("ProductPage");
    }

    public async Task<IActionResult> OnPostUpdate([Bind(Prefix = "ProductUpdate")] Product productUpdate)
    {
        ProductId = productUpdate.Id;
        ProductUpdate = productUpdate;

        if (!TryValidateModel(productUpdate, "ProductUpdate"))
        {
            Products = _productService.SearchProducts(SearchKeyword, CategoryId);
            Categories = _productService.GetCategories();
            Product = _productService.GetProductById(ProductId);
            return Page();
        }

        // Nếu có upload ảnh mới thì lưu và cập nhật danh sách ảnh
        var newImages = await SaveUploadedImagesAsync();
        if (newImages.Any())
        {
            var currentProduct = _productService.GetProductById(productUpdate.Id);
            if (currentProduct != null)
            {
                var allImages = currentProduct.ImageUrls.Concat(newImages).ToList();
                productUpdate.ImageUrls = allImages;
            }
            else
            {
                productUpdate.ImageUrls = newImages;
            }
        }

        if (!_productService.UpdateProduct(productUpdate))
        {
            ModelState.AddModelError(string.Empty, "Không thể cập nhật sản phẩm. Sản phẩm không tồn tại.");
            Products = _productService.SearchProducts(SearchKeyword, CategoryId);
            Categories = _productService.GetCategories();
            Product = _productService.GetProductById(ProductId);
            return Page();
        }

        TempData["SuccessMessage"] = "Cập nhật sản phẩm thành công.";
        return RedirectToPage("ProductPage");
    }
}
