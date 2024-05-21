using BackSideWeb.Model.Entity.MM;
using JxBackendService.Interface.Service.Web.BackSideWeb;

namespace BackSideWeb.Models.ViewModel.MM
{
    public class QueryHomeAnnouncementModel : MMHomeAnnouncementBs, IDataKey
    {
        public string IsActiveText => IsActive ? "显示" : "隐藏";
        public string KeyContent => Id.ToString();
    }
}
