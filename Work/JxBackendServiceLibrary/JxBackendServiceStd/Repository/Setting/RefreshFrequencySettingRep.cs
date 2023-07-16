using JxBackendService.Interface.Repository.Setting;
using JxBackendService.Model.Entity.Setting;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;

namespace JxBackendService.Repository.User
{
    public class RefreshFrequencySettingRep : BaseDbRepository<RefreshFrequencySetting>, IRefreshFrequencySettingRep
    {
        public RefreshFrequencySettingRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }
    }
}