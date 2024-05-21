using JxBackendService.Model.Enums;
using JxBackendService.Model.ThirdParty.OB.OBSP;
using JxBackendService.Repository.Base;

namespace JxBackendService.Repository.Transfer
{
    public class OBSPTransferSqlLiteRep : BaseTransferJsonFormatSqlLiteRepository
    {
        public OBSPTransferSqlLiteRep()
        { }

        public override PlatformProduct Product => PlatformProduct.OBSP;
    }
}