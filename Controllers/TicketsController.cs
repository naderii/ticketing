using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using TicketingSystem.Data;
using TicketingSystem.Models;
using System.Linq;
using System.Threading.Tasks;
using System;
using Microsoft.Data.SqlClient;
using TicketingSystem.Services;

namespace TicketingSystem.Controllers
{
    public class TicketsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ITicketService _ticketService;
        private readonly UserManager<ApplicationUser> _userManager;

        public TicketsController(ApplicationDbContext context, ITicketService ticketService, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _ticketService = ticketService;
            _userManager = userManager;
        }

        // اکشن Create (GET) برای نمایش فرم ایجاد تیکت
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // اکشن Create (POST) برای ایجاد تیکت جدید
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Ticket model)
        {
            // بررسی وارد شدن کاربر
            if (!User.Identity!.IsAuthenticated)
            {
                TempData["ErrorMessage"] = "لطفاً وارد سیستم شوید.";
                return RedirectToAction("Login", "Account");
            }

            // دریافت شناسه کاربر
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                TempData["ErrorMessage"] = "خطا در شناسایی کاربر. لطفاً دوباره وارد شوید.";
                return RedirectToAction("Login", "Account");
            }

            // پر کردن مقادیر تیکت
            model.UserId = userId;
            model.CreatedAt = DateTime.Now;
            model.Status = model.Status ?? "Open";  // مقدار پیش‌فرض وضعیت
            model.Priority = model.Priority ?? "Low";  // مقدار پیش‌فرض اولویت
            model.CreatedBy = User.Identity?.Name ?? "Unknown";

            try
            {
                // اضافه کردن تیکت به دیتابیس
                _context.Tickets.Add(model);
                await _context.SaveChangesAsync();

                // تنظیم پیام موفقیت در TempData
                TempData["SuccessMessage"] = "تیکت با موفقیت ثبت شد!";
                return RedirectToAction(nameof(Index));  // هدایت به صفحه لیست تیکت‌ها
            }
            catch (Exception ex)
            {
                // افزودن جزئیات خطا در صورت بروز مشکل
                TempData["ErrorMessage"] = "خطا در ثبت تیکت: " + ex.Message;
                return View(model);
            }
        }

        // اکشن نمایش جزئیات تیکت
        public async Task<IActionResult> Details(int id)
        {
            // واکشی تیکت و پاسخ‌ها از دیتابیس
            var ticket = await _context.Tickets
                .Include(t => t.Responses) // اینجا باید Responses را هم include کنید تا پاسخ‌ها لود شوند
                .FirstOrDefaultAsync(t => t.Id == id);

            if (ticket == null)
            {
                TempData["ErrorMessage"] = "تیکت پیدا نشد.";
                return RedirectToAction(nameof(Index));
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRoles = await _userManager.GetRolesAsync(await _userManager.GetUserAsync(User));

            // بررسی نقش‌ها: "Admin" و "Support" به همه تیکت‌ها دسترسی دارند
            if (userRoles.Contains("Admin") || userRoles.Contains("Support") || ticket.UserId == userId)
            {
                return View(ticket);
            }
            else
            {
                TempData["ErrorMessage"] = "شما دسترسی به این تیکت ندارید.";
                return RedirectToAction(nameof(Index));
            }
        }

        // اکشن نمایش لیست تیکت‌ها برای کاربر جاری
        [Authorize]
        public async Task<IActionResult> Index(string searchQuery, string sortBy, string sortOrder, int page = 1)
        {
            // بررسی وارد شدن کاربر
            if (!User.Identity!.IsAuthenticated)
            {
                TempData["ErrorMessage"] = "لطفاً وارد سیستم شوید.";
                return RedirectToAction("Login", "Account");
            }

            // دریافت شناسه کاربر
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                TempData["ErrorMessage"] = "خطا در شناسایی کاربر.";
                return RedirectToAction("Login", "Account");
            }

            var userRoles = await _userManager.GetRolesAsync(await _userManager.GetUserAsync(User));

            // بررسی نقش‌ها: "Admin" و "Support" به تمام تیکت‌ها دسترسی دارند
            IQueryable<Ticket> ticketsQuery;

            if (userRoles.Contains("Admin") || userRoles.Contains("Support"))
            {
                ticketsQuery = _context.Tickets; // تمام تیکت‌ها برای Admin و Support
            }
            else
            {
                ticketsQuery = _context.Tickets.Where(t => t.UserId == userId); // فقط تیکت‌های مربوط به کاربر جاری
            }

            // جستجو
            if (!string.IsNullOrEmpty(searchQuery))
            {
                ticketsQuery = ticketsQuery.Where(t => t.Title.Contains(searchQuery) || t.Priority.Contains(searchQuery));
            }

            // Apply sorting
            switch (sortBy)
            {
                case "Id":
                    ticketsQuery = sortOrder == "asc" ? ticketsQuery.OrderBy(t => t.Id) : ticketsQuery.OrderByDescending(t => t.Id);
                    break;
                case "Title":
                    ticketsQuery = sortOrder == "asc" ? ticketsQuery.OrderBy(t => t.Title) : ticketsQuery.OrderByDescending(t => t.Title);
                    break;
                case "Priority":
                    ticketsQuery = sortOrder == "asc" ? ticketsQuery.OrderBy(t => t.Priority) : ticketsQuery.OrderByDescending(t => t.Priority);
                    break;
                case "CreatedAt":
                    ticketsQuery = sortOrder == "asc" ? ticketsQuery.OrderBy(t => t.CreatedAt) : ticketsQuery.OrderByDescending(t => t.CreatedAt);
                    break;
                default:
                    ticketsQuery = ticketsQuery.OrderBy(t => t.CreatedAt); // default sorting by CreatedAt
                    break;
            }

            // صفحه‌بندی
            int pageSize = 10;
            int totalTickets = await ticketsQuery.CountAsync();
            var tickets = await ticketsQuery.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            var viewModel = new TicketListViewModel
            {
                Tickets = tickets,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling((double)totalTickets / pageSize),
                SortBy = sortBy,
                SortOrder = sortOrder,
                SearchQuery = searchQuery
            };

            return View(viewModel);
        }

        // اکشن برای نمایش تعداد تیکت‌های کاربر
        [Authorize]
        public async Task<IActionResult> UserTicketCount()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                TempData["ErrorMessage"] = "شما وارد سیستم نشده‌اید.";
                return RedirectToAction("Login", "Account");
            }

            // دریافت تعداد تیکت‌های کاربر
            var ticketCount = await _ticketService.GetUserTicketCount(userId);

            // می‌توانید این مقدار را به ویو ارسال کنید یا در TempData ذخیره کنید
            ViewData["TicketCount"] = ticketCount;

            return View();
        }

        // اکشن حذف تیکت
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            // بررسی دسترسی کاربر
            var ticket = await _context.Tickets.FirstOrDefaultAsync(t => t.Id == id);
            if (ticket == null)
            {
                TempData["ErrorMessage"] = "تیکت پیدا نشد.";
                return RedirectToAction(nameof(Index));
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRoles = await _userManager.GetRolesAsync(await _userManager.GetUserAsync(User));

            // بررسی نقش‌ها: "Admin" و "Support" به همه تیکت‌ها دسترسی دارند
            if (userRoles.Contains("Admin") || userRoles.Contains("Support") || ticket.UserId == userId)
            {
                var result = await _ticketService.DeleteTicketAsync(id);
                if (result)
                {
                    TempData["SuccessMessage"] = "تیکت با موفقیت حذف شد.";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["ErrorMessage"] = "خطا در حذف تیکت.";
                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                TempData["ErrorMessage"] = "شما دسترسی برای حذف این تیکت را ندارید.";
                return RedirectToAction(nameof(Index));
            }
        }
        [Authorize]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var ticket = await _context.Tickets.FirstOrDefaultAsync(t => t.Id == id);
            if (ticket == null)
            {
                TempData["ErrorMessage"] = "تیکت پیدا نشد.";
                return RedirectToAction(nameof(Index));
            }

            return View(ticket);
        }

        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            // واکشی تیکت از دیتابیس
            var ticket = await _context.Tickets.FirstOrDefaultAsync(t => t.Id == id);
            if (ticket == null)
            {
                TempData["ErrorMessage"] = "تیکت پیدا نشد.";
                return RedirectToAction(nameof(Index));
            }

            // بررسی دسترسی کاربر
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRoles = await _userManager.GetRolesAsync(await _userManager.GetUserAsync(User));

            // بررسی نقش‌ها: "Admin" و "Support" به همه تیکت‌ها دسترسی دارند
            if (userRoles.Contains("Admin") || userRoles.Contains("Support") || ticket.UserId == userId)
            {
                return View(ticket);
            }
            else
            {
                TempData["ErrorMessage"] = "شما دسترسی به این تیکت ندارید.";
                return RedirectToAction(nameof(Index));
            }
        }
        // Enum برای وضعیت تیکت‌ها
        public enum TicketStatus
        {
            Open,
            Closed,
            InProgress
        }

        // اکشن Edit (POST) برای ذخیره تغییرات تیکت
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Ticket model)
        {
            // بررسی وارد شدن کاربر
            if (!User.Identity!.IsAuthenticated)
            {
                TempData["ErrorMessage"] = "لطفاً وارد سیستم شوید.";
                return RedirectToAction("Login", "Account");
            }

            // واکشی تیکت از دیتابیس
            var ticket = await _context.Tickets.FirstOrDefaultAsync(t => t.Id == id);
            if (ticket == null)
            {
                TempData["ErrorMessage"] = "تیکت پیدا نشد.";
                return RedirectToAction(nameof(Index));
            }

            // بررسی دسترسی کاربر
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRoles = await _userManager.GetRolesAsync(await _userManager.GetUserAsync(User));

            // بررسی نقش‌ها: "Admin" و "Support" به همه تیکت‌ها دسترسی دارند
            if (userRoles.Contains("Admin") || userRoles.Contains("Support") || ticket.UserId == userId)
            {
                // بررسی وضعیت تیکت وارد شده
                if (!Enum.IsDefined(typeof(TicketStatus), model.Status))
                {
                    TempData["ErrorMessage"] = "وضعیت وارد شده معتبر نیست.";
                    return View(ticket);
                }

                // به‌روزرسانی مقادیر تیکت
                ticket.Title = model.Title;
                ticket.Description = model.Description;
                ticket.Priority = model.Priority ?? "Low";  // استفاده از مقدار پیش‌فرض در صورت عدم ارسال
                ticket.Status = model.Status ?? TicketStatus.Open.ToString();  // استفاده از مقدار پیش‌فرض در صورت عدم ارسال
                ticket.UpdatedAt = DateTime.Now;

                try
                {
                    // ذخیره تغییرات در دیتابیس
                    _context.Tickets.Update(ticket);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "تیکت با موفقیت ویرایش شد.";
                    return RedirectToAction(nameof(Index)); // هدایت به صفحه لیست تیکت‌ها
                }
                catch (Exception ex)
                {
                    // افزودن جزئیات خطا در صورت بروز مشکل
                    TempData["ErrorMessage"] = "خطا در ویرایش تیکت: " + ex.Message;
                    return View(ticket);
                }
            }
            else
            {
                TempData["ErrorMessage"] = "شما دسترسی به ویرایش این تیکت را ندارید.";
                return RedirectToAction(nameof(Index));
            }
        }

    }

}

