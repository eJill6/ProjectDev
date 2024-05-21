using BackSideWeb.Model.Entity.MM;
using JxBackendService.Model.Entity.Base;
using JxBackendService.Model.Enums.MM;
using Microsoft.AspNetCore.Mvc.Rendering;
using MS.Core.MMModel.Extensions;

namespace BackSideWeb.Models.ViewModel
{
    public class PageRedirectUpdateViewModel : BaseEntityModel
    {
        public MMPageRedirectBs MMPageRedirectBs { get; set; }
        public List<SelectListItem>? RedirectTypeItems { get; set; }
        public int? RedirectTypeSelectOption { get; set; }
        public string RedirectTypeText 
        { 
            get 
            {
                return ((RedirectType)RedirectTypeSelectOption).GetDescription() ?? "-"; ; 
            } 
        }
    }
}