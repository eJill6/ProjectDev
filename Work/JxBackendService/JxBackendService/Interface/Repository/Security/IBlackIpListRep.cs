using JxBackendService.Model.Entity.Security;
using JxBackendService.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Interface.Repository.Security
{
    public interface IBlackIpListRep : IBaseDbRepository<BlackIpList>
    {
        BlackIpList GetSingle(string ipAddress, BlackIpType blackIpType);
    }
}
