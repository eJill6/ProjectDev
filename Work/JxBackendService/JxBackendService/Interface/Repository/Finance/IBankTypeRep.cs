using JxBackendService.Model.Common;
using JxBackendService.Model.Entity.Finance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Interface.Repository.Finance
{
    public interface IBankTypeRep
    {
        List<BankType> GetVisibleList();
    }
}
