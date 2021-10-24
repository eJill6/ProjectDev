using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Model.ViewModel.ThirdParty.Old;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.Enums.ThirdParty
{
    public enum TransferJobTypes
    {
        TransferIn,
        TransferOut,
        RecheckProcessingOrders
    }

    public class TransferParamJob
    {
        public Guid Identity { get; set; } = Guid.NewGuid();

        public TransferJobTypes JobType { get; set; }
        
        public BaseTPGameMoneyInfo TPGameMoneyInfo { get; set; }
    }

    public class OldTransferParamJob<ApiParamType>
    {
        public Guid Identity { get; set; } = Guid.NewGuid();

        public TransferJobTypes JobType { get; set; }

        public OldTPGameOrderParam<ApiParamType> OldTPGameOrderParam { get; set; }
    }
    
}
