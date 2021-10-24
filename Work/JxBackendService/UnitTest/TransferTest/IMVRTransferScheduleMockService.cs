using JxBackendService.Common.Util;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ThirdParty.Base;
using JxBackendService.Model.ThirdParty.IM.Lottery;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Service.ThirdPartyTransfer.Base;
using System;
using System.Collections.Generic;

namespace UnitTest.TransferTest
{
    public class IMVRTransferScheduleMockService : BaseIMLotteryTransferScheduleService
    {
        public override PlatformProduct Product => throw new Exception();

        public override JxApplication Application => JxApplication.IMVRTransferService;
    }
}
