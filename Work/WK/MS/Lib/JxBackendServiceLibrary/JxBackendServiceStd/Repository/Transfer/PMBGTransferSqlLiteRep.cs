using JxBackendService.Model.Enums;
using JxBackendService.Model.ThirdParty.PM.Base;
using JxBackendService.Repository.Base;

namespace JxBackendService.Repository.Transfer
{
    public class PMBGTransferSqlLiteRep : BaseTransferJsonFormatSqlLiteRepository
    {
        public PMBGTransferSqlLiteRep()
        { }

        public override PlatformProduct Product => PlatformProduct.PMBG;
    }
}