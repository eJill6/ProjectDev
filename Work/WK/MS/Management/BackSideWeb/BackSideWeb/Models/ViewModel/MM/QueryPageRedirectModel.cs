using BackSideWeb.Model.Entity.MM;
using JxBackendService.Interface.Service.Web.BackSideWeb;

namespace BackSideWeb.Model.ViewModel.Game
{
    public class QueryPageRedirectModel : MMPageRedirectBs, IDataKey
    {
        public string KeyContent => Id.ToString();
    }
}