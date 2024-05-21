using Microsoft.AspNetCore.Mvc.Rendering;

namespace BackSideWeb.Models.ViewModel
{
    public class UserViewModel
    {
        public List<SelectListItem> CardTypeItems { get; set; }

        public List<SelectListItem> IdentityTypeItems { get; set; }

        public List<SelectListItem> OpeningStatusItems { get; set; }
	}
}