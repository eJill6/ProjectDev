using JxBackendService.Common.Util;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Common;
using JxBackendService.Model.Common.IMOne;
using JxBackendService.Model.Enums;
using JxBackendService.Model.MessageQueue;
using JxBackendService.Model.Param.OSS;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace JxBackendService.Service.Base
{
    public abstract class BaseAppSettingService : IAppSettingService
    {
        private string _masterInloDbConnectionString;

        private string _slaveInloDbConnectionString;

        private string _redisConnectionString;

        protected abstract string MasterInloDbConnectionStringConfigKey { get; }

        protected abstract string SlaveInloDbBakConnectionStringConfigKey { get; }

        private readonly string RabbitMqHostNameConfigKey = "RabbitMQ.HostName";

        private readonly string RabbitMqPortConfigKey = "RabbitMQ.Port";

        private readonly string RabbitMqUserNameConfigKey = "RabbitMQ.UserName";

        private readonly string RabbitMqPasswordConfigKey = "RabbitMQ.Password";

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
                return CommUtil.CommUtil.DecryptDES(SharedAppSettings.RedisConnectionString);
            });

            string paramString = $",poolsize=100,ssl=false,writeBuffer=20480,defaultDatabase={(int)dbIndex}";

            return _redisConnectionString + paramString;
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

        public abstract string CommonDataHash { get; }

        protected string GetAppSettingValue(string name, string defaultValue)
        {
            string value = ConfigurationManager.AppSettings[name];

            if (value == null && defaultValue != null)
            {
                value = defaultValue;
            }

            return value;
        }

        protected string GetAppSettingValue(string name)
        {
            return ConfigurationManager.AppSettings[name]; //這邊不可做任何轉空字串, 會影響後續引用處的行為改變
        }

        protected List<string> GetAppSettingValues(string name, char separator = ';')
        {
            return GetAppSettingValue(name).ToNonNullString()
                .Split(separator)
                .Where(w => !w.IsNullOrEmpty()).ToList();
        }

        public EnvironmentCode GetEnvironmentCode()
        {
            string environmentCodeValue = GetAppSettingValue("Environment");

            if (environmentCodeValue.IsNullOrEmpty())
            {
                return EnvironmentCode.Production;
            }

            EnvironmentCode environmentCode = EnvironmentCode.GetSingle(environmentCodeValue);

            if (environmentCode == null)
            {
                throw new ArgumentOutOfRangeException("EnvironmentCode not found");
            }

            return environmentCode;
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
    }

    public abstract class BaseGameAppSettingService : IGameAppSettingService
    {
        #region IM相關設定

        protected abstract string IMMerchantCodeConfigKey { get; }

        protected abstract string IMServiceUrlConfigKey { get; }

        protected abstract string IMLanguageConfigKey { get; }

        protected abstract string IMCurrencyConfigKey { get; }

        protected abstract string IMPPMerchantCodeConfigKey { get; }

        protected abstract string IMPPServiceUrlConfigKey { get; }

        protected abstract string IMPPLanguageConfigKey { get; }

        protected abstract string IMPPCurrencyConfigKey { get; }

        protected abstract string IMPTMerchantCodeConfigKey { get; }

        protected abstract string IMPTServiceUrlConfigKey { get; }

        protected abstract string IMPTLanguageConfigKey { get; }

        protected abstract string IMPTCurrencyConfigKey { get; }

        protected abstract string IMSportMerchantCodeConfigKey { get; }

        protected abstract string IMSportServiceUrlConfigKey { get; }

        protected abstract string IMSportLanguageConfigKey { get; }

        protected abstract string IMSportCurrencyConfigKey { get; }

        protected abstract string IMeBETMerchantCodeConfigKey { get; }

        protected abstract string IMeBETServiceUrlConfigKey { get; }

        protected abstract string IMeBETLanguageConfigKey { get; }

        protected abstract string IMeBETCurrencyConfigKey { get; }

        protected abstract string IMBGMerchantCodeConfigKey { get; }

        protected abstract string IMBGServiceUrlConfigKey { get; }

        protected abstract string IMBGMD5KeyConfigKey { get; }

        protected abstract string IMBGDesKeyConfigKey { get; }

        #endregion IM相關設定

        #region AG設定

        protected abstract string AGLoginBaseUrlConfigKey { get; }

        protected abstract string AGServiceBaseUrlConfigKey { get; }

        protected abstract string AGVendorIDConfigKey { get; }

        protected abstract string AGMD5KeyConfigKey { get; }

        protected abstract string AGDesKeyConfigKey { get; }

        protected abstract string AGCurrencyConfigKey { get; }

        protected abstract string AGOddsTypeConfigKey { get; }

        protected abstract string AGAcTypeConfigKey { get; }

        protected abstract string AGDMConfigKey { get; }

        #endregion AG設定

        #region LC設定

        protected abstract string LCMerchantCodeConfigKey { get; }

        protected abstract string LCServiceUrlConfigKey { get; }

        protected abstract string LCMD5KeyConfigKey { get; }

        protected abstract string LCDesKeyConfigKey { get; }

        protected abstract string LCLinecodeConfigKey { get; }

        #endregion LC設定

        #region Sport設定

        protected abstract string SportMerchantCodeConfigKey { get; }

        protected abstract string SportServiceUrlConfigKey { get; }

        protected abstract string SportLoginBaseUrlConfigKey { get; }

        protected abstract string SportCurrencyConfigKey { get; }

        protected abstract string SportOddsTypeConfigKey { get; }

        #endregion Sport設定

        public IIMOneAppSetting GetIMAppSetting()
        {
            return new IMAppSetting()
            {
                MerchantCodeConfigKey = IMMerchantCodeConfigKey,
                ServiceUrlConfigKey = IMServiceUrlConfigKey,
                LanguageConfigKey = IMLanguageConfigKey,
                CurrencyConfigKey = IMCurrencyConfigKey
            };
        }

        public IIMOneAppSetting GetIMPPAppSetting()
        {
            return new IMPPAppSetting()
            {
                MerchantCodeConfigKey = IMPPMerchantCodeConfigKey,
                ServiceUrlConfigKey = IMPPServiceUrlConfigKey,
                LanguageConfigKey = IMPPLanguageConfigKey,
                CurrencyConfigKey = IMPPCurrencyConfigKey
            };
        }

        public IIMOneAppSetting GetIMPTAppSetting()
        {
            return new IMPTAppSetting()
            {
                MerchantCodeConfigKey = IMPTMerchantCodeConfigKey,
                ServiceUrlConfigKey = IMPTServiceUrlConfigKey,
                LanguageConfigKey = IMPTLanguageConfigKey,
                CurrencyConfigKey = IMPTCurrencyConfigKey
            };
        }

        public IIMOneAppSetting GetIMSportAppSetting()
        {
            return new IMSportAppSetting()
            {
                MerchantCodeConfigKey = IMSportMerchantCodeConfigKey,
                ServiceUrlConfigKey = IMSportServiceUrlConfigKey,
                LanguageConfigKey = IMSportLanguageConfigKey,
                CurrencyConfigKey = IMSportCurrencyConfigKey
            };
        }

        public IIMOneAppSetting GetIMeBETAppSetting()
        {
            return new IMeBETAppSetting()
            {
                MerchantCodeConfigKey = IMeBETMerchantCodeConfigKey,
                ServiceUrlConfigKey = IMeBETServiceUrlConfigKey,
                LanguageConfigKey = IMeBETLanguageConfigKey,
                CurrencyConfigKey = IMeBETCurrencyConfigKey
            };
        }

        public IIMBGAppSetting GetIMBGAppSetting()
        {
            return new IMBGAppSetting()
            {
                MerchantCodeConfigKey = IMBGMerchantCodeConfigKey,
                ServiceUrlConfigKey = IMBGServiceUrlConfigKey,
                MD5KeyConfigKey = IMBGMD5KeyConfigKey,
                DESKeyConfigKey = IMBGDesKeyConfigKey
            };
        }

        public IAGAppSetting GetAGAppSetting()
        {
            return new AGAppSetting()
            {
                LoginBaseUrlConfigKey = AGLoginBaseUrlConfigKey,
                ServiceBaseUrlConfigKey = AGServiceBaseUrlConfigKey,
                VendorIDConfigKey = AGVendorIDConfigKey,
                CurrencyConfigKey = AGCurrencyConfigKey,
                OddsTypeConfigKey = AGOddsTypeConfigKey,
                AcTypeConfigKey = AGAcTypeConfigKey,
                DMConfigKey = AGDMConfigKey,
                MD5KeyConfigKey = AGMD5KeyConfigKey,
                DesKeyConfigKey = AGDesKeyConfigKey,
            };
        }

        public ILCAppSetting GetLCAppSetting()
        {
            return new LCAppSetting()
            {
                MerchantCodeConfigKey = LCMerchantCodeConfigKey,
                ServiceUrlConfigKey = LCServiceUrlConfigKey,
                MD5KeyConfigKey = LCMD5KeyConfigKey,
                DESKeyConfigKey = LCDesKeyConfigKey,
                LinecodeConfigKey = LCLinecodeConfigKey
            };
        }

        public ISportAppSetting GetSportAppSetting()
        {
            return new SportAppSetting()
            {
                MerchantCodeConfigKey = SportMerchantCodeConfigKey,
                ServiceUrlConfigKey = SportServiceUrlConfigKey,
                LoginBaseUrlConfigKey = SportLoginBaseUrlConfigKey,
                CurrencyConfigKey = SportCurrencyConfigKey,
                OddsTypeConfigKey = SportOddsTypeConfigKey,
            };
        }
    }
}