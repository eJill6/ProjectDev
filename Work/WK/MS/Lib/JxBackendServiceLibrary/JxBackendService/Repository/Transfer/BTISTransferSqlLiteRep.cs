using JxBackendService.Model.Enums;
using JxBackendService.Model.ThirdParty.BTI;
using JxBackendService.Repository.Base;

namespace JxBackendService.Repository.Transfer
{
    public class BTISTransferSqlLiteRep : BaseTransferJsonFormatSqlLiteRepository<BTISBetLog>
    {
        public BTISTransferSqlLiteRep() { }

        public override PlatformProduct Product => PlatformProduct.BTIS;

        public override string ProfitlossTableName => "BTISProfitLossInfo";
    }
}
