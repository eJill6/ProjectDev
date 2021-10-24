using JxBackendService.Model.Entity.Config;
using JxBackendService.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Interface.Repository
{
    public interface IConfigSettingsRep
    {
        List<ConfigSettings> GetList(string groupName, bool? isActive = null);
        
        ConfigSettings GetSingle(string groupName, int itemKey);
        
        bool IsActive(ConfigGroupNameEnum groupName, int itemKey);
    }
}
