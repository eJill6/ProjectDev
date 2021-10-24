using JxBackendService.Interface.Service;
using JxBackendService.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Service.Setting
{
    /// <summary>
    /// 給IMBG之前的第三方套用
    /// </summary>
    public class OldTransferServiceAppSettingService : BaseAppSettingService
    {
        protected override string MasterInloDbConnectionStringConfigKey => "ConnectionString";

        protected override string SlaveInloDbBakConnectionStringConfigKey => throw new NotSupportedException(); //目前舊版只有一個db01連線字串

        protected override string RabbitMqHostNameConfigKey => "HostName";

        protected override string RabbitMqPortConfigKey => "Port";

        protected override string RabbitMqUserNameConfigKey => "UserName";

        protected override string RabbitMqPasswordConfigKey => "Password";

        public override int AuthenticatorExpiredDays => throw new NotImplementedException();

        public override string CommonDataHash => throw new NotImplementedException();
    }
}
