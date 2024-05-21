using JxBackendService.Model.Enums;
using JxBackendService.Model.ThirdParty.Base;
using JxBackendService.Repository.Base;

namespace JxBackendService.Repository.Transfer
{
    public class AWCSPTransferSqlLiteRep : BaseTransferJsonFormatSqlLiteRepository
    {
        public AWCSPTransferSqlLiteRep()
        { }

        public override PlatformProduct Product => PlatformProduct.AWCSP;
    }
}