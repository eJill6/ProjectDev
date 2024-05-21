using JxBackendService.Interface.Model.GlobalSystem;
using JxBackendService.Model.ReturnModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JxBackendService.Interface.Service.GlobalSystem
{
    public interface IMethodInvocationLogService
    {
        BaseReturnDataModel<long> Create(IInsertMethodInvocationLogParam param);
    }
}