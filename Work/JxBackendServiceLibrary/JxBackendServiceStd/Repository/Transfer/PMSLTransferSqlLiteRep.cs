﻿using JxBackendService.Model.Enums;
using JxBackendService.Model.ThirdParty.PM.Base;
using JxBackendService.Repository.Base;

namespace JxBackendService.Repository.Transfer
{
    public class PMSLTransferSqlLiteRep : BaseTransferJsonFormatSqlLiteRepository<PMBetLog>
    {
        public PMSLTransferSqlLiteRep()
        { }

        public override PlatformProduct Product => PlatformProduct.PMSL;

        public override string ProfitlossTableName => "PMSLProfitLossInfo";
    }
}