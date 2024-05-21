using JxBackendService.Model.Enums;
using JxBackendService.Repository.Base;

namespace JxBackendService.Repository.Transfer
{
    public class IMBGTransferSqlLiteRep : BaseTransferSqlLiteRepository
    {
        public IMBGTransferSqlLiteRep()
        { }

        public override PlatformProduct Product => PlatformProduct.IMBG;
    }
}