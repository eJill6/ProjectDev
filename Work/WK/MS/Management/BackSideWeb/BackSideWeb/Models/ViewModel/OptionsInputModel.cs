using BackSideWeb.Model.Entity.MM;
using JxBackendService.Model.Entity.Base;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BackSideWeb.Models.ViewModel
{
    public class OptionsInputModel : BaseEntityModel
    {
        public MMOptionsBs MMOptionsBs { get; set; }
        public List<SelectListItem>? PostTypeListItem { get; set; }
        public List<SelectListItem>? OptionTypeListItem { get; set; }
        public List<SelectListItem>? IsActiveListItem { get; set; }
        public int PostTypeSelected { get; set; }
        public int OptionTypeSelected { get; set; }
        public int IsActiveSelected { get; set; }
    }
}
