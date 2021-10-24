using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;

namespace JxBackendService.Interface.Service.Security
{
    public interface IAreaReadService
    {
        string GetArea(JxIpInformation jxIpInformation);
    }
}
