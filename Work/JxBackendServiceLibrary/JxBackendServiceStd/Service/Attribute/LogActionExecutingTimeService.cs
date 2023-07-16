using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Attribute;
using JxBackendService.Interface.Service.GlobalSystem;
using JxBackendService.Interface.Service.Web;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Service.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace JxBackendService.Service.Attribute
{
    public class LogActionExecutingTimeService : BaseService, ILogActionExecutingTimeService
    {
        private readonly ILogExecutingTimeService _logExecutingTimeService;

        private readonly IHttpContextService _httpContextService;

        private double? _warningMilliseconds;

        public ILogExecutingTimeService LogExecutingTimeService => _logExecutingTimeService;

        public LogActionExecutingTimeService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _httpContextService = DependencyUtil.ResolveService<IHttpContextService>();
            _logExecutingTimeService = ResolveJxBackendService<ILogExecutingTimeService>(DbConnectionTypes.Slave);
        }

        public void Init(double? warningMilliseconds)
        {
            _warningMilliseconds = warningMilliseconds;
        }

        public void ActionExecuting<T>(T actionExecutingContext)
        {
            _logExecutingTimeService.Start();
        }

        public void ActionExecuted<T>(T actionExecutedContext)
        {
            _logExecutingTimeService.Stop(
                (elapsedTotalMilliseconds) => $"URL:{_httpContextService.GetUri()} 花費:{elapsedTotalMilliseconds} 毫秒"
                , _warningMilliseconds);
        }
    }
}