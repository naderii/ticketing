namespace TicketingSystem.Models
{
    /// <summary>
    /// مدل نمایش خطا که برای ارسال اطلاعات خطا و شناسه درخواست به ویو استفاده می‌شود.
    /// این مدل شامل شناسه درخواست و منطق نمایش آن است.
    /// </summary>
    public class ErrorViewModel
    {
        /// <summary>
        /// شناسه درخواست که به هنگام بروز خطا برای شناسایی درخواست HTTP استفاده می‌شود.
        /// </summary>
        public string? RequestId { get; set; }

        /// <summary>
        /// ویژگی محاسباتی برای نمایش شناسه درخواست در ویو.
        /// اگر RequestId موجود و غیر خالی باشد، این ویژگی به true برمی‌گرداند.
        /// </summary>
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
