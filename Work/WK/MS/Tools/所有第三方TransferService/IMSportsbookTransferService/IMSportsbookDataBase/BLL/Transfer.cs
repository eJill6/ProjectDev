using IMSportsbookDataBase.Common;
using IMSportsbookDataBase.DLL;
using IMSportsbookDataBase.Enums;
using IMSportsbookDataBase.Model;
using JxBackendService.Common.Util;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ThirdParty.IM.Lottery;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Model.ViewModel.ThirdParty.Old;
using JxBackendService.Service.ThirdPartyTransfer.Old;
using JxBackendServiceNF.Common.Util;
using System;
using System.Collections.Generic;
using System.Threading;

namespace IMSportsbookDataBase.BLL
{
    public class Transfer : OldBaseTPGameApiService<IMSportApiParamModel>
    {
        public Transfer(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) :
            base(envLoginUser, dbConnectionType)
        {
        }

        protected override PlatformProduct Product => PlatformProduct.IMSport;

        /// <summary>
        /// 取得第三方訂單狀態
        /// </summary>
        /// <param name="apiParam"></param>
        /// <returns></returns>
        protected override BaseReturnModel GetRemoteOrderStatus(IMSportApiParamModel apiParam)
        {
            ApiAction tmpActType = apiParam.ActType;
            string apiResult = string.Empty;
            string transferType = apiParam.ActType == ApiAction.Recharge ? "充值" : "提款";

            try
            {
                apiParam.ActType = ApiAction.OrderStatus;

                //確認訂單
                apiResult = ApiClient.CheckTransferStatus(apiParam);

                if (!string.IsNullOrEmpty(apiResult))
                {
                    var baseReturnModel = TPGameApiReadService.GetQueryOrderReturnModel(apiResult);

                    if (baseReturnModel == null || !baseReturnModel.IsSuccess)
                    {
                        LogUtil.ForcedDebug("远程查询订单状态" + transferType + "失败" +
                                "，MoneyID：" + apiParam.MoneyID +
                                "，TransactionId：" + apiParam.TransactionId +
                                "，apiResult：" + apiResult +
                                "，msg (api error code)：" + GetErrorCodeText(apiResult));
                    }

                    return baseReturnModel;
                }
            }
            catch (Exception ex)
            {
                LogsManager.InfoToTelegram("远程确认" + transferType + "失败" +
                    "，billno：" + apiParam.MoneyID +
                    "，datastatus：" + apiResult +
                    "，错误信息：" + ex.Message +
                    "，堆栈信息：" + ex.StackTrace);
            }

            apiParam.ActType = tmpActType;

            return null;
        }

        private string GetErrorCodeText(string apiResult)
        {
            string errorCodeText = null;

            try
            {
                IMTransferResponseModel transferModel = apiResult.Deserialize<IMTransferResponseModel>();
                errorCodeText = EnumHelper<APIErrorCode>.GetEnumDescription(Enum.GetName(typeof(APIErrorCode), transferModel.Code.ToInt32()));
            }
            catch (Exception)
            {
            }

            return errorCodeText;
        }

        /// <summary>
        /// 取得第三方用戶餘額
        /// </summary>
        /// <param name="apiParam"></param>
        /// <returns></returns>
        public override BaseReturnDataModel<UserScore> GetRemoteUserScore(IMSportApiParamModel apiParam)
        {
            IMSportsbookBalanceInfo balanceInfo = GetBalance(apiParam);

            BaseReturnDataModel<UserScore> returnModel = null;

            if (balanceInfo != null &&
                !balanceInfo.Balance.IsNullOrEmpty() &&
                balanceInfo.Code == (int)APIErrorCode.Success)
            {
                returnModel = new BaseReturnDataModel<UserScore>(ReturnCode.Success, new UserScore()
                {
                    AvailableScores = balanceInfo.Balance.ToDecimal(),
                    FreezeScores = 0
                });
            }
            else
            {
                returnModel = new BaseReturnDataModel<UserScore>("Get Remote API Fail", null);
            }

            return returnModel;
        }

