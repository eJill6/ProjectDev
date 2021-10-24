using System.Collections.Generic;
using JxBackendService.Model.Entity.VIP;

namespace JxBackendService.Interface.Service.VIP
{
    public interface IVIPSettingService
    {
        List<VIPLevelSetting> GetAll();
    }
}
