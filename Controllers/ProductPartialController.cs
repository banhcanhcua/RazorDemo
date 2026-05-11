using Microsoft.AspNetCore.Mvc;
using RazorDemo.Services;

namespace RazorDemo.Controllers;

[Route("[controller]/[action]")]
public class ProductPartialController : Controller
{
    private readonly ProductService _productService;
    public ProductPartialController(ProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> List(string sortOrder = "asc", int page = 1, int pageSize = 15, string? keyword = null)
    {
        var products = await _productService.SearchProductsAsync(keyword, null, sortOrder);
        var paged = products.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        ViewData["SortOrder"] = sortOrder == "asc" ? "Tăng dần" : "Giảm dần";
        ViewData["SortOrderValue"] = sortOrder;
        ViewData["CurrentPage"] = page;
        ViewData["PageSize"] = pageSize;
        ViewData["Keyword"] = keyword;
        ViewData["HasNextPage"] = page * pageSize < products.Count;
        return PartialView("~/Pages/Components/ProductBox/Default.cshtml", paged);
    }
}
