using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Game;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Param.Game;
using JxBackendService.Model.ReturnModel;
using System;
using UnitTestN6;

namespace UnitTest.Service
{
    [TestClass]
    public class LiveGameManageServiceTest : BaseUnitTest
    {
        private readonly Lazy<ILiveGameManageService> _liveGameManageService;

        //private readonly Lazy<ILiveGameManageReadService> _liveGameManageReadService;

        public LiveGameManageServiceTest()
        {
            _liveGameManageService = DependencyUtil.ResolveJxBackendService<ILiveGameManageService>(EnvironmentUser, DbConnectionTypes.Master);
            //_liveGameManageReadService = DependencyUtil.ResolveJxBackendService<ILiveGameManageReadService>(EnvironmentUser, DbConnectionTypes.Slave);
        }

        [TestMethod]
        public void CreateTest()
        {
            BaseReturnModel result = _liveGameManageService.Value.Create(new LiveGameManageCreateParam
            {
                ProductCode = "CQ9SL",
                LotteryType = "CQ9SL",
            });
        }

        [TestMethod]
        public void UpdateTest()
        {
            BaseReturnModel result = _liveGameManageService.Value.Update(new LiveGameManageUpdateParam
            {
            });
        }
    }
}