using BackSideWeb.Model.Entity.MM;
using JxBackendService.Interface.Service.Web.BackSideWeb;

namespace BackSideWeb.Model.ViewModel.MM
{
    public class QueryAdminIncomeModel : MMAdminIncomeBs, IDataKey
    {
        public string KeyContent => Id.ToString();
    }
}
