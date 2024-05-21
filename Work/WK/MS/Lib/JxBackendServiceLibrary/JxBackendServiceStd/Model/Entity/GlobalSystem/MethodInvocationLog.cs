using JxBackendService.Model.Attributes;
using System;

namespace JxBackendService.Model.Entity.GlobalSystem
{
    public class BaseMethodInvocationLog
    {
        public string CreateUser { get; set; }

        public DateTime CreateDate { get; set; }

        public string CorrelationId { get; set; }

        public int UserID { get; set; }

        public string MethodName { get; set; }

        public string TypeName { get; set; }

        public decimal ElapsedMilliseconds { get; set; }

        public string ArgumentsJson { get; set; }

        public string ReturnValueJson { get; set; }

        public string ErrorMsg { get; set; }
    }

    public class MethodInvocationLog : BaseMethodInvocationLog
    {
        [ExplicitKey]
        public string SEQID { get; set; }

        public string UpdateUser { get; set; }

        public DateTime UpdateDate { get; set; }
    }
}