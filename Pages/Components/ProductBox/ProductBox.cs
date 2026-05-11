using Microsoft.AspNetCore.Mvc;
using RazorDemo.Services;

namespace RazorDemo.Pages.Components.ProductBox;

public class ProductBox : ViewComponent
{
    private readonly ProductService _productService;

    public ProductBox(ProductService productService)
    {
        _productService = productService;
    }

    public async Task<IViewComponentResult> InvokeAsync(bool sapxepTang = true, int page = 1, int pageSize = 15, string? keyword = null)
    {
        string sortOrder = sapxepTang ? "asc" : "desc";
        var products = await _productService.SearchProductsAsync(keyword, null, sortOrder);
        var paged = products.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        ViewData["SortOrder"] = sapxepTang ? "Tăng dần" : "Giảm dần";
        ViewData["SortOrderValue"] = sortOrder;
        ViewData["CurrentPage"] = page;
        ViewData["PageSize"] = pageSize;
        ViewData["Keyword"] = keyword;
        ViewData["HasNextPage"] = page * pageSize < products.Count;
        return View(paged);
    }
}
