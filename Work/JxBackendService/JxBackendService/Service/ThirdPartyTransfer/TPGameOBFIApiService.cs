using JxBackendService.Common;
using JxBackendService.Common.Util;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Param.ThirdParty;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ThirdParty.OB.OBFI;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Service.ThirdPartyTransfer.Base;
using System;
using System.Net;
using System.Text;

namespace JxBackendService.Service.ThirdPartyTransfer
{
    public class TPGameOBFIApiService : BaseTPGameApiService
    {
        public static readonly int MaxSearchRangeMinutes = 30;

        public OBFISharedAppSettings AppSettings => OBFISharedAppSettings.Instance;

        public override PlatformProduct Product => PlatformProduct.OBFI;

        public TPGameOBFIApiService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {

        }

        /// <summary>
        /// 轉帳
        /// </summary>
        protected override string GetRemoteTransferApiResult(bool isMoneyIn, string tpGameAccount, BaseTPGameMoneyInfo tpGameMoneyInfo)
        {
            var request = new OBFITransferRequestModel
            {
                memberId = tpGameAccount,
                memberName = tpGameAccount,
                memberPwd = tpGameAccount,
                memberIp = "127.0.0.1",
                money = Convert.ToInt32(tpGameMoneyInfo.Amount * 100),//單位為分
                orderId = tpGameMoneyInfo.OrderID
            };
            BaseReturnDataModel<string> returnModel = DoPostRequest(isMoneyIn ? AppSettings.TransferInUrl : AppSettings.TransferOutUrl, request.ToJsonString());
            return returnModel.DataModel;
        }

        /// <summary>
        /// 查詢訂單
        /// </summary>
        protected override string GetRemoteOrderApiResult(string tpGameAccount, BaseTPGameMoneyInfo tpGameMoneyInfo)
        {
            var request = new OBFICheckTransferRequestModel
            {
                orderId = tpGameMoneyInfo.OrderID
            };

            BaseReturnDataModel<string> returnModel = DoPostRequest(AppSettings.CheckTransferUrl, request.ToJsonString());
            return returnModel.DataModel;
        }

        /// <summary>
        /// 查詢餘額
        /// </summary>
        protected override string GetRemoteUserScoreApiResult(string tpGameAccount)
        {
            var request = new OBFIBaseUserInfoRequestModel
            {
                memberId = tpGameAccount,
                memberPwd = tpGameAccount,
                memberIp = "127.0.0.1"
            };

            var returnModel = DoPostRequest(AppSettings.GetBalanceUrl, request.ToJsonString());
            return returnModel.DataModel;
        }

        /// <summary>
        /// 取得盈虧資料
        /// </summary>
        protected override BaseReturnDataModel<RequestAndResponse> GetRemoteBetLogApiResult(string lastSearchToken)
        {
            DateTime searchStartDate = lastSearchToken.ToInt64().ToDateTime();
            DateTime searchEndDate = searchStartDate.AddMinutes(MaxSearchRangeMinutes);

            //不能跨日
            if (searchStartDate.Day != searchEndDate.Day)
            {
                searchEndDate = searchEndDate.Date.AddSeconds(-1);
            }

            if (searchEndDate > DateTime.Now)
            {
                searchEndDate = DateTime.Now;
            }

            var request = new OBFIBetLogRequestModel
            {
                beginTime = Convert.ToInt32(searchStartDate.ToUnixOfTime() / 1000),
                endTime = Convert.ToInt32(searchEndDate.ToUnixOfTime() / 1000),
                pageNum = 1,
                pageSize = 10000
            };

            string requestBody = request.ToJsonString();

            BaseReturnDataModel<string> returnModel = DoPostRequest(AppSettings.GrabServiceUrl, AppSettings.GetBetLogUrl, requestBody);
            return new BaseReturnDataModel<RequestAndResponse>(ReturnCode.GetSingle(returnModel.Code),
                new RequestAndResponse()
                {
                    RequestBody = requestBody,
                    ResponseContent = returnModel.DataModel
                });
        }

