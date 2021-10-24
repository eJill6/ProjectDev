using JxBackendService.Interface.Repository;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Entity.Config;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository;
using JxBackendService.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Service.Config
{
    public class ConfigService : BaseService, IConfigService
    {
        private readonly IConfigGroupRep _configGroupRep;
        private readonly IConfigSettingsRep _configSettingsRep;


        public ConfigService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _configGroupRep = ResolveJxBackendService<IConfigGroupRep>();
            _configSettingsRep = ResolveJxBackendService<IConfigSettingsRep>();
        }

        /// <summary>
        /// 取得單一設定群組
        /// </summary>
        public ConfigGroup GetSingleConfigGroup(ConfigGroupNameEnum groupName)
        {
            return _configGroupRep.GetSingle(groupName.ToString());
        }

        /// <summary>
        /// 取得群組序號
        /// </summary>
        public int GetGetGroupSerial(ConfigGroupNameEnum groupName)
        {
            return _configGroupRep.GetGroupSerial(groupName.ToString());
        }

        /// <summary>
        /// 取得某類型的所有明細
        /// </summary>
        public List<ConfigSettings> GetConfigSettingsList(ConfigGroupNameEnum groupName, bool? isActive = null)
        {
            return _configSettingsRep.GetList(groupName.ToString(), isActive);
        }

        /// <summary>
        /// 取得單一設定明細
        /// </summary> 
        public ConfigSettings GetSingleConfigSettings(ConfigGroupNameEnum groupName, int itemKey)
        {
            return _configSettingsRep.GetSingle(groupName.ToString(), itemKey);
        }

        public bool IsActive(ConfigGroupNameEnum groupName)
        {
            return IsActive(groupName, 1);
        }

        /// <summary>
        /// 取得設定是否啟用
        /// </summary> 
        public bool IsActive(ConfigGroupNameEnum groupName, int itemKey)
        {
            return _configSettingsRep.IsActive(groupName, itemKey);
        }
    }
}
