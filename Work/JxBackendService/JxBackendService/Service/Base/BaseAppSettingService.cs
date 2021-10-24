using JxBackendService.Common.Util;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Finance.Apollo;
using JxBackendService.Model.MessageQueue;
using System;
using System.Configuration;

namespace JxBackendService.Service.Base
{
    public abstract class BaseAppSettingService : IAppSettingService
    {
        private string _masterInloDbConnectionString;
        private string _slaveInloDbConnectionString;
        private string _redisConnectionString;

        protected int FrontSideAuthenticatorExpiredDays => 365000;

        protected abstract string MasterInloDbConnectionStringConfigKey { get; }

        protected abstract string SlaveInloDbBakConnectionStringConfigKey { get; }

        protected abstract string RabbitMqHostNameConfigKey { get; }

        protected abstract string RabbitMqPortConfigKey { get; }

        protected abstract string RabbitMqUserNameConfigKey { get; }

        protected abstract string RabbitMqPasswordConfigKey { get; }

        public virtual bool IsClientPinCompareExactly => true;


        public string GetConnectionString(DbConnectionTypes dbConnectionType)
        {
            string connectionString = null;

            if (dbConnectionType == DbConnectionTypes.Master)
            {
                connectionString = GetMasterInloDbConnectionString();
            }
            else if (dbConnectionType == DbConnectionTypes.Slave)
            {
                connectionString = GetSlaveInloDbBakConnectionString();
            }
            else
            {
                throw new NotImplementedException();
            }

            return connectionString;
        }

        public string GetRedisConnectionString(DbIndexes dbIndex)
        {
            _redisConnectionString = AssignValueOnceUtil.GetAssignValueOnce(_redisConnectionString, () =>
            {
                return CommUtil.CommUtil.DecryptDES(SharedAppSettings.RedisConnectionString) +
                $",poolsize=100,ssl=false,writeBuffer=20480,defaultDatabase={(int)dbIndex}";
            });

            return _redisConnectionString;
        }

        public RabbitMQSetting GetRabbitMQSetting()
        {
            string hostName = GetAppSettingValue(RabbitMqHostNameConfigKey);
            int port = GetAppSettingValue(RabbitMqPortConfigKey).ToInt32();
            string userName = GetAppSettingValue(RabbitMqUserNameConfigKey);
            string password = GetAppSettingValue(RabbitMqPasswordConfigKey);

            return new RabbitMQSetting()
            {
                HostName = hostName,
                Port = port,
                UserName = userName,
                Password = password
            };
        }

        public abstract int AuthenticatorExpiredDays { get; }

        public abstract string CommonDataHash { get; }

        public string EmailHash => GetAppSettingValue("EmailHash");

        protected string GetAppSettingValue(string name)
        {
            return ConfigurationManager.AppSettings[name];
        }

        private string GetMasterInloDbConnectionString()
        {
            _masterInloDbConnectionString = _masterInloDbConnectionString.GetAssignValueOnce(() =>
            {
                string encryptConnectionString = GetAppSettingValue(MasterInloDbConnectionStringConfigKey);

                if (encryptConnectionString.IsNullOrEmpty())
                {
                    throw new ArgumentNullException("InloDbConnectionStringConfigKey is null");
                }

                return CommUtil.CommUtil.DecryptDES(encryptConnectionString);
            });

            return _masterInloDbConnectionString;
        }

        private string GetSlaveInloDbBakConnectionString()
        {
            _slaveInloDbConnectionString = _slaveInloDbConnectionString.GetAssignValueOnce(() =>
            {
                string encryptConnectionString = GetAppSettingValue(SlaveInloDbBakConnectionStringConfigKey);

                if (encryptConnectionString.IsNullOrEmpty())
                {
                    throw new ArgumentNullException("InloDbBakConnectionStringConfigKey is null");
                }

                return CommUtil.CommUtil.DecryptDES(encryptConnectionString);
            });

            return _slaveInloDbConnectionString;
        }

        public ApolloConfigParam GetApolloConfigParam()
        {
            string apollokey = GetAppSettingValue("Apollokey");
            string apolloPostUrl = GetAppSettingValue("ApolloPostUrl");

            return new ApolloConfigParam()
            {
                Apollokey = apollokey,
                ApolloPostUrl = apolloPostUrl
            };
        }
    }
}
