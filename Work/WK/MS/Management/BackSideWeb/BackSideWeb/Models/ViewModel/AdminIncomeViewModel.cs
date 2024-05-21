using Microsoft.AspNetCore.Mvc.Rendering;

namespace BackSideWeb.Models.ViewModel
{
    public class AdminIncomeViewModel
    {
        public List<SelectListItem> LockedStateItems { get; set; }
        public List<SelectListItem> IncomeStatementStatusItems { get; set; }

        public List<SelectListItem> PostTypeItems { get; set; }
        public List<SelectListItem> TimeTypeItems { get; set; }
        public List<SelectListItem> DiamondStatusItems { get; set; }
        
        public List<SelectListItem> BookingPaymentTypeItems { get; set; }
        /// <summary>
        /// 收益会员身份-
        /// </summary>
        public List<SelectListItem> IdentityTypeItems { get; set; }

    }
}
