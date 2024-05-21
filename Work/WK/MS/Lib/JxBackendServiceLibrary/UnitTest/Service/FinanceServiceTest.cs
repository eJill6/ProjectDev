using System;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Finance;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTest.Base;

namespace UnitTest.Util
{
    [TestClass]
    public class RechargeServiceTest : BaseTest
    {
        private readonly Lazy<IRechargeService> _rechargeService;

        private readonly Lazy<IWithdrawService> _withdrawService;

        public RechargeServiceTest()
        {
            _rechargeService = DependencyUtil.ResolveJxBackendService<IRechargeService>(EnvLoginUser, DbConnectionTypes.Master);
            _withdrawService = DependencyUtil.ResolveJxBackendService<IWithdrawService>(EnvLoginUser, DbConnectionTypes.Master);
        }

        [TestMethod]
        public void RecheckOrdersFromMiseLive()
        {
            _rechargeService.Value.RecheckOrdersFromMiseLive();
        }

        [TestMethod]
        public void DoWithdrawTest()
        {
            BaseReturnModel returnModel = _withdrawService.Value.WithdrawToMiseLive(100, "IMKY");
            Assert.IsTrue(returnModel.IsSuccess);

            returnModel = _withdrawService.Value.WithdrawToMiseLive(decimal.MaxValue, "IMKY");
            Assert.IsFalse(returnModel.IsSuccess);
        }

        [TestMethod]
        public void RecheckWithdrawOrdersFromMiseLiveTest()
        {
            _withdrawService.Value.RecheckWithdrawOrdersFromMiseLive();
        }
    }
}