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

    public IViewComponentResult Invoke(bool sapxepTang = true, int page = 1, int pageSize = 15)
    {
        var products = _productService.GetProducts(page, pageSize);
        var sortedProducts = sapxepTang
            ? products.OrderBy(p => p.Price).ToList()
            : products.OrderByDescending(p => p.Price).ToList();

        ViewData["SortOrder"] = sapxepTang ? "Tăng dần" : "Giảm dần";
        ViewData["CurrentPage"] = page;
        ViewData["PageSize"] = pageSize;
        return View(sortedProducts);
    }
}
