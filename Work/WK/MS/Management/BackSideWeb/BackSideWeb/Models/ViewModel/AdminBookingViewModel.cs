using Microsoft.AspNetCore.Mvc.Rendering;

namespace BackSideWeb.Models.ViewModel
{
    public class AdminBookingViewModel
    {
        public List<SelectListItem> TimeTypeItems { get; set; }
        public List<SelectListItem> PaymentTypeItems { get; set; }
        public List<SelectListItem> OrderStatusItems { get; set; }
        public List<SelectListItem> BookingStatusItems { get; set; }

        public List<SelectListItem> IdentityTypeItems { get; set; }
    }
}