        /// <summary>
        /// 處理轉帳回傳結果
        /// </summary>
        public override BaseReturnDataModel<UserScore> GetTransferReturnModel(string apiResult)
        {
            OBFITransferResponseModel transferModel = apiResult.Deserialize<OBFITransferResponseModel>();

            if (transferModel.code == OBResponseCode.Success)
            {
                return new BaseReturnDataModel<UserScore>(ReturnCode.Success);
            }

            return new BaseReturnDataModel<UserScore>(transferModel.msg, null);
        }

        /// <summary>
        /// 處理查詢訂單回傳結果
        /// </summary>
        public override BaseReturnModel GetQueryOrderReturnModel(string apiResult)
        {
            OBFIQueryOrderResponseModel transferModel = apiResult.Deserialize<OBFIQueryOrderResponseModel>();

            if (transferModel.code == OBResponseCode.Success && transferModel.data != null && transferModel.data.status == OBTransferOrderStatus.Success.Value)
            {
                return new BaseReturnDataModel<UserScore>(ReturnCode.Success);
            }

            return new BaseReturnDataModel<UserScore>(transferModel.msg, null);
        }

        /// <summary>
        /// 處理取得餘額回傳結果
        /// </summary>
        public override BaseReturnDataModel<UserScore> GetUserScoreReturnModel(string apiResult)
        {
            OBFIBalanceResponseModel userScoreModel = apiResult.Deserialize<OBFIBalanceResponseModel>();

            if (userScoreModel.code == OBResponseCode.Success)
            {
                //單位 分
                return new BaseReturnDataModel<UserScore>(ReturnCode.Success, new UserScore() { AvailableScores = (decimal)userScoreModel.data.balance / 100 });
            }

            return new BaseReturnDataModel<UserScore>(userScoreModel.msg, null);
        }

        /// <summary>
        /// 檢查以及創建帳號
        /// </summary>
        protected override BaseReturnModel CheckOrCreateRemoteAccount(CreateRemoteAccountParam param)
        {
            //創帳號同啟動遊戲
            BaseReturnDataModel<string> returnModel = GetRemoteCreateAccountApiResult(param);

            if (returnModel.IsSuccess)
            {
                OBFILunchGameResponseModel registerModel = returnModel.DataModel.Deserialize<OBFILunchGameResponseModel>();
                //檢查帳號重複同一隻API
                if (registerModel.code == OBResponseCode.Success)
                {
                    return new BaseReturnModel(ReturnCode.Success);
                }

                return new BaseReturnModel(registerModel.msg);
            }

            return new BaseReturnModel(returnModel.Message);
        }

        /// <summary>
        /// 取得遊戲入口網址
        /// </summary>
        protected override BaseReturnDataModel<string> GetRemoteForwardGameUrl(string tpGameAccount, string ip, bool isMobile)
        {
            //IPV6格式會不符合
            if (ip.Contains(":"))
            {
                ip = "127.0.0.1";
            }

            var request = new OBFILunchGameRequestModel
            {
                memberId = tpGameAccount,
                memberName = tpGameAccount,
                memberPwd = tpGameAccount,
                deviceType = isMobile ? 2 : 0,
                memberIp = ip,
                gameId = 200
            };

            var returnModel = DoPostRequest(AppSettings.CreateAccountUrl, request.ToJsonString());

            if (returnModel.IsSuccess)
            {
                OBFILunchGameResponseModel launchGameModel = returnModel.DataModel.Deserialize<OBFILunchGameResponseModel>();

                if (launchGameModel.code == OBResponseCode.Success)
                {
                    return new BaseReturnDataModel<string>(ReturnCode.Success, launchGameModel.data);
                }

                return new BaseReturnDataModel<string>(launchGameModel.msg, string.Empty);
            }

            return new BaseReturnDataModel<string>(returnModel.Message, string.Empty);
        }

        /// <summary>
        /// 打第三方API
        /// </summary>
        private BaseReturnDataModel<string> DoPostRequest(string relativeUrl, string requestBody)
        {
            return DoPostRequest(AppSettings.ServiceUrl, relativeUrl, requestBody);
        }

