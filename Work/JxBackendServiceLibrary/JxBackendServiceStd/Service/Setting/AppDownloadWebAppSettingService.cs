using Amazon.S3.Model.Internal.MarshallTransformations;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Net;
using JxBackendService.Interface.Service.Web;
using JxBackendService.Model.Enums;
using JxBackendService.Model.MessageQueue;
using JxBackendService.Model.ViewModel.AppDownload;
using JxBackendService.Service.Base;
using Microsoft.Extensions.DependencyModel;
using System;

namespace JxBackendService.Service.Setting
{
    public class AppDownloadWebAppSettingService : BaseAppSettingService, IAppDownloadWebAppSettingService
    {
        private readonly IHttpContextService _httpContextService;
        private readonly IRouteUtilService _routeUtilService;

        public AppDownloadWebAppSettingService()
        {
            _httpContextService = DependencyUtil.ResolveService<IHttpContextService>();
            _routeUtilService = DependencyUtil.ResolveService<IRouteUtilService>();

        }

        public override string CommonDataHash => throw new PlatformNotSupportedException();

        protected override string MasterInloDbConnectionStringConfigKey => throw new PlatformNotSupportedException();

        protected override string SlaveInloDbBakConnectionStringConfigKey => throw new NotImplementedException();

        public string OpenInstallAppKey
        {
            get
            {
                return ConfigUtilService.Get("OpenInstallAppKey");
            }
        }

        public string CustomerServiceUrl
        {
            get
            {
                return ConfigUtilService.Get("CustomerServiceUrl");
            }
        }

        public IOSLiteSetting IOSLiteSetting
            => ConfigUtilService.Get<IOSLiteSetting>("IOSLiteSetting");
    }
}