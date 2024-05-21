using JxBackendService.Model.Enums;
using JxBackendService.Model.ThirdParty.PM.Base;
using JxBackendService.Repository.Base;

namespace JxBackendService.Repository.Transfer
{
    public class CQ9SLTransferSqlLiteRep : BaseTransferJsonFormatSqlLiteRepository
    {
        public CQ9SLTransferSqlLiteRep()
        { }

        public override PlatformProduct Product => PlatformProduct.CQ9SL;
    }
}