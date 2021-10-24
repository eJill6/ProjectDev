using JxBackendService.Interface.Service;
using JxBackendService.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Service.Setting
{
    /// <summary>
    /// 給IMSG以後的排程套用
    /// </summary>
    public class NewTransferServiceAppSettingService : BaseAppSettingService
    {
        protected override string MasterInloDbConnectionStringConfigKey => "Master_Inlodb_ConnectionString";

        protected override string SlaveInloDbBakConnectionStringConfigKey => "Slave_InlodbBak_ConnectionString";

        protected override string RabbitMqHostNameConfigKey => "RabbitMQ.HostName";

        protected override string RabbitMqPortConfigKey => "RabbitMQ.Port";

        protected override string RabbitMqUserNameConfigKey => "RabbitMQ.UserName";

        protected override string RabbitMqPasswordConfigKey => "RabbitMQ.Password";

        public override int AuthenticatorExpiredDays => throw new NotImplementedException();

        public override string CommonDataHash => throw new NotImplementedException();
    }
}
