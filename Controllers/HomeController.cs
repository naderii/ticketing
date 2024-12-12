using Microsoft.AspNetCore.Mvc; // برای استفاده از کنترلرها و اکشن‌ها در ASP.NET Core
using System.Diagnostics; // برای استفاده از امکانات مربوط به شناسایی خطاها و مشکلات، مانند استفاده از Activity برای ردیابی درخواست‌ها
using TicketingSystem.Models; // برای استفاده از مدل‌های مورد نیاز، مثل مدل ErrorViewModel

namespace TicketingSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger; // متغیر برای ثبت لاگ‌ها و خطاها در HomeController

        // سازنده برای تزریق وابستگی به ILogger که برای ثبت لاگ‌ها در کنترلر استفاده می‌شود
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger; // ذخیره سازی سرویس لاگ
        }

        // اکشن برای صفحه اصلی (نمایش صفحه Index)
        public IActionResult Index()
        {
            return View(); // بازگشت به ویو اصلی صفحه
        }

        // اکشن برای نمایش صفحه حریم خصوصی
        public IActionResult Privacy()
        {
            return View(); // بازگشت به ویو صفحه حریم خصوصی
        }

        // اکشن برای مدیریت خطاها با تنظیم کش برای عدم ذخیره‌سازی داده‌ها در کش
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            // ایجاد مدل ErrorViewModel و ارسال اطلاعات مربوط به خطا به ویو
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
