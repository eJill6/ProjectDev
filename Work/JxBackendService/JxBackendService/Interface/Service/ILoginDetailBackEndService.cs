using JxBackendService.Model.Param.User;
using JxBackendService.Model.ReturnModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Interface.Service
{
    public interface ILoginDetailBackEndService
    {
        BaseReturnDataModel<long> InsertLoginDetail(InsertLoginDetailBackendParam param);
    }
}
