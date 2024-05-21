using JxBackendService.Model.Enums;
using JxBackendService.Model.ThirdParty.AllBet;
using JxBackendService.Repository.Base;

namespace JxBackendService.Repository.Transfer
{
    public class ABEBTransferSqlLiteRep : BaseTransferJsonFormatSqlLiteRepository
    {
        public ABEBTransferSqlLiteRep()
        { }

        public override PlatformProduct Product => PlatformProduct.ABEB;
    }
}