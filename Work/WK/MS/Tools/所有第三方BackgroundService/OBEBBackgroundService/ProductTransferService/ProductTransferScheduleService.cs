using JxBackendService.Model.Enums;
using JxBackendServiceN6.Service.ThirdPartyTransfer.Base;

namespace ProductTransferService
{
    public partial class ProductTransferScheduleService : BaseOBEBTransferScheduleService
    {
        protected override PlatformProduct Product => PlatformProduct.OBEB;

        protected override Type MainBackgroundServiceType => typeof(ProductTransferScheduleService);
    }
}