using JxBackendService.Interface.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.ViewModel.ThirdParty
{
    public class ProcessSuccessOrderParam
    {
        public bool IsMoneyIn { get; set; }

        public BaseTPGameMoneyInfo TPGameMoneyInfo { get; set; }

        public UserScore RemoteUserScore { get; set; }

        public bool IsSynchronizing { get; set; }
    }

    public class TPGameTranfserParam : IInvocationUserParam
    {
        public decimal Amount { get; set; }

        public bool IsSynchronizing { get; set; }

        public bool IsOperateByBackSide { get; set; }

        public int UserID { get; set; }

        public string CorrelationId { get; set; }
    }
}