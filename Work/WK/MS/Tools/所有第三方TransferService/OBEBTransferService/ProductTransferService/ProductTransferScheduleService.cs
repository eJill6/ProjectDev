using JxBackendService.Model.Enums;
using JxBackendService.Service.ThirdPartyTransfer.Base;
using JxBackendServiceNF.Service.ThirdPartyTransfer.Base;

namespace ProductTransferService
{
    public partial class ProductTransferScheduleService : BaseOBEBTransferScheduleService
    {
        public override PlatformProduct Product => PlatformProduct.OBEB;

        public override JxApplication Application => JxApplication.OBEBTransferService;
    }
}
