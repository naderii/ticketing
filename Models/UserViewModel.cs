using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace TicketingSystem.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "وارد کردن ایمیل الزامی است")]
        [EmailAddress(ErrorMessage = "ایمیل معتبر وارد کنید")]
        public string Email { get; set; }

        [Required(ErrorMessage = "وارد کردن نام الزامی است")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "وارد کردن نام خانوادگی الزامی است")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "انتخاب نقش الزامی است")]
        public UserRole Role { get; set; } // انتخاب نقش از طریق Enum
    }

    public enum UserRole
    {
        [Display(Name = "مدیر")]
        Admin = 1,

        [Display(Name = "کاربر معمولی")]
        User = 2,

        [Display(Name = "پشتیبان")]
        Support = 3,


    }
}
