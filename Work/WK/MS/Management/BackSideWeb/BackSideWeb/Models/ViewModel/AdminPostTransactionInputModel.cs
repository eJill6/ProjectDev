using BackSideWeb.Model.Entity.MM;
using JxBackendService.Model.Entity.Base;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BackSideWeb.Models.ViewModel
{
    public class AdminPostTransactionInputModel : BaseEntityModel
    {
        public MMAdminPostTransactionBs MMAdminPostTransactionBs { get; set; }
        public List<SelectListItem> PostTypeListItem { get; set; }
        public List<SelectListItem> OptionTypeListItem { get; set; }
        public List<SelectListItem> IsActiveListItem { get; set; }
        public int PostTypeSelected { get; set; }
        public int OptionTypeSelected { get; set; }
        public int IsActiveSelected { get; set; }
    }
}
