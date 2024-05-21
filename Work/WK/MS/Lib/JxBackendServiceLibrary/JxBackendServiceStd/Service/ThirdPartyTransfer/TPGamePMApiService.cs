using JxBackendService.Common;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Model.Common;
using JxBackendService.Interface.Service.Net;
using JxBackendService.Model.Common.PM;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Param.ThirdParty;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ThirdParty.PM.Base;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Service.ThirdPartyTransfer.Base;
using System;
using System.Net;
using System.Text;

namespace JxBackendService.Service.ThirdPartyTransfer
{
    public abstract class TPGamePMApiService : BaseTPGameApiService
    {
        public static readonly int MaxSearchRangeMinutes = 30;

        private static readonly int s_centUnit = 100;

        public abstract IPMAppSetting AppSetting { get; }

        public static int CentUnit => s_centUnit;

        protected override bool IsAllowTransferCompensation => true;

        public TPGamePMApiService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        protected override string CreateTPGamePasswordByRule(int userId, string tpGameAccount) => tpGameAccount;

        /// <summary>
        /// 轉帳
        /// </summary>
        public override DetailRequestAndResponse GetRemoteTransferApiResult(bool isMoneyIn, CreateRemoteAccountParam createRemoteAccountParam, BaseTPGameMoneyInfo tpGameMoneyInfo)
        {
            var request = new PMTransferRequestModel
            {
                MemberId = createRemoteAccountParam.TPGameAccount,
                MemberName = createRemoteAccountParam.TPGameAccount,
                MemberPwd = createRemoteAccountParam.TPGamePassword,
                MemberIp = "127.0.0.1",
                Money = Convert.ToInt32(tpGameMoneyInfo.Amount * CentUnit),//單位為分
                OrderId = tpGameMoneyInfo.OrderID
            };

            BaseReturnDataModel<string> returnModel = DoPostRequest(isMoneyIn ? BasePMAppSetting.TransferInUrl : BasePMAppSetting.TransferOutUrl,
                request.ToJsonString(),
                out DetailRequestAndResponse detail);

            return detail;
        }

        /// <summary>
        /// 查詢訂單
        /// </summary>
        protected override DetailRequestAndResponse GetRemoteOrderApiResult(string tpGameAccount, BaseTPGameMoneyInfo tpGameMoneyInfo)
        {
            var request = new PMCheckTransferRequestModel
            {
                OrderId = tpGameMoneyInfo.OrderID
            };

            BaseReturnDataModel<string> returnModel = DoPostRequest(BasePMAppSetting.CheckTransferUrl, request.ToJsonString(),
                out DetailRequestAndResponse detail);

            return detail;
        }

        /// <summary>
        /// 查詢餘額
        /// </summary>
        public override string GetRemoteUserScoreApiResult(CreateRemoteAccountParam createRemoteAccountParam, IInvocationUserParam invocationUserParam)
        {
            var request = new PMBaseUserInfoRequestModel
            {
                MemberId = createRemoteAccountParam.TPGameAccount,
                MemberPwd = createRemoteAccountParam.TPGamePassword,
                MemberIp = "127.0.0.1"
            };

            BaseReturnDataModel<string> returnModel = DoPostRequest(BasePMAppSetting.GetBalanceUrl, request.ToJsonString(), out DetailRequestAndResponse detail);

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

            var request = new PMBetLogRequestModel
            {
                BeginTime = Convert.ToInt32(searchStartDate.ToUnixOfTime() / 1000),
                EndTime = Convert.ToInt32(searchEndDate.ToUnixOfTime() / 1000),
                PageNum = 1,
                PageSize = 10000
            };

            string requestBody = request.ToJsonString();

            BaseReturnDataModel<string> returnModel = DoPostRequest(AppSetting.GrabServiceUrl, BasePMAppSetting.GetBetLogUrl, requestBody,
                out DetailRequestAndResponse detail);
            var returnDataModel = returnModel.CastByCodeAndMessage<RequestAndResponse>();

            returnDataModel.DataModel = new RequestAndResponse()
            {
                RequestBody = requestBody,
                ResponseContent = returnModel.DataModel
            };

            return returnDataModel;
        }

        /// <summary>
        /// 處理轉帳回傳結果
        /// </summary>
        public override BaseReturnDataModel<UserScore> GetTransferReturnModel(string apiResult)
        {
            PMTransferResponseModel transferModel = apiResult.Deserialize<PMTransferResponseModel>();

            if (transferModel.Code == PMResponseCode.Success)
            {
                return new BaseReturnDataModel<UserScore>(ReturnCode.Success);
            }

            return new BaseReturnDataModel<UserScore>(transferModel.Msg, null);
        }

