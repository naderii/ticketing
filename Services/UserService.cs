using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TicketingSystem.Models;

namespace TicketingSystem.Services
{
    /// <summary>
    /// پیاده‌سازی سرویس مدیریت کاربران.
    /// این کلاس برای انجام عملیات‌های مختلف بر روی کاربران سیستم از جمله دریافت، ایجاد، حذف و تغییر نقش آنها استفاده می‌شود.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        /// <summary>
        /// سازنده برای ایجاد یک نمونه از سرویس مدیریت کاربران.
        /// </summary>
        /// <param name="userManager">مدیر کاربران برای انجام عملیات‌های مختلف.</param>
        public UserService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        /// <summary>
        /// دریافت تمام کاربران از سیستم.
        /// </summary>
        /// <returns>لیستی از تمام کاربران در سیستم به صورت ناهمزمان (Asynchronous).</returns>
        public async Task<List<ApplicationUser>> GetAllUsers()
        {
            // استفاده از ToListAsync برای تبدیل IQueryable به لیست
            var users = await _userManager.Users.ToListAsync();
            return users;
        }

        /// <summary>
        /// تغییر نقش یک کاربر.
        /// </summary>
        /// <param name="userId">شناسه کاربر که نقش آن باید تغییر کند.</param>
        /// <param name="newRole">نقش جدید که باید به کاربر اختصاص یابد.</param>
        /// <returns>برگشت یک مقدار بولی که نشان می‌دهد آیا عملیات موفق بوده است یا خیر.</returns>
        public async Task<bool> ChangeUserRole(string userId, string newRole)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            // حذف نقش‌های فعلی کاربر
            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);

            // اضافه کردن نقش جدید به کاربر
            var result = await _userManager.AddToRoleAsync(user, newRole);

            return result.Succeeded;
        }

        /// <summary>
        /// ایجاد یک کاربر جدید.
        /// </summary>
        /// <param name="userName">نام کاربری که می‌خواهید ایجاد کنید.</param>
        /// <param name="email">ایمیل کاربر جدید.</param>
        /// <param name="password">رمز عبور برای کاربر جدید.</param>
        /// <returns>برگشت یک مقدار بولی که نشان می‌دهد آیا ایجاد کاربر موفقیت‌آمیز بوده است یا خیر.</returns>
        public async Task<bool> CreateUser(string userName, string email, string password)
        {
            var user = new ApplicationUser
            {
                UserName = userName,
                Email = email
            };

            var result = await _userManager.CreateAsync(user, password);
            return result.Succeeded;
        }

        /// <summary>
        /// حذف یک کاربر از سیستم.
        /// </summary>
        /// <param name="userId">شناسه کاربر که باید حذف شود.</param>
        /// <returns>برگشت یک مقدار بولی که نشان می‌دهد آیا حذف کاربر موفقیت‌آمیز بوده است یا خیر.</returns>
        public async Task<bool> DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }
    }
}
