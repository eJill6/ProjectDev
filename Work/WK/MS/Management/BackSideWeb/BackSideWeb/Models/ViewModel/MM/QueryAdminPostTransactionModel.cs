using BackSideWeb.Model.Entity.MM;
using JxBackendService.Interface.Service.Web.BackSideWeb;

namespace BackSideWeb.Model.ViewModel.MM
{
    public class QueryAdminPostTransactionModel : MMAdminPostTransactionBs, IDataKey
    {

        public string KeyContent => Id.ToString();
    }
}
