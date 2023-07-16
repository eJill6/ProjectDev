using JxBackendService.Model.Enums;
using JxBackendService.Model.ThirdParty.Base;
using JxBackendService.Repository.Base;

namespace JxBackendService.Repository.Transfer
{
    public class WLBGTransferSqlLiteRep : BaseTransferJsonFormatSqlLiteRepository<JsonRemoteBetLog>
    {
        public WLBGTransferSqlLiteRep()
        { }

        public override PlatformProduct Product => PlatformProduct.WLBG;

        public override string ProfitlossTableName => "WLBGProfitLossInfo";
    }
}