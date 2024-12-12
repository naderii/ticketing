using Microsoft.AspNetCore.Identity;

namespace TicketingSystem.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? ProfilePicture { get; set; } // مسیر ذخیره عکس
        public List<string> Roles { get; set; } = new List<string>(); // اضافه کردن ویژگی Roles


        // کانستراکتور
        public ApplicationUser()
        {
            FirstName = "No First Name"; // مقدار پیش‌فرض برای FirstName
            LastName = "No Last Name";   // مقدار پیش‌فرض برای LastName
        }
    }
}
