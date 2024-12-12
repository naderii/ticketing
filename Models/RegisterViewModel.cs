using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace TicketingSystem.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "ایمیل اجباری است.")]
        [EmailAddress(ErrorMessage = "فرمت ایمیل معتبر نیست.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "رمز عبور اجباری است.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "تأیید رمز عبور اجباری است.")]
        [Compare("Password", ErrorMessage = "رمز عبور و تأیید آن یکسان نیستند.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "نام اجباری است.")]
        [StringLength(50, ErrorMessage = "نام نباید بیش از ۵۰ کاراکتر باشد.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "نام خانوادگی اجباری است.")]
        [StringLength(50, ErrorMessage = "نام خانوادگی نباید بیش از ۵۰ کاراکتر باشد.")]
        public string LastName { get; set; }

        [Phone(ErrorMessage = "شماره تلفن معتبر نیست.")]
        public string? Phone { get; set; }

        public IFormFile? ProfilePicture { get; set; } // برای آپلود عکس

        // فیلد جدید برای پشتیبان
        public List<string> AvailableRoles { get; set; } = new List<string>();
        public List<string> SelectedRoles { get; set; } = new List<string>();
    }
}