        /// <summary>
        /// 處理查詢訂單回傳結果
        /// </summary>
        public override BaseReturnModel GetQueryOrderReturnModel(string apiResult)
        {
            PMQueryOrderResponseModel transferModel = apiResult.Deserialize<PMQueryOrderResponseModel>();

            if (transferModel.Code == PMResponseCode.Success &&
                transferModel.Data != null &&
                transferModel.Data.Status == PMTransferOrderStatus.Success.Value)
            {
                return new BaseReturnModel(ReturnCode.Success);
            }

            return new BaseReturnModel(transferModel.Msg);
        }

        /// <summary>
        /// 處理取得餘額回傳結果
        /// </summary>
        public override BaseReturnDataModel<UserScore> GetUserScoreReturnModel(string apiResult)
        {
            PMBalanceResponseModel userScoreModel = apiResult.Deserialize<PMBalanceResponseModel>();

            if (userScoreModel.Code == PMResponseCode.Success)
            {
                //單位 分
                return new BaseReturnDataModel<UserScore>(ReturnCode.Success,
                    new UserScore()
                    {
                        AvailableScores = userScoreModel.Data.Balance / CentUnit
                    });
            }

            return new BaseReturnDataModel<UserScore>(userScoreModel.Msg, null);
        }

        /// <summary>
        /// 檢查以及創建帳號
        /// </summary>
        protected override BaseReturnModel CheckOrCreateRemoteAccount(CreateRemoteAccountParam createRemoteAccountParam)
        {
            //創帳號同啟動遊戲
            BaseReturnDataModel<string> returnModel = GetRemoteCreateAccountApiResult(createRemoteAccountParam);

            if (returnModel.IsSuccess)
            {
                PMLunchGameResponseModel registerModel = returnModel.DataModel.Deserialize<PMLunchGameResponseModel>();
                //檢查帳號重複同一隻API
                if (registerModel.Code == PMResponseCode.Success)
                {
                    return new BaseReturnModel(ReturnCode.Success);
                }

                return new BaseReturnModel(registerModel.Msg);
            }

            return new BaseReturnModel(returnModel.Message);
        }

        /// <summary>
        /// 取得遊戲入口網址
        /// </summary>
        protected override BaseReturnDataModel<string> GetRemoteForwardGameUrl(TPGameRemoteLoginParam tpGameRemoteLoginParam)
        {
            BaseReturnDataModel<string> returnModel = GetRemoteLoginApiResult(tpGameRemoteLoginParam);

            if (!returnModel.IsSuccess)
            {
                return new BaseReturnDataModel<string>(returnModel.Message, string.Empty);
            }

            PMLunchGameResponseModel launchGameModel = returnModel.DataModel.Deserialize<PMLunchGameResponseModel>();

            if (launchGameModel.Code == PMResponseCode.Success)
            {
                string launchGameUrl = ProcessLaunchGameUrl(launchGameModel.Data);

                return new BaseReturnDataModel<string>(ReturnCode.Success, launchGameUrl);
            }

            return new BaseReturnDataModel<string>(launchGameModel.Msg);
        }

        protected override BaseReturnDataModel<string> GetRemoteLoginApiResult(TPGameRemoteLoginParam tpGameRemoteLoginParam)
        {
            //IPV6格式會不符合
            if (tpGameRemoteLoginParam.IpAddress.Contains(":"))
            {
                tpGameRemoteLoginParam.IpAddress = "127.0.0.1";
            }

            int gameId = AppSetting.GameId;

            if (!tpGameRemoteLoginParam.LoginInfo.RemoteCode.IsNullOrEmpty())
            {
                gameId = Convert.ToInt32(tpGameRemoteLoginParam.LoginInfo.RemoteCode);
            }

            var request = new PMLunchGameRequestModel
            {
                MemberId = tpGameRemoteLoginParam.CreateRemoteAccountParam.TPGameAccount,
                MemberName = tpGameRemoteLoginParam.CreateRemoteAccountParam.TPGameAccount,
                MemberPwd = tpGameRemoteLoginParam.CreateRemoteAccountParam.TPGamePassword,
                DeviceType = tpGameRemoteLoginParam.IsMobile ? 2 : 0,
                MemberIp = tpGameRemoteLoginParam.IpAddress,
                GameId = gameId
            };

            BaseReturnDataModel<string> returnModel = DoPostRequest(AppSetting.LaunchGameUrl, request.ToJsonString(), out DetailRequestAndResponse detail);

            return returnModel;
        }