        /// <summary>
        /// 获取用户余额，返回结果为-9999表示调用失败
        /// </summary>
        /// <returns></returns>
        public static IMSportsbookBalanceInfo GetBalance(IMSportApiParamModel model)
        {
            IMSportsbookBalanceInfo result = null;
            var tmpActType = model.ActType;

            try
            {
                string apiResult = string.Empty;
                model.ActType = ApiAction.TotalBalance;

                apiResult = ApiClient.GetBalance(model);

                if (!string.IsNullOrEmpty(apiResult))
                {
                    result = apiResult.Deserialize<IMSportsbookBalanceInfo>();
                }
            }
            catch (Exception ex)
            {
                LogsManager.InfoToTelegram("用户" + model.PlayerId + "在 IMSportsbook 平台获取余额失败，错误信息：" + ex.Message + "，堆栈信息：" + ex.StackTrace);
            }

            model.ActType = tmpActType;

            return result;
        }

        /// <summary>
        /// 轉帳單取得成功處理
        /// </summary>
        /// <param name="oldTPGameOrderParam"></param>
        /// <param name="userScore"></param>
        /// <returns></returns>
        protected override BaseReturnModel SaveTransferOrderSuccess(OldTPGameOrderParam<IMSportApiParamModel> oldTPGameOrderParam, UserScore userScore)
        {
            ResultModel resultModel = CycleTryTransferSuccess(
                oldTPGameOrderParam.TPGameMoneyInfo.GetMoneyID(),
                ((int)oldTPGameOrderParam.ApiParam.ActType).ToString(),
                oldTPGameOrderParam.TPGameMoneyInfo.UserID,
                userScore.AvailableScores,
                userScore.FreezeScores);

            if (resultModel.IsSuccess)
            {
                return new BaseReturnModel(ReturnCode.Success);
            }
            else
            {
                return new BaseReturnModel(resultModel.Msg);
            }
        }

        /// <summary>
        /// 循环尝试转账成功时数据处理
        /// </summary>
        /// <param name="transferID"></param>
        /// <param name="actionType"></param>
        private ResultModel CycleTryTransferSuccess(string transferID, string actionType, int userId, decimal availableScores, decimal freezeScores)
        {
            ResultModel result = new ResultModel();
            int tryCount = 0;
            while (tryCount < 5)
            {
                tryCount++;

                try
                {
                    result.IsSuccess = new IMSportsbookMoneyTransferService()
                        .IMSportsbookTransferSuccess(transferID, actionType, userId, availableScores, freezeScores);

                    if (!result.IsSuccess)
                    {
                        result.Info = "-1";
                        result.Msg = "数据库内部异常";
                    }
                }
                catch (Exception ex)
                {
                    result.IsSuccess = false;
                    result.Info = "-1";
                    result.Msg = ex.Message;
                }

                if (result.IsSuccess)
                {
                    break;
                }

                Thread.Sleep(5000);
            }

            return result;
        }

        /// <summary>
        /// 第三方轉帳單取得失敗處理
        /// </summary>
        /// <param name="oldTPGameOrderParam"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        protected override BaseReturnModel SaveTransferOrderFail(OldTPGameOrderParam<IMSportApiParamModel> oldTPGameOrderParam, string errorMsg)
        {
            ResultModel errorHandle = CycleTryTransferRollback(
                oldTPGameOrderParam.TPGameMoneyInfo.GetMoneyID(),
                ((int)oldTPGameOrderParam.ApiParam.ActType).ToString(),
                errorMsg);

            if (errorHandle.IsSuccess)
            {
                return new BaseReturnModel(ReturnCode.Success);
            }
            else
            {
                return new BaseReturnModel(errorHandle.Msg);
            }
        }

        /// <summary>
        /// 循环尝试数据回滚
        /// </summary>
        private ResultModel CycleTryTransferRollback(string transferID, string actionType, string message)
        {
            ResultModel result = new ResultModel();

            int tryCount = 0;
            while (tryCount < 5)
            {
                tryCount++;

                try
                {
                    bool isRollBack = true;
                    result.IsSuccess = new IMSportsbookMoneyTransferService().IMSportsbookTransferRollback(transferID, actionType, message, isRollBack);

                    if (!result.IsSuccess)
                    {
                        result.Info = "-1";
                        result.Msg = "数据库内部异常";
                    }
                }
                catch (Exception ex)
                {
                    result.IsSuccess = false;
                    result.Info = "-1";
                    result.Msg = ex.Message;
                }

                if (result.IsSuccess)
                {
                    break;
                }

                Thread.Sleep(5000);
            }

            return result;
        }

