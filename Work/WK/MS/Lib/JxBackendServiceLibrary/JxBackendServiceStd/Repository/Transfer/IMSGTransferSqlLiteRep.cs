using JxBackendService.Model.Enums;
using JxBackendService.Model.ThirdParty.Base;
using JxBackendService.Repository.Base;

namespace JxBackendService.Repository.Transfer
{
    public class IMSGTransferSqlLiteRep : BaseTransferSqlLiteRepository
    {
        public IMSGTransferSqlLiteRep()
        { }

        public override PlatformProduct Product => PlatformProduct.IMSG;
    }
}