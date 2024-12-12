using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TicketingSystem.Models;
using TicketingSystem.Services;
using TicketingSystem.ViewModels;

namespace TicketingSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ITicketService _ticketService;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            ITicketService ticketService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _ticketService = ticketService;
        }

        // صفحه ورود
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // فرآیند ورود کاربر
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    return RedirectToLocal(returnUrl);
                }

                // اگر ورود ناموفق بود
                ModelState.AddModelError(string.Empty, "نام کاربری یا کلمه عبور اشتباه است.");
            }
            return View(model);
        }

        // فرآیند خروج کاربر
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        // متد برای هدایت به صفحه‌های محلی
        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        // صفحه اصلی پس از ورود
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User); // دریافت کاربر وارد شده
            var userName = user != null ? user.FirstName + " " + user.LastName : "کاربر ناشناس"; // نمایش نام کاربر
            ViewData["Name"] = userName;  // ارسال نام کاربر به ویو
            return View();
        }

        // صفحه نمایش فرم ثبت‌نام
        [HttpGet]
        public IActionResult Register()
        {
            var model = new RegisterViewModel
            {
                // هیچ نیازی به SelectedRole نداریم چون به صورت پیش‌فرض نقش "User" را تخصیص خواهیم داد
            };

            return View(model);
        }

        // فرآیند ثبت‌نام کاربر
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // بررسی ایمیل تکراری
                var existingUser = await _userManager.FindByEmailAsync(model.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Email", "این ایمیل قبلاً ثبت‌نام شده است.");
                    return View(model);
                }

                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.Phone,
                };

                // ذخیره عکس پروفایل با اعتبارسنجی
                if (model.ProfilePicture != null)
                {
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                    var extension = Path.GetExtension(model.ProfilePicture.FileName).ToLower();
                    if (!allowedExtensions.Contains(extension))
                    {
                        ModelState.AddModelError("ProfilePicture", "فایل انتخاب شده باید یک تصویر باشد.");
                        return View(model);
                    }

                    if (model.ProfilePicture.Length > 2 * 1024 * 1024)
                    {
                        ModelState.AddModelError("ProfilePicture", "اندازه فایل نباید بیش از ۲ مگابایت باشد.");
                        return View(model);
                    }

                    var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }

                    var fileName = $"{Guid.NewGuid()}_{model.ProfilePicture.FileName}";
                    var filePath = Path.Combine(uploadPath, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.ProfilePicture.CopyToAsync(stream);
                    }

                    user.ProfilePicture = $"/images/{fileName}";
                }

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // نقش پیش‌فرض "User" به کاربر اختصاص داده می‌شود
                    await _userManager.AddToRoleAsync(user, "User");

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }

                //مدیریت خطاهای ایجاد کاربر
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }


        // ویرایش اطلاعات کاربر توسط مدیر
        //[HttpGet]
        //public async Task<IActionResult> EditUser(string id)
        //{
        //    if (id == null)
        //    {
        //        return BadRequest("شناسه کاربر معتبر نیست.");
        //    }

        //    var user = await _userManager.FindByIdAsync(id);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    // بارگذاری نقش‌ها و گرفتن نقش‌های فعلی کاربر
        //    var roles = await _roleManager.Roles.ToListAsync();
        //    var userRoles = await _userManager.GetRolesAsync(user);

        //    var userViewModel = new UserViewModel
        //    {
        //        Id = user.Id,
        //        Email = user.Email,
        //        FirstName = user.FirstName,
        //        LastName = user.LastName,
        //        Roles = new SelectList(roles, "Name", "Name"), // بارگذاری نقش‌ها
        //        SelectedRoles = userRoles?.ToList() ?? new List<string>() // گرفتن نقش‌های فعلی کاربر
        //    };

        //    return View(userViewModel);
        //}

        //[HttpPost]
        //public async Task<IActionResult> EditUser(UserViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = await _userManager.FindByIdAsync(model.Id);
        //        if (user == null)
        //        {
        //            return NotFound();
        //        }

        //        user.FirstName = model.FirstName;
        //        user.LastName = model.LastName;
        //        user.Email = model.Email;

        //        // حذف نقش‌های قدیمی
        //        var currentRoles = await _userManager.GetRolesAsync(user);
        //        foreach (var role in currentRoles)
        //        {
        //            await _userManager.RemoveFromRoleAsync(user, role);
        //        }

        //        // اضافه کردن نقش‌های جدید
        //        foreach (var role in model.SelectedRoles)
        //        {
        //            await _userManager.AddToRoleAsync(user, role);
        //        }

        //        await _userManager.UpdateAsync(user);

        //        TempData["SuccessMessage"] = "تغییرات با موفقیت ذخیره شد";
        //        return RedirectToAction("Index");
        //    }

        //    return View(model);
        //}
    }
}
