using JxBackendService.Model.Entity.Config;
using JxBackendService.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Interface.Service
{
    public interface IConfigService
    {
        List<ConfigSettings> GetConfigSettingsList(ConfigGroupNameEnum groupName, bool? isActive = null);

        int GetGetGroupSerial(ConfigGroupNameEnum groupName);

        ConfigGroup GetSingleConfigGroup(ConfigGroupNameEnum groupName);

        ConfigSettings GetSingleConfigSettings(ConfigGroupNameEnum groupName, int itemKey);

        bool IsActive(ConfigGroupNameEnum groupName, int itemKey);

        bool IsActive(ConfigGroupNameEnum groupName);
    }
}
