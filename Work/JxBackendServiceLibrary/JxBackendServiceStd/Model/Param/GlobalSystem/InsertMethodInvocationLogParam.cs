using JxBackendService.Interface.Model.GlobalSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JxBackendService.Model.Param.GlobalSystem
{
    public class InsertMethodInvocationLogParam : IInsertMethodInvocationLogParam
    {
        public object Arguments { get; set; }

        public string CorrelationId { get; set; }

        public decimal ElapsedMilliseconds { get; set; }

        public string ErrorMsg { get; set; }

        public string MethodName { get; set; }

        public object ReturnValue { get; set; }

        public int UserID { get; set; }
    }
}