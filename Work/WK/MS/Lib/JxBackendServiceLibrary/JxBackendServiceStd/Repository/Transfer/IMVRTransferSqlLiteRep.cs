using JxBackendService.Model.Enums;
using JxBackendService.Model.ThirdParty.Base;
using JxBackendService.Repository.Base;

namespace JxBackendService.Repository.Transfer
{
    public class IMVRTransferSqlLiteRep : BaseTransferSqlLiteRepository
    {
        public IMVRTransferSqlLiteRep()
        { }

        public override PlatformProduct Product => PlatformProduct.IMVR;
    }
}