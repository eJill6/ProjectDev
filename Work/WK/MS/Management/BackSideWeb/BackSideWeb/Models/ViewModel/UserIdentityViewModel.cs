using Microsoft.AspNetCore.Mvc.Rendering;

namespace BackSideWeb.Models.ViewModel
{
    public class UserIdentityViewModel
    {
        public List<SelectListItem> IdentityTypeItems { get; set; }
        public List<SelectListItem> AuditStatusItems { get; set; }
    }
}
