using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using TicketingSystem.Models;

namespace TicketingSystem.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // تعریف نقش‌ها
            string[] roleNames = { "Admin", "User", "Support" };

            // ایجاد نقش‌ها در پایگاه داده اگر وجود ندارند
            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName).ConfigureAwait(false);
                if (!roleExist)
                {
                    var role = new IdentityRole(roleName);
                    var result = await roleManager.CreateAsync(role).ConfigureAwait(false);
                    if (!result.Succeeded)
                    {
                        throw new Exception($"خطا در ایجاد نقش {roleName}: {string.Join(", ", result.Errors)}");
                    }
                }
            }

            // اطلاعات کاربر Admin
            var adminEmail = "nader@kashan.ir";
            var adminPassword = "Na12345@";

            // بررسی وجود کاربر Admin
            var adminUser = await userManager.FindByEmailAsync(adminEmail).ConfigureAwait(false);
            if (adminUser == null)
            {
                // ایجاد کاربر Admin
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FirstName = "Nader",
                    LastName = "Kashan"
                };

                var createAdminResult = await userManager.CreateAsync(adminUser, adminPassword).ConfigureAwait(false);
                if (!createAdminResult.Succeeded)
                {
                    throw new Exception($"خطا در ایجاد کاربر Admin: {string.Join(", ", createAdminResult.Errors)}");
                }
            }

            // اطمینان از انتساب نقش Admin به کاربر
            foreach (var roleName in roleNames)
            {
                if (!await userManager.IsInRoleAsync(adminUser, roleName).ConfigureAwait(false))
                {
                    var addRoleResult = await userManager.AddToRoleAsync(adminUser, roleName).ConfigureAwait(false);
                    if (!addRoleResult.Succeeded)
                    {
                        throw new Exception($"خطا در افزودن نقش {roleName} به کاربر: {string.Join(", ", addRoleResult.Errors)}");
                    }
                }
            }
        }
    }
}
