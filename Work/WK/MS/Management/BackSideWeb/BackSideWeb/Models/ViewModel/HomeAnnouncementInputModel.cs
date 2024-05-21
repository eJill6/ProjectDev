using BackSideWeb.Model.Entity.MM;
using JxBackendService.Model.Entity.Base;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BackSideWeb.Models.ViewModel
{
    public class HomeAnnouncementInputModel : BaseEntityModel
    {
        public MMHomeAnnouncementBs MMHomeAnnouncementBs { get; set; }
        public List<SelectListItem>? Items { get; set; }
    }
}