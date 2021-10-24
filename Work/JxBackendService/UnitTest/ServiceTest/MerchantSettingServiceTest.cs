using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Merchant;
using JxBackendService.Interface.Service.VIP;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.User;
using JxBackendService.Model.ViewModel.VIP;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using UnitTest.Base;

namespace UnitTest.ServiceTest
{
    [TestClass]
    public class MerchantSettingServiceTest : BaseTest
    {


        [TestMethod]
        public void GetApolloSetting()
        {
            var merchantSettingService = DependencyUtil.ResolveKeyed<IMerchantSettingService>(SharedAppSettings.PlatformMerchant);
            var setting = merchantSettingService.GetApolloSetting(JxApplication.FrontSideWeb);
        }
    }
}