        protected override void DoKickUser(Model.Entity.ThirdPartyUserAccount thirdPartyUserAccount) => new NotImplementedException();

        protected virtual string ProcessLaunchGameUrl(string launchGameUrl) => launchGameUrl; // 父類不進行額外處理

        protected override BaseReturnDataModel<string> GetRemoteCheckAccountExistApiResult(CreateRemoteAccountParam createRemoteAccountParam)
        {
            //無此API
            throw new NotSupportedException();
        }

        protected override BaseReturnDataModel<string> GetRemoteCreateAccountApiResult(CreateRemoteAccountParam createRemoteAccountParam)
        {
            var request = new PMLunchGameRequestModel
            {
                MemberId = createRemoteAccountParam.TPGameAccount,
                MemberName = createRemoteAccountParam.TPGameAccount,
                MemberPwd = createRemoteAccountParam.TPGameAccount,
                DeviceType = 0,
                MemberIp = "127.0.0.1",
                GameId = AppSetting.GameId
            };

            return DoPostRequest(BasePMAppSetting.CreateAccountUrl, request.ToJsonString(), out DetailRequestAndResponse detail);
        }

        protected override bool IsDoTransferCompensation(string apiResult)
        {
            var transferModel = apiResult.Deserialize<PMTransferResponseModel>();

            if (transferModel.Code == PMResponseCode.NoWithdrawalInGame)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 打第三方API
        /// </summary>
        private BaseReturnDataModel<string> DoPostRequest(string relativeUrl, string requestBody, out DetailRequestAndResponse detail)
        {
            return DoPostRequest(AppSetting.ServiceUrl, relativeUrl, requestBody, out detail);
        }

        /// <summary>
        /// 打第三方API，加密簽名方法
        /// </summary>
        private BaseReturnDataModel<string> DoPostRequest(string url, string relativeUrl, string requestBody, out DetailRequestAndResponse detail)
        {
            long unixOfTime = DateTime.Now.ToUnixOfTime() / 1000;

            //轉帳要特殊處理 訂單號時間戳與url時間戳要一致
            if (relativeUrl == "transferIn" || relativeUrl == "transferOut")
            {
                var data = JsonUtil.Deserialize<PMTransferRequestModel>(requestBody);

                try
                {
                    string orderId = data.OrderId.Split(':')[1];
                    unixOfTime = long.Parse(orderId) / 1000;
                }
                catch (Exception e)
                {
                    throw new Exception("訂單格式校驗失敗," + e.ToString());
                }
            }

            url = GenerateApiPath(GetFullUrl(url, relativeUrl), AppSetting.MerchantCode, AppSetting.SecretKey, unixOfTime);
            string postString = AESTool.AesEncrypt(requestBody, AppSetting.SecretKey, AppSetting.IV);

            var httpWebRequestUtilService = DependencyUtil.ResolveService<IHttpWebRequestUtilService>().Value;
            var webRequestParam = new WebRequestParam()
            {
                Purpose = $"TPGService.{Product.Value}請求",
                Method = HttpMethod.Post,
                Url = url,
                Body = postString,
                ContentType = HttpWebRequestContentType.TextPlain,
                IsResponseValidJson = true
            };

            string apiResult = httpWebRequestUtilService.GetResponse(webRequestParam, out HttpStatusCode httpStatusCode);
            detail = new DetailRequestAndResponse(webRequestParam, apiResult);

            return ConvertToApiReturnDataModel(httpStatusCode, apiResult);
        }

        private string GenerateApiPath(string host, string Agent, string Md5Key, long timeStamp)
        {
            Random random = new Random();
            string text = random.Next(10000, 99999).ToString();
            string text2 = random.Next(10000, 99999).ToString();
            string value = text + text2;
            string value2 = GenerateSign(Agent, Md5Key, timeStamp);

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(host);
            stringBuilder.Append("?agent=").Append(Agent);
            stringBuilder.Append("&timestamp=").Append(timeStamp);
            stringBuilder.Append("&randno=").Append(value);
            stringBuilder.Append("&sign=").Append(value2);

            return stringBuilder.ToString();
        }

        private string GenerateSign(string agent, string Md5Key, long timeStamp)
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

        private string GetCode(int num)
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
    }
}