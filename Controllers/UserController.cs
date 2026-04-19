using Microsoft.AspNetCore.Mvc;
using RazorDemo.Models;
using RazorDemo.Services;
using System.Threading.Tasks;

namespace RazorDemo.Controllers;

public class UserController : Controller
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    private bool IsAuthenticated => HttpContext.Session.GetString("UserId") != null;
    private string? UserRole => HttpContext.Session.GetString("Role");

    // GET: User
    public async Task<IActionResult> Index()
    {
        if (!IsAuthenticated || UserRole != "admin")
        {
            return RedirectToPage("/Login");
        }

        var users = await _userService.GetAllUsersAsync();
        return View(users);
    }

    // GET: User/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (!IsAuthenticated || UserRole != "admin")
        {
            return RedirectToPage("/Login");
        }

        if (id == null)
        {
            return NotFound();
        }

        var user = await _userService.GetUserByIdAsync(id.Value);
        if (user == null)
        {
            return NotFound();
        }

        return View(user);
    }

    // GET: User/Create
    public IActionResult Create()
    {
        if (!IsAuthenticated || UserRole != "admin")
        {
            return RedirectToPage("/Login");
        }

        return View();
    }

    // POST: User/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Username,Email,Password,Role")] User user)
    {
        if (!IsAuthenticated || UserRole != "admin")
        {
            return RedirectToPage("/Login");
        }

        if (ModelState.IsValid)
        {
            var success = await _userService.CreateUserAsync(user);
            if (success)
            {
                TempData["SuccessMessage"] = "Thêm người dùng thành công.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("", "Tên đăng nhập hoặc email đã tồn tại.");
            }
        }
        return View(user);
    }

    // GET: User/Edit/5
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

        var user = await _userService.GetUserByIdAsync(id.Value);
        if (user == null)
        {
            return NotFound();
        }
        return View(user);
    }

    // POST: User/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Username,Email,Password,Role")] User user)
    {
        if (!IsAuthenticated || UserRole != "admin")
        {
            return RedirectToPage("/Login");
        }

        if (id != user.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            var success = await _userService.UpdateUserAsync(user);
            if (success)
            {
                TempData["SuccessMessage"] = "Cập nhật người dùng thành công.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("", "Tên đăng nhập hoặc email đã tồn tại.");
            }
        }
        return View(user);
    }

    // GET: User/Delete/5
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

        var user = await _userService.GetUserByIdAsync(id.Value);
        if (user == null)
        {
            return NotFound();
        }

        return View(user);
    }

    // POST: User/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (!IsAuthenticated || UserRole != "admin")
        {
            return RedirectToPage("/Login");
        }

        var success = await _userService.DeleteUserAsync(id);
        if (success)
        {
            TempData["SuccessMessage"] = "Xóa người dùng thành công.";
        }
        return RedirectToAction(nameof(Index));
    }
}