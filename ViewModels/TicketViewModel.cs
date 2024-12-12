using System;
using System.ComponentModel.DataAnnotations;

namespace TicketingSystem.ViewModels
{
    /// <summary>
    /// مدل نمایش اطلاعات تیکت.
    /// این مدل برای دریافت اطلاعات مربوط به تیکت از جمله عنوان، توضیحات و اولویت استفاده می‌شود.
    /// </summary>
    public class TicketViewModel
    {
        /// <summary>
        /// عنوان تیکت. 
        /// فیلد اجباری است و باید حداکثر 100 کاراکتر باشد.
        /// </summary>
        [Required(ErrorMessage = "عنوان تیکت باید وارد شود.")]
        [MaxLength(100, ErrorMessage = "عنوان نمی‌تواند بیش از 100 کاراکتر باشد.")]
        public string? Title { get; set; }

        /// <summary>
        /// توضیحات تیکت.
        /// فیلد اجباری است و باید حداکثر 500 کاراکتر باشد.
        /// </summary>
        [Required(ErrorMessage = "توضیحات تیکت باید وارد شود.")]
        [MaxLength(500, ErrorMessage = "توضیحات نمی‌تواند بیش از 500 کاراکتر باشد.")]
        public string? Description { get; set; }

        /// <summary>
        /// اولویت تیکت. 
        /// این فیلد اجباری است و باید مقداری برای اولویت تیکت وارد شود.
        /// </summary>
        [Required(ErrorMessage = "اولویت تیکت باید وارد شود.")]
        public string? Priority { get; set; }
    }
}
