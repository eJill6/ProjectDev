using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;

namespace JxBackendService.Interface.Service.Security
{
    public interface IBlackLocationService
    {
        bool CreateBlackIp(string ipAddress, BlackIpType blackIpType, string remark, string userName);
    }

    public interface IBlackLocationReadService
    {
        bool IsFrontSideLoginIpActive(JxIpInformation jxIpInformation);
    }
}
