using BackSideWeb.Model.Entity.MM;
using JxBackendService.Interface.Service.Web.BackSideWeb;

namespace BackSideWeb.Model.ViewModel.MM
{
    public class QueryAdminBookingRefundModel : MMAdminBookingRefundBs, IDataKey
    {
        public string KeyContent => RefundId.ToString();
    }
}
