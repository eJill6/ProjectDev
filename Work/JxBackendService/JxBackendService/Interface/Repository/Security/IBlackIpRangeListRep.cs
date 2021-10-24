using JxBackendService.Model.Entity.Security;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Interface.Repository.Security
{
    public interface IBlackIpRangeListRep : IBaseDbRepository<BlackIpRangeList>
    {
        bool IsActive(JxIpInformation ipInformation, BlackIpType blackIpType);
    }
}