        /// <summary>
        /// 打第三方API，加密簽名方法
        /// </summary>
        private BaseReturnDataModel<string> DoPostRequest(string url, string relativeUrl, string requestBody)
        {
            long unixOfTime = DateTime.Now.ToUnixOfTime() / 1000;
            //轉帳要特殊處理 訂單號時間戳與url時間戳要一致
            if (relativeUrl == "transferIn" || relativeUrl == "transferOut")
            {
                var data = JsonUtil.Deserialize<OBFITransferRequestModel>(requestBody);
                try
                {
                    string orderId = data.orderId.Split(':')[1];
                    unixOfTime = long.Parse(orderId) / 1000;
                }
                catch (Exception e)
                {
                    throw new Exception("訂單格式校驗失敗," + e.ToString());
                }
            }

            url = GenerateApiPath(GetFullUrl(url, relativeUrl), AppSettings.MerchantCode, AppSettings.SecretKey, unixOfTime);
            string postString = AESTool.AesEncrypt(requestBody, AppSettings.SecretKey, AppSettings.IV);

            string result = HttpWebRequestUtil.GetResponse(
                new WebRequestParam()
                {
                    Purpose = $"TPGService.{Product.Value}請求",
                    Method = HttpMethod.Post,
                    Url = url,
                    Body = postString,
                    ContentType = HttpWebRequestContentType.TextPlain,
                    IsResponseValidJson = true
                },
            out HttpStatusCode httpStatusCode);

            return new BaseReturnDataModel<string>()
            {
                IsSuccess = httpStatusCode == HttpStatusCode.OK,
                DataModel = result
            };
        }

        public string GenerateApiPath(string Host, string Agent, string Md5Key, long timeStamp)
        {
            Random random = new Random();
            string text = random.Next(10000, 99999).ToString();
            string text2 = random.Next(10000, 99999).ToString();
            string value = text + text2;
            string value2 = GenerateSign(Host, Agent, Md5Key, timeStamp);
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(Host);
            stringBuilder.Append("?agent=").Append(Agent);
            stringBuilder.Append("&timestamp=").Append(timeStamp);
            stringBuilder.Append("&randno=").Append(value);
            stringBuilder.Append("&sign=").Append(value2);
            return stringBuilder.ToString();
        }

        public string GenerateSign(string Host, string agent, string Md5Key, long timeStamp)
        {
            string text = MD5Tool.MD5EncodingForOBGameProvider(agent + timeStamp + Md5Key);
            string text2 = text.Substring(0, 9);
            string text3 = text.Substring(9, 8);
            string text4 = text.Substring(17);
            string code = GetCode(2);
            string code2 = GetCode(2);
            string code3 = GetCode(2);
            string code4 = GetCode(2);
            return code + text2 + code2 + text3 + code3 + text4 + code4;
        }

        public string GetCode(int num)
        {
            string text = "1,2,3,4,5,6,7,8,9,A,B,C,D,E,F,G,H,I,J,K,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z,a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z";
            string[] array = text.Split(new char[1] { ',' });
            string text2 = "";
            Random random = new Random();
            for (int i = 0; i < num; i++)
            {
                int num2 = random.Next(60);
                text2 += array[num2];
            }
            return text2;
        }

        protected override BaseReturnDataModel<string> GetRemoteCheckAccountExistApiResult(string tpGameAccount)
        {
            //無此API
            throw new NotSupportedException();
        }

        protected override BaseReturnDataModel<string> GetRemoteCreateAccountApiResult(CreateRemoteAccountParam param)
        {
            var request = new OBFILunchGameRequestModel
            {
                memberId = param.TPGameAccount,
                memberName = param.TPGameAccount,
                memberPwd = param.TPGameAccount,
                deviceType = 0,
                memberIp = "127.0.0.1",
                gameId = 200
            };

            return DoPostRequest(AppSettings.CreateAccountUrl, request.ToJsonString());
        }
    }
}
