using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using TicketingSystem.Data;
using TicketingSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace TicketingSystem.Controllers
{
    public class TicketResponsesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public TicketResponsesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // اکشن Create (GET) برای نمایش فرم ایجاد پاسخ به تیکت
        [Authorize]
        public IActionResult Create(int ticketId)
        {
            // بررسی وجود تیکت
            var ticket = _context.Tickets.FirstOrDefault(t => t.Id == ticketId);
            if (ticket == null)
            {
                TempData["ErrorMessage"] = "تیکت مورد نظر پیدا نشد.";
                return RedirectToAction("Index", "Tickets");
            }

            // ارسال اطلاعات تیکت به ویو
            ViewBag.TicketId = ticketId;
            return View();
        }

        // اکشن Create (POST) برای ارسال پاسخ به تیکت
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TicketResponse model)
        {
            if (!User.Identity!.IsAuthenticated)
            {
                TempData["ErrorMessage"] = "لطفاً وارد سیستم شوید.";
                return RedirectToAction("Login", "Account");
            }

            var userNames = new Dictionary<string, string>();

            // واکشی تیکت از دیتابیس
            var ticket = await _context.Tickets.FirstOrDefaultAsync(t => t.Id == model.TicketId);
            if (ticket != null)
            {
                foreach (var response in ticket.Responses)
                {
                    var user = await _userManager.FindByIdAsync(response.UserId);
                    if (user != null)
                    {
                        userNames[response.UserId] = $"{user.FirstName} {user.LastName}";
                    }
                }
            }

            // دریافت شناسه کاربر
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                TempData["ErrorMessage"] = "خطا در شناسایی کاربر. لطفاً دوباره وارد شوید.";
                return RedirectToAction("Login", "Account");
            }

            // بررسی وجود تیکت
            if (ticket == null)
            {
                TempData["ErrorMessage"] = "تیکت پیدا نشد.";
                return RedirectToAction("Index", "Tickets");
            }

            // پر کردن مقادیر مدل پاسخ
            model.UserId = userId;
            model.CreatedAt = DateTime.Now;

            try
            {
                // اضافه کردن پاسخ جدید به دیتابیس
                _context.TicketResponses.Add(model);
                await _context.SaveChangesAsync();

                // تنظیم پیام موفقیت
                TempData["SuccessMessage"] = "پاسخ با موفقیت ثبت شد!";
                return RedirectToAction("Details", "Tickets", new { id = model.TicketId });
            }
            catch (Exception ex)
            {
                // افزودن جزئیات خطا در صورت بروز مشکل
                TempData["ErrorMessage"] = "خطا در ثبت پاسخ: " + ex.Message;
                return View(model);
            }
        }

        // اکشن نمایش جزئیات پاسخ‌ها به یک تیکت
        public async Task<IActionResult> Details(int id)
        {
            var ticket = await _context.Tickets
                .Include(t => t.Responses)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (ticket == null)
            {
                TempData["ErrorMessage"] = "تیکت پیدا نشد.";
                return RedirectToAction(nameof(Index), "Tickets");
            }

            // دیکشنری برای ذخیره نام‌های خانوادگی کاربران
            var userNames = new Dictionary<string, string>();

            // دریافت نام و نام خانوادگی برای هر پاسخ
            foreach (var response in ticket.Responses)
            {
                var user = await _userManager.FindByIdAsync(response.UserId);
                if (user != null)
                {
                    userNames[response.UserId] = $"{user.FirstName} {user.LastName}";
                }
            }

            // ارسال نام‌های کاربران به ویو
            ViewBag.UserNames = userNames;

            // دسترسی به پاسخ‌ها بر اساس نقش کاربر
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRoles = await _userManager.GetRolesAsync(await _userManager.GetUserAsync(User));

            // بررسی نقش‌ها: "Admin" و "Support" به تمام پاسخ‌ها دسترسی دارند
            if (userRoles.Contains("Admin") || userRoles.Contains("Support") || ticket.UserId == userId)
            {
                return View(ticket);
            }
            else
            {
                TempData["ErrorMessage"] = "شما دسترسی به این تیکت ندارید.";
                return RedirectToAction(nameof(Index), "Tickets");
            }
        }

        // اکشن برای نمایش لیست تیکت‌ها
        public async Task<IActionResult> Index()
        {
            var tickets = await _context.Tickets.ToListAsync();
            return View(tickets);
        }

    }
}
