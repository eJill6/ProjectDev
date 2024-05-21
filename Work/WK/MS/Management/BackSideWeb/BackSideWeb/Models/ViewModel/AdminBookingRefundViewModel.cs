using Microsoft.AspNetCore.Mvc.Rendering;

namespace BackSideWeb.Models.ViewModel
{
    public class AdminBookingRefundViewModel
    {
        public List<SelectListItem> PaymentTypeItems { get; set; }
        public List<SelectListItem> ApplyReasonItems { get; set; }
        /// <summary>
        /// 身份选项
        /// </summary>
        public List<SelectListItem> IdentityTypeItems { get;set; }
    }
}
