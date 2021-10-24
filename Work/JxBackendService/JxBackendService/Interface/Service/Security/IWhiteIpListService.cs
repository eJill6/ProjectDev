using JxBackendService.Model.Enums;

namespace JxBackendService.Interface.Service.Security
{
    public interface IWhiteIpListService
    {
        //bool Create(string ipAddress, WhiteIpType whiteIpType, string remark);
    }

    public interface IWhiteIpListReadService
    {
        bool IsActive(string ipAddress, WhiteIpType whiteIpType);
    }
}
