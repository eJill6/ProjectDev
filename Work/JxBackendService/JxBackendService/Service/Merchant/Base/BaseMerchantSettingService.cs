using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Finance.Apollo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Service.Merchant.Base
{
    public class BaseMerchantSettingService
    {
        protected ApolloConfigParam GetApolloConfigParam(JxApplication jxApplication)
        {
            IAppSettingService appSettingService = DependencyUtil.ResolveKeyed<IAppSettingService>(jxApplication, SharedAppSettings.PlatformMerchant);
            return appSettingService.GetApolloConfigParam();
        }
    }
}
