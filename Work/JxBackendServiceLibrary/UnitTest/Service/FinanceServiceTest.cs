using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Model.MiseLive.Request;
using JxBackendService.Interface.Service.Finance;
using JxBackendService.Interface.Service.MiseLive;
using JxBackendService.Model.Enums;
using JxBackendService.Model.MiseLive.Request;
using JxBackendService.Model.MiseLive.Response;
using JxBackendService.Model.ReturnModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using UnitTest.Base;

namespace UnitTest.Util
{
    [TestClass]
    public class RechargeServiceTest : BaseTest
    {
        private readonly IRechargeService _rechargeService;

        private readonly IWithdrawService _withdrawService;

        public RechargeServiceTest()
        {
            _rechargeService = DependencyUtil.ResolveJxBackendService<IRechargeService>(EnvLoginUser, DbConnectionTypes.Master);
            _withdrawService = DependencyUtil.ResolveJxBackendService<IWithdrawService>(EnvLoginUser, DbConnectionTypes.Master);
        }

        [TestMethod]
        public void RecheckOrdersFromMiseLive()
        {
            _rechargeService.RecheckOrdersFromMiseLive();
        }

        [TestMethod]
        public void DoWithdrawTest()
        {
            BaseReturnModel returnModel = _withdrawService.WithdrawToMiseLive(100, "IMKY");
            Assert.IsTrue(returnModel.IsSuccess);

            returnModel = _withdrawService.WithdrawToMiseLive(decimal.MaxValue, "IMKY");
            Assert.IsFalse(returnModel.IsSuccess);
        }

        [TestMethod]
        public void RecheckWithdrawOrdersFromMiseLiveTest()
        {
            _withdrawService.RecheckWithdrawOrdersFromMiseLive();
        }
    }
}