using BackSideWeb.Model.Entity.MM;
using JxBackendService.Interface.Service.Web.BackSideWeb;

namespace BackSideWeb.Model.ViewModel.MM
{
    public class QueryAdminBookingModel : MMAdminBookingBs, IDataKey
    {
        public string KeyContent => BookingId.ToString();
    }
}
