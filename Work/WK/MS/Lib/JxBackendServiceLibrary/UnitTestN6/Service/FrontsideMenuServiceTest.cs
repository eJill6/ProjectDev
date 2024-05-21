using System;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Enums;
using UnitTestN6;

namespace UnitTest.Util
{
    [TestClass]
    public class FrontsideMenuServiceTest : BaseUnitTest
    {
        private readonly Lazy<IFrontsideMenuService> _frontsideMenuService;

        public FrontsideMenuServiceTest()
        {
            _frontsideMenuService = DependencyUtil.ResolveJxBackendService<IFrontsideMenuService>(EnvironmentUser, DbConnectionTypes.Master);
        }

        [TestMethod]
        public void GetActiveFrontsideMenusTest()
        {
            _frontsideMenuService.Value.GetActiveFrontsideMenus();
            _frontsideMenuService.Value.GetActiveFrontsideMenus(IsForceRefresh: true);
            _frontsideMenuService.Value.GetActiveFrontsideMenus();
        }
    }
}