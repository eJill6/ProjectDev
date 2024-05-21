using ControllerShareLib.Infrastructure.Jobs.Setting;
using ControllerShareLib.Interfaces.Service.Setting;
using JxBackendService.Service.Enums;

namespace ControllerShareLib.Services.Setting
{
    public class ControllerJobSettingWebService : BaseValueModelService<string, ControllerJobSetting>, IControllerJobSettingService
    {
        private static readonly HashSet<ControllerJobSetting> s_controllerJobSettings = new HashSet<ControllerJobSetting>()
        {
            ControllerJobSetting.MemoryCacheNextIssue,
        };

        protected override List<ControllerJobSetting> CreateAllList()
        {
            return base.CreateAllList().Where(w => s_controllerJobSettings.Contains(w)).ToList();
        }
    }
}