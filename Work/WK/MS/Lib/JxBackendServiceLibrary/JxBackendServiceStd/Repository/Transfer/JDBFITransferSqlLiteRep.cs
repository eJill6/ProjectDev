using JxBackendService.Model.Enums;
using JxBackendService.Model.ThirdParty.Base;
using JxBackendService.Repository.Base;

namespace JxBackendService.Repository.Transfer
{
    public class JDBFITransferSqlLiteRep : BaseTransferJsonFormatSqlLiteRepository
    {
        public JDBFITransferSqlLiteRep()
        { }

        public override PlatformProduct Product => PlatformProduct.JDBFI;
    }
}