using System;
using System.Collections.Generic;
using System.Threading;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Util;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Model.ViewModel.ThirdParty.Old;
using JxBackendService.Service.ThirdPartyTransfer.Old;
using LCDataBase.Common;
using LCDataBase.DLL;
using LCDataBase.Enums;
using LCDataBase.Model;
using Newtonsoft.Json;

namespace LCDataBase.BLL
{
    public class Transfer : OldBaseTPGameApiService<LCApiParamModel>
    {
        public Transfer(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        protected override PlatformProduct Product => PlatformProduct.LC;

        protected override BaseReturnModel GetRemoteOrderStatus(LCApiParamModel apiParam)
        {
            var tmpActType = apiParam.ActType;

            try
            {
                apiParam.ActType = ApiAction.OrderStatus;

                //確認訂單
                ApiResult<FundTransferResult> transferResult = GetOrderStatus(apiParam);

                if (transferResult != null && transferResult.Data != null)
                {
                    if (transferResult.Data.Status == (int)TransferStatus.Success)
                    {
                        return new BaseReturnModel(ReturnCode.Success);
                    }
                    else
                    {
                        string errorCodeText = EnumHelper<APIErrorCode>.GetEnumDescription(Enum.GetName(typeof(APIErrorCode), transferResult.Data.Code));

                        LogUtilService.ForcedDebug("远程查询订单状态失败" +
                                "，billno：" + apiParam.OrderID +
                                "，transferResult：" + transferResult.ToJsonString() +
                                "，msg (api error code)：" + new { transferResult.Data.Status, transferResult.Data.Code, errorCodeText }.ToJsonString());

                        if (transferResult.Data.Code == (int)APIErrorCode.Success)
                        {
                            return new BaseReturnModel("远程查询订单状态失败");
                        }
                        else
                        {
                            return new BaseReturnModel(errorCodeText);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogsManager.InfoToTelegram("远程确认" + (apiParam.ActType == ApiAction.Recharge ? "充值" : "提款") + "失败" +
                    "，billno：" + apiParam.OrderID +
                    "，错误信息：" + ex.Message +
                    "，堆栈信息：" + ex.StackTrace);
            }

            apiParam.ActType = tmpActType;

            return null;
        }

        public override BaseReturnDataModel<UserScore> GetRemoteUserScore(LCApiParamModel apiParam)
        {
            BaseReturnDataModel<UserScore> returnModel = null;
            ApiResult<LCBalanceInfo> balanceData = GetBalance(apiParam);

            if (balanceData != null && balanceData.Data != null)
            {
                if (balanceData.Data.Code == (int)APIErrorCode.Success)
                {
                    returnModel = new BaseReturnDataModel<UserScore>(ReturnCode.Success, new UserScore()
                    {
                        AvailableScores = balanceData.Data.TotalMoney,
                        FreezeScores = balanceData.Data.TotalMoney - balanceData.Data.FreeMoney
                    });
                }
                else
                {
                    string errorCodeText = EnumHelper<APIErrorCode>.GetEnumDescription(Enum.GetName(typeof(APIErrorCode), balanceData.Data.Code));

                    return new BaseReturnDataModel<UserScore>(errorCodeText, null);
                }
            }
            else
            {
                returnModel = new BaseReturnDataModel<UserScore>("Get Remote API Fail", null);
            }

            return returnModel;
        }

        protected override BaseReturnModel SaveTransferOrderSuccess(OldTPGameOrderParam<LCApiParamModel> oldTPGameOrderParam, UserScore userScore)
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

        protected override BaseReturnModel SaveTransferOrderFail(OldTPGameOrderParam<LCApiParamModel> oldTPGameOrderParam, string errorMsg)
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

        protected override BaseReturnDataModel<TPGameTransferReturnResult> GetTransferReturnModel(OldTPGameOrderParam<LCApiParamModel> oldTPGameOrderParam)
        {
            LCApiParamModel model = oldTPGameOrderParam.ApiParam;
            string apiResult = null;
            ApiResult<FundTransferResult> fundTransferResult = FundTransfer(model, ref apiResult);

            var tpGameTransferReturnResult = new TPGameTransferReturnResult()
            {
                ApiResult = apiResult
            };

            if (fundTransferResult == null)
            {
                string errorMsg = "远程" + (model.ActType == ApiAction.Recharge ? "充值" : "提款") +
                    "失败，billno：" + model.OrderID + "，api retun null.";

                LogUtilService.Error(errorMsg);

                return new BaseReturnDataModel<TPGameTransferReturnResult>(errorMsg, tpGameTransferReturnResult);
            }

            if (fundTransferResult.Data.Code == (int)APIErrorCode.Success)
            {
                tpGameTransferReturnResult.RemoteUserScore = new UserScore()
                {
                    AvailableScores = fundTransferResult.Data.Money,
                    FreezeScores = 0
                };

                return new BaseReturnDataModel<TPGameTransferReturnResult>(ReturnCode.Success, tpGameTransferReturnResult);
            }
            else
            {
                string errorCodeText = EnumHelper<APIErrorCode>.GetEnumDescription(Enum.GetName(typeof(APIErrorCode), fundTransferResult.Data.Code));
                string errorMsg = "远程" + (model.ActType == ApiAction.Recharge ? "充值" : "提款") + "失败" +
                    "，billno：" + model.OrderID +
                    "，error_code：" + fundTransferResult.Data.Code + $"({errorCodeText})" +
                    "，datastatus：" + fundTransferResult.ToJsonString();

                LogUtilService.Error(errorMsg);

                return new BaseReturnDataModel<TPGameTransferReturnResult>(errorMsg, tpGameTransferReturnResult);
            }
        }

        //public void TransferHandle(object obj)
        //{
        //    try
        //    {
        //        LCApiParamModel lcApiParam = (LCApiParamModel)obj;
        //        ApiResult<FundTransferResult> result = FundTransfer(lcApiParam);

        //        string actType = lcApiParam.ActType == ApiAction.Recharge ? "充值" : "提款";

        //        if (result == null)
        //        {
        //            LogsManager.InfoToTelegram("远程" + actType +
        //                "失败，billno：" + lcApiParam.OrderID + "，api retun null.");
        //            return;
        //        }

        //        if (result.Data == null)
        //        {
        //            LogsManager.InfoToTelegram("远程" + actType + "失败" +
        //                "，billno：" + lcApiParam.OrderID +
        //                "，datastatus：" + JsonConvert.SerializeObject(result));
        //            return;
        //        }

        //        if (result.Data.Code == (int)APIErrorCode.Success || result.Data.Code == (int)APIErrorCode.OrderRepeat)
        //        {
        //            LogsManager.Info("远程" + actType + "成功，等待确认" +
        //                "，billno：" + lcApiParam.OrderID +
        //                "，datastatus：" + JsonConvert.SerializeObject(result));

        //            CheckFundTransfer(lcApiParam, result.Data.Money);
        //        }
        //        else
        //        {
        //            LogsManager.InfoToTelegram("远程" + actType + "失败" +
        //                "，billno：" + lcApiParam.OrderID +
        //                "，error_code：" + result.Data.Code +
        //                "，datastatus：" + JsonConvert.SerializeObject(result));

        //            bool isSuccess = IsSuccessTransferOrder(lcApiParam);
        //            if (isSuccess)
        //            {
        //                ApiResult<FundTransferResult> retryResult = FundTransfer(lcApiParam);
        //                if (retryResult != null && retryResult.Data != null &&
        //                    (retryResult.Data.Code == (int)APIErrorCode.Success || retryResult.Data.Code == (int)APIErrorCode.OrderRepeat))
        //                {
        //                    CheckFundTransfer(lcApiParam, retryResult.Data.Money);
        //                    return;
        //                }
        //            }

        //            var errorMsg = EnumHelper<APIErrorCode>.GetEnumDescription(Enum.GetName(typeof(APIErrorCode), result.Data.Code));

        //            //退还积分
        //            ResultModel rollbackResult = CycleTryTransferRollback(lcApiParam.TransferID, ((int)lcApiParam.ActType).ToString(), errorMsg, true);
        //            if (rollbackResult.IsSuccess)
        //            {
        //                LogsManager.Info("回滚" + actType + "成功" +
        //                    "，billno：" + lcApiParam.OrderID +
        //                    "，error_code：" + result.Data.Code + "，message：" + errorMsg +
        //                    "，datastatus：" + JsonConvert.SerializeObject(result));
        //            }
        //            else
        //            {
        //                LogsManager.InfoToTelegram("回滚" + actType + "失败" +
        //                    "，billno：" + lcApiParam.OrderID +
        //                    "，code：" + rollbackResult.Info + "，msg：" + rollbackResult.Msg +
        //                    "，error_code：" + result.Data.Code + "，message：" + errorMsg +
        //                    "，datastatus：" + JsonConvert.SerializeObject(result));
        //            }

        //            #region 转账失败发送消息

        //            string summary = string.Empty;

        //            if (lcApiParam.ActType == ApiAction.Recharge)
        //            {
        //                if (rollbackResult.IsSuccess)
        //                {
        //                    summary = "转账¥" + lcApiParam.Money.ToString("N4") + "到棋牌账户失败，请重试。";
        //                }
        //                else
        //                {
        //                    summary = "转账¥" + lcApiParam.Money.ToString("N4") + "到棋牌账户失败，请联系客服。";
        //                }
        //            }
        //            else
        //            {
        //                summary = "从棋牌账户提款¥" + lcApiParam.Money.ToString("N4") + "失败，请重试。";
        //            }

        //            SendTransferMessage(lcApiParam.UserID.ToString(), lcApiParam.Money, summary);

        //            #endregion
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogsManager.Info("未知异常，详细信息：" + ex.Message + "，堆栈：" + ex.StackTrace);
        //    }
        //}

        /// <summary>
        /// 上下分
        /// </summary>
        /// <param name="lcApiParam"></param>
        /// <returns></returns>
        public ApiResult<FundTransferResult> FundTransfer(LCApiParamModel lcApiParam, ref string apiResult)
        {
            ApiResult<FundTransferResult> result = null;
            string actType = lcApiParam.ActType == ApiAction.Recharge ? "充值" : "提款";
            apiResult = ApiClient.GetAPIResult(lcApiParam);

            if (!string.IsNullOrEmpty(apiResult))
            {
                LogsManager.Info("远程确认" + actType + "成功，FundTransfer：" + apiResult);
                result = apiResult.Deserialize<ApiResult<FundTransferResult>>();
            }

            return result;
        }

        ///// <summary>
        ///// 確認上下分狀態
        ///// </summary>
        ///// <param name="model"></param>
        ///// <param name="freeMoney">上分后可下分金额</param>
        //public void CheckFundTransfer(LCApiParamModel model, decimal freeMoney)
        //{
        //    int tryCount = 0;
        //    var errorMsg = string.Empty;
        //    string actType = model.ActType == ApiAction.Recharge ? "充值" : "提款";

        //    while (tryCount < 5)
        //    {
        //        tryCount++;

        //        //確認訂單
        //        ApiResult<FundTransferResult> transferResult = GetOrderStatus(model);

        //        if (transferResult != null && transferResult.Data != null)
        //        {
        //            if (transferResult.Data.Status == (int)TransferStatus.Success)
        //            {
        //                decimal availableScores = freeMoney;
        //                decimal freezeScores = 0;

        //                LogsManager.Info("远程确认" + actType + "成功，billno：" + model.OrderID);

        //                // 獲取棋牌平台餘額
        //                ApiResult<LCBalanceInfo> balanceData = GetBalance(model);

        //                if (balanceData != null && balanceData.Data != null && balanceData.Data.Code == (int)APIErrorCode.Success)
        //                {
        //                    // 確認訂單回上分后可下分金额，與問第三方餘額不同時，以確認訂單回來上分后可下分金额為主
        //                    if (availableScores == balanceData.Data.FreeMoney)
        //                    {
        //                        availableScores = balanceData.Data.TotalMoney;
        //                        freezeScores = availableScores - balanceData.Data.FreeMoney;
        //                    }
        //                }

        //                //本地转账
        //                ResultModel saveDB = CycleTryTransferSuccess(
        //                    model.TransferID,
        //                    ((int)model.ActType).ToString(),
        //                    model.UserID,
        //                    availableScores,
        //                    freezeScores);

        //                if (saveDB.IsSuccess)
        //                {
        //                    #region 发送转帐成功消息
        //                    string summary = string.Empty;

        //                    if (model.ActType == ApiAction.Recharge)
        //                    {
        //                        summary = "您成功转账 ¥" + model.Money.ToString("N4") + " 到棋牌账户";
        //                    }
        //                    else
        //                    {
        //                        summary = "您成功从棋牌账户提款 ¥" + model.Money.ToString("N4");
        //                    }

        //                    SendTransferMessage(model.UserID.ToString(), model.Money, summary);

        //                    LogsManager.Info("本地" + actType + "成功，billno：" + model.OrderID);

        //                    #endregion

        //                    break;
        //                }
        //                else
        //                {
        //                    LogsManager.InfoToTelegram("本地" + actType + "失败" +
        //                        "，billno：" + model.OrderID +
        //                        "，code：" + saveDB.Info +
        //                        "，msg：" + saveDB.Msg);

        //                    errorMsg = saveDB.Msg;
        //                }
        //            }
        //            else
        //            {
        //                LogsManager.InfoToTelegram("远程查询订单状态，类型：" + actType + "失败" +
        //                    "，billno：" + model.OrderID +
        //                    "，code：" + transferResult.Data.Code +
        //                    ", transferResult：" + transferResult.ToJsonString() +
        //                    "，msg (api error code)：" + errorMsg);
        //            }
        //        }
        //        else
        //        {
        //            errorMsg = "资料回传异常，无法取得数据或解析";

        //            LogsManager.InfoToTelegram("远程查询订单状态，类型：" + actType + "失败" +
        //                "，billno：" + model.OrderID +
        //                "，msg：" + errorMsg);
        //        }

        //        Thread.Sleep(5000);
        //    }

        //    if (tryCount >= 5)
        //    {
        //        //标识订单为失败状态，但不退还积分
        //        ResultModel errorHandle = CycleTryTransferRollback(model.TransferID, ((int)model.ActType).ToString(), errorMsg, false);

        //        if (errorHandle.IsSuccess)
        //        {
        //            LogsManager.Info("标记「" + actType + "」失败-成功" +
        //                "，billno：" + model.OrderID +
        //                "，message：" + errorMsg);
        //        }
        //        else
        //        {
        //            LogsManager.InfoToTelegram("标记「" + actType + "」失败-失败" +
        //                "，billno：" + model.OrderID +
        //                "，code：" + errorHandle.Info +
        //                "，msg：" + errorHandle.Msg +
        //                "，message：" + errorMsg);
        //        }

        //        #region 转账失败发送消息
        //        string summary = string.Empty;

        //        if (model.ActType == ApiAction.Recharge)
        //        {
        //            summary = "转账¥" + model.Money.ToString("N4") + "到棋牌账户失败，请联系客服。";
        //        }
        //        else
        //        {
        //            summary = "从棋牌账户提款¥" + model.Money.ToString("N4") + "失败，请重试。";
        //        }

        //        SendTransferMessage(model.UserID.ToString(), model.Money, summary);
        //        #endregion
        //    }
        //}

        //private static bool IsSuccessTransferOrder(LCApiParamModel lcApiParam)
        //{
        //    ApiResult<FundTransferResult> transferResult = GetOrderStatus(lcApiParam);

        //    if (transferResult != null && transferResult.Data != null &&
        //        transferResult.Data.Status == (int)TransferStatus.Success)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        /// <summary>
        /// 获取用户订单状态，返回结果为null表示呼叫远程失败
        /// </summary>
        /// <returns></returns>
        public static ApiResult<FundTransferResult> GetOrderStatus(LCApiParamModel model)
        {
            ApiResult<FundTransferResult> result = null;
            var tmpActType = model.ActType;
            string apiResult = string.Empty;
            string actType = model.ActType == ApiAction.Recharge ? "充值" : "提款";

            try
            {
                model.ActType = ApiAction.OrderStatus;

                //確認訂單
                apiResult = ApiClient.GetAPIResult(model);

                if (!string.IsNullOrEmpty(apiResult))
                {
                    LogsManager.Info("远程确认" + actType + "成功，GetOrderStatus：" + apiResult.ToJsonString());
                    result = apiResult.Deserialize<ApiResult<FundTransferResult>>();
                }
            }
            catch (Exception ex)
            {
                LogsManager.InfoToTelegram("远程确认" + actType + "失败" +
                    "，billno：" + model.OrderID +
                    "，datastatus：" + apiResult +
                    "，错误信息：" + ex.Message +
                    "，堆栈信息：" + ex.StackTrace);
            }

            model.ActType = tmpActType;

            return result;
        }

        /// <summary>
        /// 获取用户余额，返回结果为-9999表示调用失败
        /// </summary>
        /// <returns></returns>
        public static ApiResult<LCBalanceInfo> GetBalance(LCApiParamModel model)
        {
            ApiResult<LCBalanceInfo> result = new ApiResult<LCBalanceInfo>();
            var tmpActType = model.ActType;
            string apiResult = string.Empty;
            string actType = model.ActType == ApiAction.Recharge ? "充值" : "提款";

            try
            {
                model.ActType = ApiAction.TotalBalance;

                apiResult = ApiClient.GetAPIResult(model);

                if (!string.IsNullOrEmpty(apiResult))
                {
                    LogsManager.Info("远程确认" + actType + "成功，GetBalance：" + apiResult.ToJsonString());
                    result = JsonUtil.Deserialize<ApiResult<LCBalanceInfo>>(apiResult);
                }
            }
            catch (Exception ex)
            {
                LogsManager.InfoToTelegram("用户" + model.Account + "在 LC 平台获取余额失败，错误信息：" + ex.Message + "，堆栈信息：" + ex.StackTrace);
            }

            model.ActType = tmpActType;

            return result;
        }

        /// <summary>
        /// 循环尝试转账成功时数据处理
        /// </summary>
        /// <param name="TransferID"></param>
        /// <param name="ActionType"></param>
        private ResultModel CycleTryTransferSuccess(string TransferID, string ActionType, int userId, decimal availableScores, decimal freezeScores)
        {
            ResultModel result = new ResultModel();
            int tryCount = 0;
            while (tryCount < 5)
            {
                tryCount++;

                try
                {
                    result.IsSuccess = new LCDataBase.DLL.LCMoneyTransferService()
                        .LCTransferSuccess(TransferID, ActionType, userId, availableScores, freezeScores);

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
        /// 循环尝试数据回滚
        /// </summary>
        private ResultModel CycleTryTransferRollback(string TransferID, string ActionType, string Msg)
        {
            ResultModel result = new ResultModel();

            int tryCount = 0;
            while (tryCount < 5)
            {
                tryCount++;

                try
                {
                    bool isRollback = true;
                    result.IsSuccess = new LCDataBase.DLL.LCMoneyTransferService().LCTransferRollback(TransferID, ActionType, Msg, isRollback);

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
        /// 转入棋牌数据
        /// </summary>
        /// <returns></returns>
        public List<LCDataBase.Model.LCMoneyInInfo> GetLCMoneyInInfo()
        {
            try
            {
                return new LCDataBase.DLL.LCMoneyTransferService().SearchMoneyIn();
            }
            catch (Exception ex)
            {
                LogsManager.Info("获取充值数据失败，错误描述：" + ex.Message + "，堆栈：" + ex.StackTrace);
                return null;
            }
        }

        /// <summary>
        /// 转出棋牌数据
        /// </summary>
        /// <returns></returns>
        public List<LCDataBase.Model.LCMoneyOutInfo> GetLCMoneyOutInfo()
        {
            try
            {
                return new LCDataBase.DLL.LCMoneyTransferService().SearchMoneyOut();
            }
            catch (Exception ex)
            {
                LogsManager.Info("获取提款数据失败，错误描述：" + ex.Message + "，堆栈：" + ex.StackTrace);
                return null;
            }
        }

        /// <summary>
        /// 更新棋牌總餘額
        /// </summary>
        public void RefreshAvailableScores(LCApiParamModel model)
        {
            var userInfos = new List<LCUserInfo>();
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

            foreach (LCUserInfo userInfo in userInfos)
            {
                string tpGameAccount = TPGameAccountReadService.GetTPGameAccountByRule(Product, userInfo.UserID);
                model.UserID = userInfo.UserID;
                model.Account = tpGameAccount;

                try
                {
                    var balanceData = GetBalance(model);

                    if (balanceData != null && balanceData.Data != null)
                    {
                        if (balanceData.Data.Code == (int)APIErrorCode.Success)
                        {
                            userInfo.AvailableScores = balanceData.Data.TotalMoney;
                            userInfo.FreezeScores = balanceData.Data.TotalMoney - balanceData.Data.FreeMoney;

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
                            string errorCodeText = EnumHelper<APIErrorCode>.GetEnumDescription(Enum.GetName(typeof(APIErrorCode), balanceData.Data.Code));
                            LogsManager.InfoToTelegram("远程获取用户：" + userInfo.UserName + "，余额失败，未能成功更新余额(" + errorCodeText + ")");
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