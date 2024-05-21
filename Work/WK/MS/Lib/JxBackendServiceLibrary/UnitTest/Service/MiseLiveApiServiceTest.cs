using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Model.MiseLive.Request;
using JxBackendService.Interface.Service.MiseLive;
using JxBackendService.Interface.Service.Util;
using JxBackendService.Model.MiseLive.Request;
using JxBackendService.Model.MiseLive.Response;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using UnitTest.Base;

namespace UnitTest.Util
{
    [TestClass]
    public class MiseLiveApiServiceTest : BaseTest
    {
        private readonly Lazy<IMiseLiveApiService> _miseLiveApiService;

        public MiseLiveApiServiceTest()
        {
            _miseLiveApiService = DependencyUtil.ResolveService<IMiseLiveApiService>();
        }

        [TestMethod]
        public void BalanceTest()
        {
            IMiseLiveUserBalanceRequestParam requestParam = new MiseLiveUserBalanceRequestParam()
            {
                UserId = 6251
            };

            MiseLiveResponse<MiseLiveBalance> response = _miseLiveApiService.Value.GetUserBalance(requestParam);

            DependencyUtil.ResolveService<ILogUtilService>().Value.ForcedDebug(response.ToJsonString());
        }

        [TestMethod]
        public void TransferInTest()
        {
            IMiseLiveTransferRequestParam requestParam = new MiseLiveTransferRequestParam()
            {
                UserId = 6251,
                Amount = 123.00m,
                OrderNo = "12312312323"
            };

            MiseLiveResponse<MiseLiveBalance> response = _miseLiveApiService.Value.TransferIn(requestParam);

            DependencyUtil.ResolveService<ILogUtilService>().Value.ForcedDebug(response.ToJsonString());
        }

        [TestMethod]
        public void TransferOutTest()
        {
            IMiseLiveTransferRequestParam requestParam = new MiseLiveTransferRequestParam()
            {
                UserId = 6251,
                Amount = 123.12m,
                OrderNo = "12312312324"
            };

            MiseLiveResponse<MiseLiveBalance> response = _miseLiveApiService.Value.TransferOut(requestParam);

            DependencyUtil.ResolveService<ILogUtilService>().Value.ForcedDebug(response.ToJsonString());
        }

        [TestMethod]
        public void TransferResultTest()
        {
            IMiseLiveTransferOrderRequestParam requestParam = new MiseLiveTransferOrderRequestParam()
            {
                OrderNo = "12312312324"
            };

            MiseLiveResponse<MiseLiveTransferOrder> response = _miseLiveApiService.Value.GetTransferOrderResult(requestParam);

            DependencyUtil.ResolveService<ILogUtilService>().Value.ForcedDebug(response.ToJsonString());
        }
    }
}