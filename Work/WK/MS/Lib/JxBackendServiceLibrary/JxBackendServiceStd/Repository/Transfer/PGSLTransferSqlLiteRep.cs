using JxBackendService.Model.Enums;
using JxBackendService.Model.ThirdParty.PG;
using JxBackendService.Repository.Base;

namespace JxBackendService.Repository.Transfer
{
    public class PGSLTransferSqlLiteRep : BaseTransferSqlLiteRepository
    {
        public PGSLTransferSqlLiteRep()
        { }

        public override PlatformProduct Product => PlatformProduct.PGSL;
    }
}