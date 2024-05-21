using JxBackendService.Model.Enums;
using JxBackendService.Repository.Base;

namespace JxBackendService.Repository.Transfer
{
    public class BTISTransferSqlLiteRep : BaseTransferJsonFormatSqlLiteRepository
    {
        public BTISTransferSqlLiteRep()
        { }

        public override PlatformProduct Product => PlatformProduct.BTIS;
    }
}