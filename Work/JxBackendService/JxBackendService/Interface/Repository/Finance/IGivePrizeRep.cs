using JxBackendService.Model.Param.Finance;
using JxBackendService.Model.ReturnModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Interface.Repository.Finance
{
    public interface IGivePrizeRep
    {
        BaseReturnModel GivePrizesByCustomerType(GivePrizesByCustomerTypeParam param);
    }
}
