using BackSideWeb.Model.Entity.MM;
using JxBackendService.Model.Entity.Base;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BackSideWeb.Models.ViewModel
{
    public class AdvertisingContentUpdateViewModel : BaseEntityModel
    {
        public MMAdvertisingContentBs MMAdvertisingContentBs { get; set; }
        public List<SelectListItem>? Items { get; set; }
        public int SelectOption { get; set; }
    }
}
