using TicketingSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TicketingSystem.Services
{
    /// <summary>
    /// رابط سرویس مدیریت کاربران.
    /// این رابط برای انجام عملیات‌های مختلف روی کاربران مانند دریافت لیست کاربران، تغییر نقش، ایجاد و حذف کاربران استفاده می‌شود.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// دریافت تمام کاربران از سیستم.
        /// </summary>
        /// <returns>لیستی از تمام کاربران در سیستم به صورت ناهمزمان (Asynchronous).</returns>
        Task<List<ApplicationUser>> GetAllUsers();

        /// <summary>
        /// تغییر نقش یک کاربر خاص.
        /// </summary>
        /// <param name="userId">شناسه کاربر که نقش آن تغییر خواهد کرد.</param>
        /// <param name="newRole">نقش جدیدی که به کاربر اختصاص داده می‌شود.</param>
        /// <returns>برگشت یک مقدار بولی برای تایید موفقیت عملیات.</returns>
        Task<bool> ChangeUserRole(string userId, string newRole);

        /// <summary>
        /// ایجاد یک کاربر جدید.
        /// </summary>
        /// <param name="userName">نام کاربری کاربر جدید.</param>
        /// <param name="email">ایمیل کاربر جدید.</param>
        /// <param name="password">رمز عبور کاربر جدید.</param>
        /// <returns>برگشت یک مقدار بولی برای تایید موفقیت ایجاد کاربر.</returns>
        Task<bool> CreateUser(string userName, string email, string password);

        /// <summary>
        /// حذف یک کاربر از سیستم.
        /// </summary>
        /// <param name="userId">شناسه کاربری که باید حذف شود.</param>
        /// <returns>برگشت یک مقدار بولی برای تایید موفقیت حذف کاربر.</returns>
        Task<bool> DeleteUser(string userId);
    }
}
