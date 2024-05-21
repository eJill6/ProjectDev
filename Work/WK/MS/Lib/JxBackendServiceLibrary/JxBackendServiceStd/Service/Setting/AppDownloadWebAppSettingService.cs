using JxBackendService.Interface.Service;
using JxBackendService.Model.ViewModel.AppDownload;
using JxBackendService.Service.Base;
using System;

namespace JxBackendService.Service.Setting
{
    public class AppDownloadWebAppSettingService : BaseAppSettingService, IAppDownloadWebAppSettingService
    {
        public AppDownloadWebAppSettingService()
        {
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