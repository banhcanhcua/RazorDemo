using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorDemo.Models;

namespace RazorDemo.Pages;

public class ContactPageModel : PageModel
{
    [BindProperty]
    public Contact contact { get; set; } = new Contact();

    public string thongbao { get; set; } = string.Empty;

    public void OnGet()
    {
    }

    public void OnPost()
    {
        if (ModelState.IsValid)
        {
            thongbao = "Dữ liệu gửi đến hợp lệ";
        }
        else
        {
            thongbao = "Dữ liệu không hợp lệ";
        }
    }
}
