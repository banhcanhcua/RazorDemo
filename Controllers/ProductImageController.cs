using Microsoft.AspNetCore.Mvc;
using RazorDemo.Services;

namespace RazorDemo.Controllers;

[Route("[controller]/[action]")]
public class ProductImageController : Controller
{
    private readonly ProductService _productService;
    public ProductImageController(ProductService productService)
    {
        _productService = productService;
    }

    [HttpPost]
    public async Task<IActionResult> DeleteImage(int productId, string imageUrl)
    {
        var result = await _productService.DeleteProductImageAsync(productId, imageUrl);
        return Json(new { success = result });
    }

    [HttpPost]
    public async Task<IActionResult> DeleteAllImages(int productId)
    {
        var result = await _productService.DeleteAllProductImagesAsync(productId);
        return Json(new { success = result });
    }
}
