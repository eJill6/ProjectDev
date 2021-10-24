using System.Collections.Generic;
using JxBackendService.Interface.Repository.VIP;
using JxBackendService.Interface.Service.VIP;
using JxBackendService.Model.Entity.VIP;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Service.Base;

namespace JxBackendService.Service.VIP
{
    public class VIPSettingService : BaseService, IVIPSettingService
    {
        private readonly IVIPLevelSettingRep _vipLevelSettingRep;

        public VIPSettingService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser,
            dbConnectionType)
        {
            _vipLevelSettingRep = ResolveJxBackendService<IVIPLevelSettingRep>();
        }

        public List<VIPLevelSetting> GetAll()
        {
            return _vipLevelSettingRep.GetAll();
        }
    }
}