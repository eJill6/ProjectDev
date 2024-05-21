using ControllerShareLib.Interfaces.Service.Controller;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Enums.Queue;

namespace ControllerShareLib.Infrastructure.Jobs
{
    public class ReloadGetWebGameLobbyMenuJob : BaseControllerJob
    {
        private readonly Lazy<IHomeControllerService> _homeControllerService;

        public ReloadGetWebGameLobbyMenuJob()
        {
            _homeControllerService = DependencyUtil.ResolveService<IHomeControllerService>();
        }

        protected override Task DoExecute()
        {
            _homeControllerService.Value.GetMobileApiGameLobbyMenu(isUseRequestHost: false, isForceRefresh: true);

            return Task.CompletedTask;
        }
    }
}