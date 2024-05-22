using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Param.ThirdParty;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTestProject
{
    [TestClass]
    public class ThirdPartyApiServiceTest : BaseUnitTest
    {
        private readonly IThirdPartyApiWCFService _thirdPartyApiWCFService;

        private readonly EnvironmentUser _envLoginUser = new EnvironmentUser
        {
            LoginUser = new BasicUserInfo { UserId = 888 },
        };

        public ThirdPartyApiServiceTest()
        {
            _thirdPartyApiWCFService = DependencyUtil.ResolveService<IThirdPartyApiWCFService>();
        }

        [TestMethod]
        public void TestGetForwardGameUrl()
        {
            string correlationId = System.Guid.NewGuid().ToString();

            BaseReturnDataModel<TPGameOpenParam> baseReturnDataModel = _thirdPartyApiWCFService.GetForwardGameUrl(
                PlatformProduct.OBEB.Value,
                loginInfoJson: null,
                isMobile: true,
                correlationId);

            Assert.IsTrue(baseReturnDataModel.IsSuccess);

            baseReturnDataModel = _thirdPartyApiWCFService.GetForwardGameUrl(
                PlatformProduct.IMKY.Value,
                loginInfoJson: null,
                isMobile: true,
                correlationId);

            Assert.IsTrue(baseReturnDataModel.IsSuccess);
        }

        [TestMethod]
        public void TestGetFrontsideMenuViewModel()
        {
            var frontsideMenuService = DependencyUtil.ResolveJxBackendService<IFrontsideMenuService>(_envLoginUser, DbConnectionTypes.Slave);

            frontsideMenuService.GetFrontsideMenuViewModel();
        }
    }
}