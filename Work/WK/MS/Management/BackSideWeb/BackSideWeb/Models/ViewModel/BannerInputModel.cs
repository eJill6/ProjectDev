using BackSideWeb.Model.Entity.MM;
using JxBackendService.Model.Entity.Base;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BackSideWeb.Models.ViewModel
{
    public class BannerInputModel : BaseEntityModel
    {
        public MMBannerBs MMBannerBs { get; set; }
        public List<SelectListItem>? IsActiveListItem { get; set; }
        public int IsActiveSelected { get; set; }
        public List<SelectListItem>? LinkTypeItem { get; set; }
        public int LinkTypeSelected { get; set; }
        public IFormFile? File { get; set; }
        public string? ShowFileName { get; set; }
        public List<SelectListItem>? AreaTypeItem { get; set; }
        public int AreaTypeSelected { get; set; }
        public List<SelectListItem>? GameTypeItem { get; set; }
        public int GameTypeSelected { get; set; }

        public List<SelectListItem>? InsideTypeItem { get; set; }
        public int InsideTypeSelected { get; set; }
        public List<SelectListItem>? LocationTypeItem { get; set; }
        public int LocationTypeSelected { get; set; }
    }
}