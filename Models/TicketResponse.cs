using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace TicketingSystem.Models
{
    /// <summary>
    /// مدل برای پاسخ به تیکت‌ها. این مدل اطلاعات مربوط به پاسخ‌ها به تیکت‌ها را شامل می‌شود.
    /// </summary>
    public class TicketResponse
    {
        /// <summary>
        /// شناسه منحصر به فرد پاسخ
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// شناسه تیکت که این پاسخ به آن تعلق دارد.
        /// </summary>
        [Required(ErrorMessage = "TicketId is required.")]
        public int TicketId { get; set; }

        /// <summary>
        /// متن پاسخ به تیکت.
        /// </summary>
        [Required(ErrorMessage = "Response text is required.")]
        public string ResponseText { get; set; }

        /// <summary>
        /// شناسه کاربر پاسخ‌دهنده. 
        /// </summary>
        [Required(ErrorMessage = "UserId is required.")]
        public string UserId { get; set; }

        /// <summary>
        /// تاریخ و زمان ایجاد پاسخ.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        /// <summary>
        /// ویژگی نیوگیشن برای دسترسی به تیکت مربوط به این پاسخ.
        /// </summary>
        public virtual Ticket Ticket { get; set; }
    }
}
