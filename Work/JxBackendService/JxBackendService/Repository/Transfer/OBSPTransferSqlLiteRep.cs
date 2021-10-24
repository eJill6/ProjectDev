using JxBackendService.Common.Util;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ThirdParty.BTI;
using JxBackendService.Model.ThirdParty.OB.OBSP;
using JxBackendService.Repository.Base;
using System;
using System.Collections.Generic;

namespace JxBackendService.Repository.Transfer
{
    public class OBSPTransferSqlLiteRep : BaseTransferJsonFormatSqlLiteRepository<OBSPBetLog>
    {
        public OBSPTransferSqlLiteRep() { }

        public override PlatformProduct Product => PlatformProduct.OBSP;

        public override string ProfitlossTableName => "OBSPProfitLossInfo";
    }
}
