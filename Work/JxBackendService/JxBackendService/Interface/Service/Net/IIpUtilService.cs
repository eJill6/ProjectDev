using JxBackendService.Model.ViewModel;

namespace JxBackendService.Interface.Service.Net
{
    public interface IIpUtilService
    {
        JxIpInformation GetDoWorkIPInformation();

        string GetIPAddress();
    }
}
