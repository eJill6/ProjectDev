using Microsoft.AspNetCore.Mvc.Rendering;

namespace BackSideWeb.Models.ViewModel
{
    public class OptionsViewModel
    {
        public List<SelectListItem> PostTypeItems { get; set; }
        public List<SelectListItem> OptionTypeItems { get; set; }
    }
}
