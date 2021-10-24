using JxBackendService.Interface.Service.Enums.Product;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Service.Enums.Product
{
    public class BasePlatformProductSettingCTLService : IPlatformProductSettingService
    {
        public virtual bool IsParseUserIdFromSuffix => true;
    }

    public class PlatformProductAGSettingCTLService : BasePlatformProductSettingCTLService, IPlatformProductSettingService
    {
        public override bool IsParseUserIdFromSuffix => false;
    }

    public class PlatformProductSportSettingCTLService : BasePlatformProductSettingCTLService, IPlatformProductSettingService
    {
        public override bool IsParseUserIdFromSuffix => false;
    }

    public class PlatformProductPTSettingCTLService : BasePlatformProductSettingCTLService, IPlatformProductSettingService
    {
        public override bool IsParseUserIdFromSuffix => false;
    }

    public class PlatformProductLCSettingCTLService : BasePlatformProductSettingCTLService, IPlatformProductSettingService
    {
        public override bool IsParseUserIdFromSuffix => false;
    }

    public class PlatformProductRGSettingCTLService : BasePlatformProductSettingCTLService, IPlatformProductSettingService
    {
        public override bool IsParseUserIdFromSuffix => false;
    }

    public class BasePlatformProductSettingCTSService : IPlatformProductSettingService
    {
        public bool IsParseUserIdFromSuffix => true;
    }
}
