using JxBackendService.Model.Param.Setting;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel.Setting;

namespace JxBackendService.Interface.Service.Setting
{
    public interface IRefreshFrequencySettingService
    {
        RefreshFrequencySettingViewModel GetBWUserRefreshFrequencySettingInfo(string permissionKey);

        RefreshFrequencyInfo GetRefreshFrequencyInfo(string permissionKey);

        BaseReturnModel SaveRefreshFrequencySetting(SaveRefreshFrequencySettingParam settingParm);
    }
}