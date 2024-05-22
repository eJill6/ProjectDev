

using JxBackendService.Model.Enums;
using JxBackendServiceNF.Service.ThirdPartyTransfer.Base;

namespace ProductTransferService
{
    public partial class ProductTransferScheduleService : BaseABTransferScheduleService
    {
        public override PlatformProduct Product => PlatformProduct.ABEB;

        public override JxApplication Application => JxApplication.ABEBTransferService;
    }
}
