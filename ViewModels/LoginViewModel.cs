using System.ComponentModel.DataAnnotations;

namespace TicketingSystem.ViewModels
{
    /// <summary>
    /// مدل نمایش اطلاعات ورود کاربر.
    /// این مدل برای دریافت اطلاعات ورود کاربر از جمله ایمیل، رمز عبور و گزینه "مرا به خاطر بسپار" استفاده می‌شود.
    /// </summary>
    public class LoginViewModel
    {
        /// <summary>
        /// ایمیل کاربر برای ورود.
        /// فیلد اجباری است و باید فرمت صحیح ایمیل را داشته باشد.
        /// </summary>
        [Required(ErrorMessage = "ایمیل باید وارد شود.")]
        [EmailAddress(ErrorMessage = "فرمت ایمیل صحیح نیست.")]
        public string? Email { get; set; }

        /// <summary>
        /// رمز عبور کاربر برای ورود.
        /// فیلد اجباری است و باید به صورت رمز عبور باشد.
        /// </summary>
        [Required(ErrorMessage = "رمز عبور باید وارد شود.")]
        [DataType(DataType.Password, ErrorMessage = "فرمت رمز عبور صحیح نیست.")]
        public string? Password { get; set; }

        /// <summary>
        /// مشخص می‌کند که آیا کاربر می‌خواهد برای دفعات بعدی وارد سیستم شود (یادآوری ورود).
        /// این فیلد اختیاری است.
        /// </summary>
        public bool RememberMe { get; set; }
    }
}
