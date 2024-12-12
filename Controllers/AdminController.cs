using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TicketingSystem.Models;
using TicketingSystem.ViewModels;
using Microsoft.EntityFrameworkCore;

public class AdminController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AdminController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    // GET: EditUser
    [HttpGet]
    public async Task<IActionResult> EditUser(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        // دریافت تمام نقش‌ها از Enum
        var allRoles = Enum.GetValues(typeof(UserRole)).Cast<UserRole>().ToList();

        // گرفتن نقش‌های فعلی کاربر
        var userRoles = await _userManager.GetRolesAsync(user);

        // ایجاد ViewModel برای نمایش در ویو
        var userViewModel = new UserViewModel
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            // تبدیل Enum به لیست رشته‌ای برای نمایش در لیست کشویی
            Role = allRoles.FirstOrDefault(role => userRoles.Contains(role.ToString())),
        };

        return View(userViewModel);
    }

    // POST: EditUser
    [HttpPost]
    public async Task<IActionResult> EditUser(UserViewModel userViewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(userViewModel);
        }

        var user = await _userManager.FindByIdAsync(userViewModel.Id);
        if (user == null)
        {
            return NotFound();
        }

        // به‌روزرسانی اطلاعات کاربر
        user.Email = userViewModel.Email;
        user.FirstName = userViewModel.FirstName;
        user.LastName = userViewModel.LastName;

        // به‌روزرسانی نقش‌ها
        var selectedRole = userViewModel.Role.ToString(); // تبدیل انتخاب به رشته برای ذخیره نقش
        var currentRoles = await _userManager.GetRolesAsync(user);
        await _userManager.RemoveFromRolesAsync(user, currentRoles);
        await _userManager.AddToRoleAsync(user, selectedRole);

        var result = await _userManager.UpdateAsync(user);
        if (result.Succeeded)
        {
            TempData["Message"] = "کاربر با موفقیت ویرایش شد.";
            return RedirectToAction("Index");
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError("", error.Description);
        }

        return View(userViewModel);
    }

    // GET: Index
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        // گرفتن تمام کاربران
        var users = await _userManager.Users.ToListAsync();

        // ایجاد مدل ViewModel برای نمایش کاربران
        var userViewModels = users.Select(user => new UserViewModel
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName
        }).ToList();

        return View(userViewModels);
    }
}
