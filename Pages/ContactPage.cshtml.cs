using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorDemo.Data;
using RazorDemo.Models;

namespace RazorDemo.Pages;

public class ContactPageModel : PageModel
{
    private const int ContactPageSize = 5;
    private readonly ApplicationDbContext _context;

    public ContactPageModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Contact contact { get; set; } = new Contact();

    public string thongbao { get; set; } = string.Empty;

    public bool IsSuccess { get; set; }

    public bool IsAdmin { get; private set; }

    [BindProperty(SupportsGet = true)]
    public bool ShowContacts { get; set; }

    [BindProperty(SupportsGet = true)]
    public int ContactPage { get; set; } = 1;

    public List<Contact> SubmittedContacts { get; private set; } = new();

    public int TotalContactPages { get; private set; } = 1;

    public async Task OnGetAsync()
    {
        await LoadContactStateAsync();
    }

    public async Task OnPostAsync()
    {
        await LoadContactStateAsync();

        if (ModelState.IsValid)
        {
            contact.CreatedAt = DateTime.Now;
            _context.Contacts.Add(contact);
            await _context.SaveChangesAsync();
            thongbao = "Gửi thông tin liên hệ thành công.";
            IsSuccess = true;

            if (IsAdmin && ShowContacts)
            {
                ContactPage = 1;
                await LoadSubmittedContactsAsync();
            }
        }
        else
        {
            thongbao = "Dữ liệu không hợp lệ";
        }
    }

    private async Task LoadContactStateAsync()
    {
        IsAdmin = HttpContext.Session.GetString("Role") == "admin";
        if (ContactPage < 1)
        {
            ContactPage = 1;
        }

        if (IsAdmin && ShowContacts)
        {
            await LoadSubmittedContactsAsync();
        }
    }

    private async Task LoadSubmittedContactsAsync()
    {
        var query = _context.Contacts.OrderByDescending(item => item.CreatedAt).ThenByDescending(item => item.ContactId);
        var totalContacts = await query.CountAsync();
        TotalContactPages = Math.Max(1, (int)Math.Ceiling(totalContacts / (double)ContactPageSize));
        if (ContactPage > TotalContactPages)
        {
            ContactPage = TotalContactPages;
        }

        SubmittedContacts = await query
            .Skip((ContactPage - 1) * ContactPageSize)
            .Take(ContactPageSize)
            .ToListAsync();
    }
}
