namespace TicketingSystem.Models
{
    /// <summary>
    /// مدل نمای خانه که برای ارسال داده‌ها به ویو صفحه خانه استفاده می‌شود.
    /// این مدل شامل پیامی خوشامدگویی است که در ویو نمایش داده می‌شود.
    /// </summary>
    public class HomeViewModel
    {
        /// <summary>
        /// پیام خوشامدگویی که به ویو ارسال می‌شود و در صفحه خانه نمایش داده می‌شود.
        /// </summary>
        public string? WelcomeMessage { get; set; }
    }
}
