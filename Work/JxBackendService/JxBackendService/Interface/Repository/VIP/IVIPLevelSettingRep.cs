using System.Collections.Generic;
using JxBackendService.Model.Entity.VIP;

namespace JxBackendService.Interface.Repository.VIP
{
    public interface IVIPLevelSettingRep : IBaseDbRepository<VIPLevelSetting>
    {
        List<VIPLevelSetting> GetAll();
    }
}