using JxBackendService.Model.Enums;
using JxBackendService.Model.ThirdParty.Base;
using JxBackendService.Repository.Base;

namespace JxBackendService.Repository.Transfer
{
    public class IMKYTransferSqlLiteRep : BaseTransferJsonFormatSqlLiteRepository<JsonRemoteBetLog>
    {
        public IMKYTransferSqlLiteRep()
        { }

        public override PlatformProduct Product => PlatformProduct.IMKY;

        public override string ProfitlossTableName => "IMKYProfitLossInfo";
    }
}