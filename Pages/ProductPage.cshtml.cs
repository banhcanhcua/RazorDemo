using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorDemo.Models;
using RazorDemo.Services;

namespace RazorDemo.Pages;

public class ProductPageModel : PageModel
{
    private readonly ProductService _productService;
    private readonly ProductImageStorageService _productImageStorageService;

    public ProductPageModel(ProductService productService, ProductImageStorageService productImageStorageService)
    {
        _productService = productService;
        _productImageStorageService = productImageStorageService;
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

    public async Task OnGetAsync(int? id)
    {
        int page = 1;
        if (Request.Query.ContainsKey("page"))
        {
            int.TryParse(Request.Query["page"], out page);
            if (page < 1) page = 1;
        }
        string sortOrder = Request.Query.ContainsKey("SortOrder") ? Request.Query["SortOrder"].ToString() : "asc";
        var allProducts = await _productService.SearchProductsAsync(SearchKeyword, CategoryId, sortOrder);
        int totalProducts = allProducts.Count;
        Products = allProducts.Skip((page - 1) * PageSize).Take(PageSize).ToList();
        CurrentPage = page;
        TotalPages = (int)Math.Ceiling((double)totalProducts / PageSize);
        Categories = await _productService.GetCategoriesAsync();

        if (id is not null)
        {
            ProductId = id.Value;
            Product = await _productService.GetProductByIdAsync(id.Value);
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

    public async Task<IActionResult> OnGetLastProductAsync()
    {
        var products = await _productService.GetProductsAsync();
        Product = products.LastOrDefault();
        if (Product is null)
        {
            return NotFound();
        }
        Products = products;
        Categories = await _productService.GetCategoriesAsync();
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

    public async Task<IActionResult> OnGetRemoveAllAsync()
    {
        await _productService.DeleteAllAsync();
        return RedirectToPage("ProductPage");
    }

    public async Task<IActionResult> OnPostCreateAsync([Bind(Prefix = "NewProduct")] Product newProduct)
    {
        if (!TryValidateModel(newProduct, "NewProduct"))
        {
            Products = await _productService.SearchProductsAsync(SearchKeyword, CategoryId);
            Categories = await _productService.GetCategoriesAsync();
            NewProduct = newProduct;
            return Page();
        }

        try
        {
            newProduct.ImageUrls = await _productImageStorageService.SaveUploadedImagesAsync(UploadedImages, HttpContext.RequestAborted);
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(nameof(UploadedImages), ex.Message);
            Products = await _productService.SearchProductsAsync(SearchKeyword, CategoryId);
            Categories = await _productService.GetCategoriesAsync();
            NewProduct = newProduct;
            return Page();
        }

        await _productService.AddProductAsync(newProduct);
        TempData["SuccessMessage"] = "Thêm sản phẩm thành công.";
        return RedirectToPage("ProductPage");
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        await _productService.DeleteProductAsync(id);
        return RedirectToPage("ProductPage");
    }

    public async Task<IActionResult> OnPostUpdateAsync([Bind(Prefix = "ProductUpdate")] Product productUpdate)
    {
        ProductId = productUpdate.Id;
        ProductUpdate = productUpdate;

        if (!TryValidateModel(productUpdate, "ProductUpdate"))
        {
            Products = await _productService.SearchProductsAsync(SearchKeyword, CategoryId);
            Categories = await _productService.GetCategoriesAsync();
            Product = await _productService.GetProductByIdAsync(ProductId);
            return Page();
        }

        // Nếu có upload ảnh mới thì lưu và cập nhật danh sách ảnh
        List<string> newImages;
        try
        {
            newImages = await _productImageStorageService.SaveUploadedImagesAsync(UploadedImages, HttpContext.RequestAborted);
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(nameof(UploadedImages), ex.Message);
            Products = await _productService.SearchProductsAsync(SearchKeyword, CategoryId);
            Categories = await _productService.GetCategoriesAsync();
            Product = await _productService.GetProductByIdAsync(ProductId);
            return Page();
        }

        if (newImages.Any())
        {
            var currentProduct = await _productService.GetProductByIdAsync(productUpdate.Id);
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

        await _productService.UpdateProductAsync(productUpdate);

        TempData["SuccessMessage"] = "Cập nhật sản phẩm thành công.";
        return RedirectToPage("ProductPage");
    }
}
