using JxBackendService.Model.Enums;
using JxBackendService.Service.ThirdPartyTransfer.Base;
using JxBackendServiceNF.Service.ThirdPartyTransfer.Base;

namespace ProductTransferService
{
    public partial class ProductTransferScheduleService : BaseEVTransferScheduleService
    {
        public override PlatformProduct Product => PlatformProduct.EVEB;

        public override JxApplication Application => JxApplication.EVEBTransferService;
    }
}
