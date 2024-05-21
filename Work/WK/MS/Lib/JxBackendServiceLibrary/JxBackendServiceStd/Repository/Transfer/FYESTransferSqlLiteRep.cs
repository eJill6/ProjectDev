using JxBackendService.Model.Enums;
using JxBackendService.Model.ThirdParty.Base;
using JxBackendService.Repository.Base;

namespace JxBackendService.Repository.Transfer
{
    public class FYESTransferSqlLiteRep : BaseTransferJsonFormatSqlLiteRepository
    {
        public FYESTransferSqlLiteRep()
        { }

        public override PlatformProduct Product => PlatformProduct.FYES;
    }
}