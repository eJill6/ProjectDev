using JxBackendService.Model.Param.OSS;

namespace JxBackendService.Interface.Service.Config
{
    public interface IOSSSettingService
    {
        IOSSSetting GetOSSClientSetting();

        IOSSSetting GetCoreOSSClientSetting();
    }
}