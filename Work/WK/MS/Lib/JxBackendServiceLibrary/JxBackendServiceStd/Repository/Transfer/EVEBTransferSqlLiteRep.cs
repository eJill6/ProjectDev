using JxBackendService.Model.Enums;
using JxBackendService.Model.ThirdParty.EVO;
using JxBackendService.Repository.Base;

namespace JxBackendService.Repository.Transfer
{
    public class EVEBTransferSqlLiteRep : BaseTransferSqlLiteRepository
    {
        public EVEBTransferSqlLiteRep()
        { }

        public override PlatformProduct Product => PlatformProduct.EVEB;
    }
}