        protected override BaseReturnDataModel<TPGameTransferReturnResult> GetTransferReturnModel(OldTPGameOrderParam<IMSportApiParamModel> oldTPGameOrderParam)
        {
            IMSportApiParamModel model = oldTPGameOrderParam.ApiParam;
            string apiResult = null;
            FundTransferResult fundTransferResult = FundTransferWithRetry(model, ref apiResult);

            var tpGameTransferReturnResult = new TPGameTransferReturnResult()
            {
                ApiResult = apiResult
            };

            string transferType = model.ActType == ApiAction.Recharge ? "充值" : "提款";

            if (fundTransferResult == null)
            {
                string errorMsg = "远程" + transferType + "失败，billno：" + model.MoneyID + "，api retun null.";

                LogUtil.Error(errorMsg);
                return new BaseReturnDataModel<TPGameTransferReturnResult>(errorMsg, tpGameTransferReturnResult);
            }

            if (fundTransferResult.Code == (int)APIErrorCode.Success)
            {
                return new BaseReturnDataModel<TPGameTransferReturnResult>(ReturnCode.Success, tpGameTransferReturnResult);
            }
            else
            {
                string errorCodeText = EnumHelper<APIErrorCode>.GetEnumDescription(Enum.GetName(typeof(APIErrorCode), fundTransferResult.Code));
                string errorMsg = "远程" + transferType + "失败" +
                    "，billno：" + model.MoneyID +
                    "，error_code：" + fundTransferResult.Code + $"({errorCodeText})" +
                    "，datastatus：" + fundTransferResult.ToJsonString();

                LogUtil.Error(errorMsg);
                return new BaseReturnDataModel<TPGameTransferReturnResult>(errorMsg, tpGameTransferReturnResult);
            }
        }

        /// <summary>
        /// 上下分
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private FundTransferResult FundTransferWithRetry(IMSportApiParamModel model, ref string apiResult)
        {
            FundTransferResult result = null;
            apiResult = null;

            int tryCount = 0;
            while (tryCount < 5)
            {
                tryCount++;

                apiResult = ApiClient.Transfer(model);

                if (!string.IsNullOrEmpty(apiResult))
                {
                    result = apiResult.Deserialize<FundTransferResult>();
                    if (result != null)
                    {
                        break;
                    }
                }

                Thread.Sleep(5000);
            }

            return result;
        }

        /// <summary> 更新總餘額 </summary>
        public void RefreshAvailableScores(IMSportApiParamModel model)
        {
            var userInfos = new List<IMSportsbookUserInfo>();
            try
            {
                userInfos = UserDal.GetUserInfo();
                LogsManager.Info("获取到 " + userInfos.Count.ToString() + " 个待更新用户");
            }
            catch (Exception ex)
            {
                LogsManager.InfoToTelegram("获取待更新用户失败，详细信息：" + ex.Message);
                return;
            }

            foreach (var userInfo in userInfos)
            {
                string tpGameAccount = TPGameAccountReadService.GetTPGameAccountByRule(Product, userInfo.UserID);
                model.PlayerId = tpGameAccount;

                try
                {
                    var balanceData = GetBalance(model);

                    if (balanceData != null && balanceData.Code == (int)APIErrorCode.Success)
                    {
                        userInfo.AvailableScores = decimal.Parse(balanceData.Balance);
                        userInfo.FreezeScores = 0;

                        if (UserDal.UpdateAvailableScores(userInfo))
                        {
                            LogsManager.Info(@"更新用户：" + userInfo.UserName +
                                "，余额为：" + userInfo.AvailableScores +
                                "，可用余额为：" + userInfo.FreezeScores +
                                "，冻结金额为：" + (userInfo.AvailableScores - userInfo.FreezeScores));
                        }
                        else
                        {
                            LogsManager.InfoToTelegram("更新用户：" + userInfo.UserName + "，余额失败，未能成功更新余额");
                        }
                    }
                    else
                    {
                        LogsManager.InfoToTelegram("远程获取用户：" + userInfo.UserName + "，余额失败，未能成功更新余额");
                    }
                }
                catch (Exception ex)
                {
                    LogsManager.InfoToTelegram("更新用户：" + userInfo.UserName + "，余额失败，详细信息：" + ex);
                }

                Thread.Sleep(1000);
            }

            LogsManager.Info("更新用户余额完毕");
        }
    }
}