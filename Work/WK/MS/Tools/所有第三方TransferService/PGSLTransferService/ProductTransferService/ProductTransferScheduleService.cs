using JxBackendService.Model.Enums;
using JxBackendService.Service.ThirdPartyTransfer.Base;
using JxBackendServiceNF.Service.ThirdPartyTransfer.Base;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceProcess;
using System.Threading;

namespace ProductTransferService
{
    public partial class ProductTransferScheduleService : BasePGTransferScheduleService
    {
        public override PlatformProduct Product => PlatformProduct.PGSL;

        public override JxApplication Application => JxApplication.PGSLTransferService;
    }
}
