using JxBackendService.Model.Enums;
using JxBackendService.Model.ThirdParty.Base;
using JxBackendService.Repository.Base;

namespace JxBackendService.Repository.Transfer
{
    public class JDBFITransferSqlLiteRep : BaseTransferJsonFormatSqlLiteRepository<JsonRemoteBetLog>
    {
        public JDBFITransferSqlLiteRep()
        { }

        public override PlatformProduct Product => PlatformProduct.JDBFI;

        public override string ProfitlossTableName => "JDBFIProfitLossInfo";
    }
}