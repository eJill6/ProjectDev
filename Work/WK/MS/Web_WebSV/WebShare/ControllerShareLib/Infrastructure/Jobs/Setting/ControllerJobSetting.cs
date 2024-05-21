using JxBackendService.Interface.Service.Enums;
using JxBackendService.Model.Enums;
using JxBackendService.Service.Enums;

namespace ControllerShareLib.Infrastructure.Jobs.Setting
{
    public class ControllerJobSetting : BaseStringValueModel<ControllerJobSetting>
    {
        public Type JobType { get; private set; }

        public int IntervalSeconds { get; private set; }

        public double StartDelaySeconds { get; private set; }

        private ControllerJobSetting()
        { }

        public static readonly ControllerJobSetting MemoryCacheNextIssue = new ControllerJobSetting()
        {
            Value = nameof(MemoryCacheNextIssue),
            JobType = typeof(MemoryCacheNextIssueJob),
            IntervalSeconds = 1,
            StartDelaySeconds = 1,
        };

        public static readonly ControllerJobSetting ReloadGetWebGameLobbyMenu = new ControllerJobSetting()
        {
            Value = nameof(ReloadGetWebGameLobbyMenu),
            JobType = typeof(ReloadGetWebGameLobbyMenuJob),
            IntervalSeconds = 60
        };
    }
}