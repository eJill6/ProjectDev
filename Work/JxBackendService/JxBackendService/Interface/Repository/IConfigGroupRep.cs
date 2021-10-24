using JxBackendService.Model.Entity.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Interface.Repository
{
    public interface IConfigGroupRep
    {
        int GetGroupSerial(string groupName);
        ConfigGroup GetSingle(string groupName);
    }
}
