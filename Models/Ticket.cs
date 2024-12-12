using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TicketingSystem.Models
{
    /// <summary>
    /// مدل برای تیکت‌ها در سیستم. 
    /// این مدل اطلاعات مربوط به یک تیکت، از جمله عنوان، توضیحات، اولویت، وضعیت و ارتباطات با کاربر و پاسخ‌ها را ذخیره می‌کند.
    /// </summary>
    public class Ticket
    {
        /// <summary>
        /// شناسه منحصر به فرد تیکت
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// عنوان تیکت. این ویژگی الزامی است.
        /// </summary>
        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; }

        /// <summary>
        /// توضیحات تیکت. این ویژگی الزامی است.
        /// </summary>
        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }

        /// <summary>
        /// اولویت تیکت. این ویژگی الزامی است.
        /// </summary>
        [Required(ErrorMessage = "Priority is required.")]
        public string Priority { get; set; }

        /// <summary>
        /// وضعیت تیکت. این ویژگی الزامی است.
        /// </summary>
        [Required(ErrorMessage = "Status is required.")]
        public string Status { get; set; }

        /// <summary>
        /// کاربری که این تیکت را ایجاد کرده است.
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// تاریخ و زمان ایجاد تیکت. این ویژگی الزامی است.
        /// </summary>
        [Required(ErrorMessage = "Creation time is required.")]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// شناسه کاربر مرتبط با این تیکت. این ویژگی الزامی است.
        /// </summary>
        [Required(ErrorMessage = "UserId is required.")]
        public string UserId { get; set; }

        /// <summary>
        /// ویژگی نیوگیشن برای دسترسی به اطلاعات کاربر مرتبط با این تیکت.
        /// </summary>
        public virtual ApplicationUser User { get; set; }

        /// <summary>
        /// لیستی از پاسخ‌های مرتبط با این تیکت.
        /// </summary>
        public virtual ICollection<TicketResponse> Responses { get; set; }

        /// <summary>
        /// سازنده برای مقداردهی پیش‌فرض ویژگی‌ها
        /// </summary>
        public Ticket()
        {
            Status = "Open"; // وضعیت پیش‌فرض تیکت "Open"
            Priority = "Low"; // اولویت پیش‌فرض تیکت "Low"
            CreatedBy = "System"; // کاربر پیش‌فرض "System"
            CreatedAt = DateTime.Now; // زمان ایجاد پیش‌فرض به‌صورت زمان کنونی
            Responses = new List<TicketResponse>(); // ایجاد لیست پیش‌فرض برای پاسخ‌ها
        }
    }
}
