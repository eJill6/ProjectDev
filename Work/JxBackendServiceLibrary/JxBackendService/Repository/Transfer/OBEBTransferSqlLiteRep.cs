using JxBackendService.Model.Enums;
using JxBackendService.Model.ThirdParty.OB.OBEB;
using JxBackendService.Repository.Base;

namespace JxBackendService.Repository.Transfer
{
    public class OBEBTransferSqlLiteRep : BaseTransferJsonFormatSqlLiteRepository<BetRecordLog>
    {
        public OBEBTransferSqlLiteRep() { }

        public override PlatformProduct Product => PlatformProduct.OBEB;

        public override string ProfitlossTableName => "OBEBProfitLossInfo";
    }
}
