using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Config;
using JxBackendService.Model.Common;
using JxBackendService.Model.Common.IMOne;
using JxBackendService.Model.Enums;
using JxBackendService.Model.MessageQueue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Service.Base
{
    public abstract class BaseAppSettingService : IAppSettingService
    {
        private string _masterInloDbConnectionString;

        private string _slaveInloDbConnectionString;

        private string _masterIMsgConnectionString;

        private string _slaveIMsgConnectionString;

        private string _masterMimiConnectionString;

        private string _slaveMimiConnectionString;

        private string _redisConnectionString;

        private static readonly Lazy<IConfigUtilService> s_configUtilService = DependencyUtil.ResolveService<IConfigUtilService>();

        public IConfigUtilService ConfigUtilService => s_configUtilService.Value;

        protected abstract string MasterInloDbConnectionStringConfigKey { get; }

        protected abstract string SlaveInloDbBakConnectionStringConfigKey { get; }

        private static readonly string s_masterIMsgConnectionStringConfigKey = "Master_IMsg_ConnectionString";

        private static readonly string s_slaveIMsgBakConnectionStringConfigKey = "Slave_IMsgBak_ConnectionString";

        private static readonly string s_masterMimiConnectionStringConfigKey = "Master_Mimi_ConnectionString";

        private static readonly string s_slaveMimiBakConnectionStringConfigKey = "Slave_MimiBak_ConnectionString";

        private static readonly string s_endUserRabbitMQWebSocketSettingsConfigKey = "EndUser.RabbitMQWebSocketSettings";

        private static readonly string s_endUserRabbitMQConnectionsConfigKey = "EndUser.RabbitMQConnections";

        private static readonly string s_internalRabbitMQConnectionsConfigKey = "Internal.RabbitMQConnections";

        private static readonly string s_isEnabledMethodInvocationLogConfigKey = "IsEnabledMethodInvocationLog";

        protected BaseAppSettingService()
        {
        }

        public string GetConnectionString(DbConnectionTypes dbConnectionType)
        {
            string connectionString;

            switch (dbConnectionType)
            {
                case DbConnectionTypes.Master:
                    connectionString = GetMasterInloDbConnectionString();
                    break;

                case DbConnectionTypes.Slave:
                    connectionString = GetSlaveInloDbBakConnectionString();

                    break;

                case DbConnectionTypes.IMsgMaster:
                    connectionString = GetMasterIMsgConnectionString();

                    break;

                case DbConnectionTypes.IMsgSlave:
                    connectionString = GetSlaveIMsgBakConnectionString();

                    break;

                case DbConnectionTypes.MimiMaster:
                    connectionString = GetMasterMimiConnectionString();

                    break;

                case DbConnectionTypes.MimiSlave:
                    connectionString = GetSlaveMimiBakConnectionString();

                    break;

                default:
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

            string paramString = $",poolsize=2000,preheat=false,idleTimeout=15000,ssl=false,writeBuffer=20480,defaultDatabase={(int)dbIndex}";

            return _redisConnectionString + paramString;
        }

        public List<RabbitMQWebSocketSetting> GetEndUserRabbitMQWebSocketSettings()
        {
            List<RabbitMQWebSocketSetting> settings = ConfigUtilService.Get<List<RabbitMQWebSocketSetting>>(s_endUserRabbitMQWebSocketSettingsConfigKey);

            settings.ForEach(setting =>
            {
                if (setting.VirtualHost.IsNullOrEmpty())
                {
                    setting.VirtualHost = "/";
                }
            });

            return settings;
        }

        public List<RabbitMQSetting> GetEndUserRabbitMQSettings()
        {
            return ConfigUtilService.Get<List<RabbitMQSetting>>(s_endUserRabbitMQConnectionsConfigKey);
        }

        public List<RabbitMQSetting> GetInternalRabbitMQSettings()
        {
            return ConfigUtilService.Get<List<RabbitMQSetting>>(s_internalRabbitMQConnectionsConfigKey);
        }

        public bool IsEnabledMethodInvocationLog
        {
            get
            {
                return GetAppSettingValue(s_isEnabledMethodInvocationLogConfigKey) == "1";
            }
        }

        public abstract string CommonDataHash { get; }

        public virtual int? MinWorkerThreads => null;

        protected string GetAppSettingValue(string name)
        {
            string value = ConfigUtilService.Get(name);

            return value; //這邊不可做任何轉空字串, 會影響後續引用處的行為改變
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
                return DecryptConnectionStringFromAppSetting(MasterInloDbConnectionStringConfigKey);
            });

            return _masterInloDbConnectionString;
        }

        private string GetSlaveInloDbBakConnectionString()
        {
            _slaveInloDbConnectionString = _slaveInloDbConnectionString.GetAssignValueOnce(() =>
            {
                return DecryptConnectionStringFromAppSetting(SlaveInloDbBakConnectionStringConfigKey);
            });

            return _slaveInloDbConnectionString;
        }

        private string GetMasterIMsgConnectionString()
        {
            _masterIMsgConnectionString = _masterIMsgConnectionString.GetAssignValueOnce(() =>
            {
                return DecryptConnectionStringFromAppSetting(s_masterIMsgConnectionStringConfigKey);
            });

            return _masterIMsgConnectionString;
        }

        private string GetSlaveIMsgBakConnectionString()
        {
            _slaveIMsgConnectionString = _slaveIMsgConnectionString.GetAssignValueOnce(() =>
            {
                return DecryptConnectionStringFromAppSetting(s_slaveIMsgBakConnectionStringConfigKey);
            });

            return _slaveIMsgConnectionString;
        }

        private string GetMasterMimiConnectionString()
        {
            _masterMimiConnectionString = _masterMimiConnectionString.GetAssignValueOnce(() =>
            {
                return DecryptConnectionStringFromAppSetting(s_masterMimiConnectionStringConfigKey);
            });

            return _masterMimiConnectionString;
        }

        private string GetSlaveMimiBakConnectionString()
        {
            _slaveMimiConnectionString = _slaveMimiConnectionString.GetAssignValueOnce(() =>
            {
                return DecryptConnectionStringFromAppSetting(s_slaveMimiBakConnectionStringConfigKey);
            });

            return _slaveMimiConnectionString;
        }

        private string DecryptConnectionStringFromAppSetting(string configKey)
        {
            string encryptConnectionString = GetAppSettingValue(configKey);

            if (encryptConnectionString.IsNullOrEmpty())
            {
                throw new ArgumentNullException($"appsetting key={configKey}, value is null");
            }

            string connectionString = CommUtil.CommUtil.DecryptDES(encryptConnectionString);
            connectionString = AppendTrustServerCertificateNX(connectionString);

            return connectionString;
        }

        private string AppendTrustServerCertificateNX(string connectionString)
        {
            if (connectionString.IndexOf("TrustServerCertificate", StringComparison.CurrentCultureIgnoreCase) >= 0)
            {
                return connectionString;
            }

            var fullConnectionString = new StringBuilder(connectionString);

            if (!connectionString.EndsWith(";"))
            {
                fullConnectionString.Append(";");
            }

            fullConnectionString.Append("TrustServerCertificate=true");

            return fullConnectionString.ToString();
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