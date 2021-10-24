using JxBackendService.Interface.Service;
using JxBackendService.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Service.Setting
{
    public class BackSideWebAppSettingService : BaseAppSettingService
    {

        protected override string MasterInloDbConnectionStringConfigKey => "ConnectionString";

        protected override string SlaveInloDbBakConnectionStringConfigKey => throw new NotSupportedException(); //後台有兩個站台,用功能切割db存取,故統一用master取得連線

        protected override string RabbitMqHostNameConfigKey => "HostName";

        protected override string RabbitMqPortConfigKey => "Port";

        protected override string RabbitMqUserNameConfigKey => "UserName";

        protected override string RabbitMqPasswordConfigKey => "Password";
        
        public override int AuthenticatorExpiredDays => 30;

        public override string CommonDataHash => GetAppSettingValue("CommonDataHash");

        public override bool IsClientPinCompareExactly => false;
    }
}
