using ControllerShareLib.Interfaces.Service;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Config;
using JxBackendService.Interface.Service.GlobalSystem;
using JxBackendService.Model.ViewModel;
using Quartz;

namespace ControllerShareLib.Infrastructure.Jobs
{
    [DisallowConcurrentExecution]
    public abstract class BaseControllerJob : IJob
    {
        private readonly Lazy<ICacheService> _cacheService;

        private readonly Lazy<IConfigUtilService> _configUtilService;

        private readonly Lazy<IEnvironmentService> _environmentService;

        protected ICacheService CacheService => _cacheService.Value;

        protected IConfigUtilService ConfigUtilService => _configUtilService.Value;

        protected EnvironmentUser EnvUser => new EnvironmentUser()
        {
            Application = _environmentService.Value.Application,
            LoginUser = new BasicUserInfo()
        };

        public BaseControllerJob()
        {
            _cacheService = DependencyUtil.ResolveService<ICacheService>(); ;
            _configUtilService = DependencyUtil.ResolveService<IConfigUtilService>();
            _environmentService = DependencyUtil.ResolveService<IEnvironmentService>();
        }

        protected abstract Task DoExecute();

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                await DoExecute();
            }
            catch (Exception ex)
            {
                var errorMsgUtilService = DependencyUtil.ResolveService<IErrorMsgUtilService>().Value;
                errorMsgUtilService.ErrorHandle(ex, EnvUser, SendErrorMsgTypes.Queue);
            }
        }
    }
}