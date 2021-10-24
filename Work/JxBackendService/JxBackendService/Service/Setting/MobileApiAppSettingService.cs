using JxBackendService.Interface.Service;
using JxBackendService.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Service.Setting
{
    public class MobileApiAppSettingService : BaseAppSettingService
    {
        protected override string MasterInloDbConnectionStringConfigKey => "ConnectionString";

        protected override string SlaveInloDbBakConnectionStringConfigKey => "ConnectionString_Inlodb_Bak";

        protected override string RabbitMqHostNameConfigKey => "HostName";

        protected override string RabbitMqPortConfigKey => "Port";

        protected override string RabbitMqUserNameConfigKey => "UserName";

        protected override string RabbitMqPasswordConfigKey => "Password";

        public override int AuthenticatorExpiredDays => FrontSideAuthenticatorExpiredDays;

        public override string CommonDataHash => GetAppSettingValue("CommonHash");
    }
}
