using BackSideWeb.Model.Entity.MM;
using JxBackendService.Interface.Service.Web.BackSideWeb;

namespace BackSideWeb.Model.ViewModel.MM
{
    public class QueryBannerModel : MMBannerBs, IDataKey
    {
        public string IsActiveText => IsActive ? "显示" : "隐藏";
        public string? KeyContent => Id;
    }
}
