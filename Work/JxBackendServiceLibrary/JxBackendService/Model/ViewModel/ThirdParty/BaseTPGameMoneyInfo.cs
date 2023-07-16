using JxBackendService.Interface.Model.Common;
using System;

namespace JxBackendService.Model.ViewModel.ThirdParty
{
    public abstract class BaseTPGameMoneyInfo : IInvocationUserParam
    {
        public decimal Amount { get; set; }

        public string OrderID { get; set; }

        public DateTime OrderTime { get; set; }

        public string Handle { get; set; }

        public DateTime? HandTime { get; set; }

        public int UserID { get; set; }

        public string UserName { get; set; }

        public short Status { get; set; }

        public string Memo { get; set; }

        public string CorrelationId { get; set; }

        public abstract string GetMoneyID();

        public abstract string GetPrimaryKeyColumnName();
    }
}