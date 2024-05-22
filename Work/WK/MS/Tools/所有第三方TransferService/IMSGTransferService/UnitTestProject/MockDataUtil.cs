using JxBackendService.Common.Util;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ThirdParty.IM;
using JxBackendService.Model.ThirdParty.IM.Lottery;
using JxBackendService.Model.ViewModel.ThirdParty;

namespace UnitTestProject
{
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
                MerchantCode = IMSGAppSettings.Instance.MerchantCode,
                ProductWallet = IMSGAppSettings.Instance.ProductWallet,
                PlayerId = tpGameAccount,
                Amount = amountText,
                TransactionId = tpGameMoneyInfo.OrderID,
            };

            var webRequestParam = new WebRequestParam
            {
                Url = $"http://test.transfer.{PlatformProduct.IMSG.Value}",
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
