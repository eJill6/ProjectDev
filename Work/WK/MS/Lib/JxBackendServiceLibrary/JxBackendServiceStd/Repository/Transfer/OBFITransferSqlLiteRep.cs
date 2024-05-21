using JxBackendService.Model.Enums;
using JxBackendService.Model.ThirdParty.PM.Base;
using JxBackendService.Repository.Base;

namespace JxBackendService.Repository.Transfer
{
    public class OBFITransferSqlLiteRep : BaseTransferSqlLiteRepository
    {
        public OBFITransferSqlLiteRep()
        { }

        public override PlatformProduct Product => PlatformProduct.OBFI;
    }
}