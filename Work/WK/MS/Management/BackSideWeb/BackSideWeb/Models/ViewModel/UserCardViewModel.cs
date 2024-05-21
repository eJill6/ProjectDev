using Microsoft.AspNetCore.Mvc.Rendering;

namespace BackSideWeb.Models.ViewModel
{
    public class UserCardViewModel
    {
        public List<SelectListItem> CardTypeItems { get; set; }
        public List<SelectListItem> PayTypeItems { get; set; }
    }
}
