using JxBackendService.Model.Enums;
using JxBackendService.Model.ThirdParty.Base;
using JxBackendService.Repository.Base;

namespace JxBackendService.Repository.Transfer
{
    public class FYESTransferSqlLiteRep : BaseTransferJsonFormatSqlLiteRepository<JsonRemoteBetLog>
    {
        public FYESTransferSqlLiteRep()
        { }

        public override PlatformProduct Product => PlatformProduct.FYES;

        public override string ProfitlossTableName => "FYESProfitLossInfo";
    }
}