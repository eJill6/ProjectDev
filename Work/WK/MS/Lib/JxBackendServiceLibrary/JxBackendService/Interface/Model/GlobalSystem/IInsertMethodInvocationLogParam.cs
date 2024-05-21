using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JxBackendService.Interface.Model.GlobalSystem
{
    public interface IInsertMethodInvocationLogParam
    {
        object Arguments { get; set; }

        string CorrelationId { get; set; }

        decimal ElapsedMilliseconds { get; set; }

        string ErrorMsg { get; set; }

        string MethodName { get; set; }

        object ReturnValue { get; set; }

        int UserID { get; set; }
    }
}