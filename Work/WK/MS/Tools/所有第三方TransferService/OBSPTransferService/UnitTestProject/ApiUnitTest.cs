using JxBackendService.Common.Util;
using JxBackendService.Model.Common;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ThirdParty.OB.OBSP;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Net;

namespace UnitTestProject
{
    [TestClass]
    public class ApiUnitTest
    {
        [TestMethod]
        public void TestCreateUserApi()
        {
            var request = new OBSPCreateUserRequest()
            {
                MerchantCode = OBSPSharedAppSetting.MerchantCode,
                Key = OBSPSharedAppSetting.Key,
                Currency = OBSPSharedAppSetting.Currency,
                UserName = "jxD_69778",
            };

            string requestBody = request.ToRequestBody();
            BaseReturnDataModel<string> returnModel = DoPostRequest(OBSPSharedAppSetting.UserCreateActionUrl, requestBody);
            System.Diagnostics.Debug.WriteLine(returnModel.ToJsonString());
        }

        [TestMethod]
        public void TestCheckBalanceApi()
        {
            var request = new OBSPUserBasicRequest()
            {
                MerchantCode = OBSPSharedAppSetting.MerchantCode,
                Key = OBSPSharedAppSetting.Key,
                UserName = "jxD_69778",
            };

            string requestBody = request.ToRequestBody();
            BaseReturnDataModel<string> returnModel = DoPostRequest(OBSPSharedAppSetting.FundCheckBalanceActionUrl, requestBody);
            System.Diagnostics.Debug.WriteLine(returnModel.ToJsonString());
        }

        
        [TestMethod]
        public void TestTransferApi()
        {
            var request = new OBSPCreateTransferRequest()
            {
                MerchantCode = OBSPSharedAppSetting.MerchantCode,
                Key = OBSPSharedAppSetting.Key,
                TransferId = "123456789",
                UserName = "jxD_69778",
                TransferType = OBSPTransferType.Deposit,
                Amount = "100"                
            };

            string requestBody = request.ToRequestBody();
            BaseReturnDataModel<string> returnModel = DoPostRequest(OBSPSharedAppSetting.FundTransferActionUrl, requestBody);
            System.Diagnostics.Debug.WriteLine(returnModel.ToJsonString());
        }

        [TestMethod]
        public void TestUserLoginApi()
        {
            //pc or mobile
            string terminal = "pc";

            var createUserRequest = new OBSPLoginUserRequest()
            {
                MerchantCode = OBSPSharedAppSetting.MerchantCode,
                Key = OBSPSharedAppSetting.Key,
                Currency = OBSPSharedAppSetting.Currency,
                UserName = "jxD_69778",
                Terminal = terminal
            };

            BaseReturnDataModel<string> returnModel = DoPostRequest(OBSPSharedAppSetting.UserLoginActionUrl, createUserRequest.ToRequestBody());
            System.Diagnostics.Debug.WriteLine(returnModel.ToJsonString());
        }

        [TestMethod]
        public void TestGetTransferRecordApi()
        {
            var request = new OBSPGetTransferRecordRequest()
            {
                MerchantCode = OBSPSharedAppSetting.MerchantCode,
                Key = OBSPSharedAppSetting.Key,
                TransferId = "123456789",
                UserName = "jxD_69778",
            };

            string requestBody = request.ToRequestBody();
            BaseReturnDataModel<string> returnModel = DoPostRequest(OBSPSharedAppSetting.FundGetTransferRecordActionUrl, requestBody);
            System.Diagnostics.Debug.WriteLine(returnModel.ToJsonString());
        }

        /// <summary>
        /// 打第三方API
        /// </summary>
        private BaseReturnDataModel<string> DoPostRequest(string relativeUrl, string requestBody)
        {
            string fullUrl = GetCombineUrl(OBSPSharedAppSetting.ApiRootUrl, relativeUrl);

            string result = HttpWebRequestUtil.GetResponse(
                new WebRequestParam()
                {
                    Purpose = $"TPGService Test API",
                    Method = HttpMethod.Post,
                    Url = fullUrl,
                    Body = requestBody,
                    ContentType = HttpWebRequestContentType.WwwFormUrlencoded,
                    IsResponseValidJson = true,
                },
            out HttpStatusCode httpStatusCode);

            return new BaseReturnDataModel<string>()
            {
                IsSuccess = httpStatusCode == HttpStatusCode.OK,
                DataModel = result
            };
        }

        protected string GetCombineUrl(params string[] urls)
        {
            return string.Join("/", urls.Select(s => s.TrimStart("/").TrimEnd("/")));
        }
    }
}
