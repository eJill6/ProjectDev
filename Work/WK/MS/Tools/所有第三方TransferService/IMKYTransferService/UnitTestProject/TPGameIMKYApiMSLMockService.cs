using JxBackendService.Common.Util;
using JxBackendService.Model.Common.IMOne;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ThirdParty.IM;
using JxBackendService.Model.ThirdParty.IM.Lottery;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Service.ThirdPartyTransfer;

namespace UnitTestProject
{
    public class TPGameIMKYApiMSLMockService : TPGameIMKYApiService
    {
        public TPGameIMKYApiMSLMockService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        public override bool IsBackupBetLog => false;

        protected override BaseReturnDataModel<RequestAndResponse> GetRemoteBetLogApiResult(string lastSearchToken)
        {
            return base.GetRemoteBetLogApiResult(lastSearchToken);
            //return MockDataUtil.GetRemoteBetLogApiResult(lastSearchToken);
        }
    }

    public class MockDataUtil
    {
        public static BaseReturnDataModel<RequestAndResponse> GetRemoteBetLogApiResult(string lastSearchToken)
        {
            var model = new IMBaseResponseModel();

            return new BaseReturnDataModel<RequestAndResponse>(ReturnCode.Success, new RequestAndResponse()
            {
                RequestBody = "1",
                ResponseContent = new string[] { model.ToJsonString() }.ToJsonString()
            });
        }

        public static DetailRequestAndResponse GetRemoteTransferApiResult(bool isMoneyIn, string tpGameAccount, BaseTPGameMoneyInfo tpGameMoneyInfo)
        {
            string amountText = tpGameMoneyInfo.Amount.ToString("0.####");

            var request = new IMTransferRequestModel
            {
                MerchantCode = IMKYSharedAppSetting.Instance.MerchantCode,
                ProductWallet = IMKYSharedAppSetting.Instance.ProductWallet,
                PlayerId = tpGameAccount,
                Amount = amountText,
                TransactionId = tpGameMoneyInfo.OrderID,
            };

            var webRequestParam = new WebRequestParam
            {
                Url = $"http://test.transfer.{PlatformProduct.IMKY.Value}",
                Body = request.ToJsonString(),
            };

            string apiResult = new IMTransferResponseModel
            {
                Code = "510",
                Status = "Declined",
                Message = "Insufficient amount."
            }.ToJsonString();

            return new DetailRequestAndResponse(webRequestParam, apiResult);
        }
    }
}