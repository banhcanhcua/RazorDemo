using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RazorDemo.Models;
using RazorDemo.Services;
using System.Threading.Tasks;

namespace RazorDemo.Controllers;

public class ProductController : Controller
{
    private readonly ProductService _productService;
    private readonly ProductImageStorageService _productImageStorageService;

    public ProductController(ProductService productService, ProductImageStorageService productImageStorageService)
    {
        _productService = productService;
        _productImageStorageService = productImageStorageService;
    }

    private bool IsAuthenticated => HttpContext.Session.GetString("UserId") != null;
    private string? UserRole => HttpContext.Session.GetString("Role");

    // GET: Product
    public async Task<IActionResult> Index(string? searchKeyword, int? categoryId)
    {
        if (!IsAuthenticated)
        {
            return RedirectToPage("/Login");
        }
        if (UserRole != "admin")
        {
            return Forbid();
        }

        ViewData["SearchKeyword"] = searchKeyword;
        ViewData["CategoryId"] = categoryId;
        ViewData["Categories"] = await _productService.GetCategoriesAsync();

        int pageSize = 15;
        int page = 1;
        if (Request.Query.ContainsKey("page"))
        {
            int.TryParse(Request.Query["page"], out page);
            if (page < 1) page = 1;
        }
        var products = await _productService.SearchProductsAsync(searchKeyword, categoryId);
        int totalProducts = products.Count;
        var pagedProducts = products.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        ViewData["CurrentPage"] = page;
        ViewData["TotalPages"] = (int)Math.Ceiling((double)totalProducts / pageSize);
        return View(pagedProducts);
    }

    // GET: Product/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (!IsAuthenticated)
        {
            return RedirectToPage("/Login");
        }

        if (id == null)
        {
            return NotFound();
        }

        var product = await _productService.GetProductByIdAsync(id.Value);
        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }

    // GET: Product/Create
    public async Task<IActionResult> Create()
    {
        if (!IsAuthenticated || UserRole != "admin")
        {
            return RedirectToPage("/Login");
        }

        ViewData["Categories"] = await _productService.GetCategoriesAsync();
        return View();
    }

    // POST: Product/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Name,Description,Price,Stock,CategoryId")] Product product, List<IFormFile> uploadedImages)
    {
        if (!IsAuthenticated || UserRole != "admin")
        {
            return RedirectToPage("/Login");
        }

        if (ModelState.IsValid)
        {
            try
            {
                product.ImageUrls = await _productImageStorageService.SaveUploadedImagesAsync(uploadedImages, HttpContext.RequestAborted);
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("uploadedImages", ex.Message);
            }
        }

        if (ModelState.IsValid)
        {
            await _productService.AddProductAsync(product);
            TempData["SuccessMessage"] = "Thêm sản phẩm thành công.";
            return RedirectToAction(nameof(Index));
        }
        ViewData["Categories"] = await _productService.GetCategoriesAsync();
        return View(product);
    }

    // GET: Product/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (!IsAuthenticated || UserRole != "admin")
        {
            return RedirectToPage("/Login");
        }

        if (id == null)
        {
            return NotFound();
        }

        var product = await _productService.GetProductByIdAsync(id.Value);
        if (product == null)
        {
            return NotFound();
        }
        ViewData["Categories"] = await _productService.GetCategoriesAsync();
        return View(product);
    }

    // POST: Product/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Price,Stock,CategoryId")] Product product)
    {
        if (!IsAuthenticated || UserRole != "admin")
        {
            return RedirectToPage("/Login");
        }

        if (id != product.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            await _productService.UpdateProductAsync(product);
            TempData["SuccessMessage"] = "Cập nhật sản phẩm thành công.";
            return RedirectToAction(nameof(Index));
        }
        ViewData["Categories"] = await _productService.GetCategoriesAsync();
        return View(product);
    }

    // GET: Product/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (!IsAuthenticated || UserRole != "admin")
        {
            return RedirectToPage("/Login");
        }

        if (id == null)
        {
            return NotFound();
        }

        var product = await _productService.GetProductByIdAsync(id.Value);
        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }

    // POST: Product/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (!IsAuthenticated || UserRole != "admin")
        {
            return RedirectToPage("/Login");
        }

        await _productService.DeleteProductAsync(id);
        return RedirectToAction(nameof(Index));
    }

    // POST: Product/RemoveAll
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RemoveAll()
    {
        if (!IsAuthenticated || UserRole != "admin")
        {
            return RedirectToPage("/Login");
        }

        await _productService.DeleteAllAsync();
        return RedirectToAction(nameof(Index));
    }
}