using IMPTDataBase.Common;
using IMPTDataBase.DLL;
using IMPTDataBase.Enums;
using IMPTDataBase.Model;
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

namespace IMPTDataBase.BLL
{
    public class Transfer : OldBaseTPGameApiService<IMPTApiParamModel>
    {
        public Transfer(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        protected override PlatformProduct Product => PlatformProduct.IMPT;

        protected override BaseReturnModel GetRemoteOrderStatus(IMPTApiParamModel apiParam)
        {
            var tmpActType = apiParam.ActType;
            string apiResult = string.Empty;

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
                        LogUtil.ForcedDebug("远程查询订单状态失败" +
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
                LogsManager.InfoToTelegram("远程确认" + (apiParam.ActType == ApiAction.Recharge ? "充值" : "提款") + "失败" +
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

        public override BaseReturnDataModel<UserScore> GetRemoteUserScore(IMPTApiParamModel apiParam)
        {
            IMBalanceInfo balanceInfo = GetBalance(apiParam);

            BaseReturnDataModel<UserScore> returnModel = null;

            if (balanceInfo != null && !balanceInfo.Balance.IsNullOrEmpty() && balanceInfo.Code == (int)APIErrorCode.Success)
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

        protected override BaseReturnModel SaveTransferOrderSuccess(OldTPGameOrderParam<IMPTApiParamModel> oldTPGameOrderParam, UserScore userScore)
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

        protected override BaseReturnModel SaveTransferOrderFail(OldTPGameOrderParam<IMPTApiParamModel> oldTPGameOrderParam, string errorMsg)
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

        protected override BaseReturnDataModel<TPGameTransferReturnResult> GetTransferReturnModel(OldTPGameOrderParam<IMPTApiParamModel> oldTPGameOrderParam)
        {
            IMPTApiParamModel model = oldTPGameOrderParam.ApiParam;
            string apiResult = null;
            FundTransferResult fundTransferResult = FundTransferWithRetry(model, ref apiResult);

            var tpGameTransferReturnResult = new TPGameTransferReturnResult()
            {
                ApiResult = apiResult
            };

            if (fundTransferResult == null)
            {
                string errorMsg = "远程" + (model.ActType == ApiAction.Recharge ? "充值" : "提款") +
                    "失败，billno：" + model.MoneyID + "，api retun null.";

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
                string errorMsg = "远程" + (model.ActType == ApiAction.Recharge ? "充值" : "提款") + "失败" +
                    "，billno：" + model.MoneyID +
                    "，error_code：" + fundTransferResult.Code + $"({errorCodeText})" +
                    "，datastatus：" + fundTransferResult.ToJsonString();

                LogUtil.Error(errorMsg);
                return new BaseReturnDataModel<TPGameTransferReturnResult>(errorMsg, tpGameTransferReturnResult);
            }
        }

        //public void TransferHandle(object obj)
        //{
        //    try
        //    {
        //        IMPTApiParamModel model = (IMPTApiParamModel)obj;

        //        FundTransferResult result = FundTransferWithRetry((IMPTApiParamModel)model);

        //        if (result == null)
        //        {
        //            LogsManager.InfoToTelegram("远程" + (model.ActType == ApiAction.Recharge ? "充值" : "提款") +
        //                "失败，billno：" + model.MoneyID + "，api retun null.");
        //            return;
        //        }

        //        if (result.Code != (int)APIErrorCode.Success)
        //        {
        //            LogsManager.InfoToTelegram("远程" + (model.ActType == ApiAction.Recharge ? "充值" : "提款") + "失败" +
        //                "，billno：" + model.MoneyID +
        //                "，error_code：" + result.Code +
        //                "，datastatus：" + result.ToJsonString());

        //            var errorMsg = EnumHelper<APIErrorCode>.GetEnumDescription(Enum.GetName(typeof(APIErrorCode), result.Code));

        //            if (result.Status == "Processed")
        //            {
        //                //處理中再驗一次
        //                CheckFundTransfer(model);
        //            }
        //            else if (result.Status == "Declined")
        //            {
        //                //失敗
        //                #region 退还积分
        //                //退还积分
        //                ResultModel rollbackResult = CycleTryTransferRollback(model.MoneyID, ((int)model.ActType).ToString(), errorMsg, true);
        //                if (rollbackResult.IsSuccess)
        //                {
        //                    LogsManager.Info("回滚" + (model.ActType == ApiAction.Recharge ? "充值" : "提款") + "成功" +
        //                        "，billno：" + model.MoneyID +
        //                        "，error_code：" + result.Code + "，message：" + errorMsg +
        //                        "，datastatus：" + JsonConvert.SerializeObject(result));
        //                }
        //                else
        //                {
        //                    LogsManager.InfoToTelegram("回滚" + (model.ActType == ApiAction.Recharge ? "充值" : "提款") + "失败" +
        //                        "，billno：" + model.MoneyID +
        //                        "，code：" + rollbackResult.Info + "，msg：" + rollbackResult.Msg +
        //                        "，error_code：" + result.Code + "，message：" + errorMsg +
        //                        "，datastatus：" + JsonConvert.SerializeObject(result));
        //                }

        //                #region 转账失败发送消息

        //                string summary = string.Empty;

        //                if (model.ActType == ApiAction.Recharge)
        //                {
        //                    if (rollbackResult.IsSuccess)
        //                    {
        //                        summary = "转账¥" + model.Amount + "到PT电游账户失败，请重试。";
        //                    }
        //                    else
        //                    {
        //                        summary = "转账¥" + model.Amount + "到PT电游账户失败，请联系客服。";
        //                    }
        //                }
        //                else
        //                {
        //                    summary = "从PT电游账户提款¥" + model.Amount + "失败，请重试。";
        //                }

        //                SendTransferMessage(model.UserID.ToString(), decimal.Parse(model.Amount), summary);

        //                #endregion
        //                #endregion
        //            }
        //        }
        //        else
        //        {
        //            LogsManager.Info("远程" + (model.ActType == ApiAction.Recharge ? "充值" : "提款") + "成功，等待确认" +
        //                "，billno：" + model.MoneyID +
        //                "，datastatus：" + result.ToJsonString());

        //            CheckFundTransfer(model);
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
        /// <param name="model"></param>
        /// <returns></returns>
        public FundTransferResult FundTransferWithRetry(IMPTApiParamModel model, ref string apiResult)
        {
            FundTransferResult result = null;

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

        ///// <summary>
        ///// 確認上下分狀態
        ///// </summary>
        ///// <param name="model"></param>
        //public void CheckFundTransfer(IMPTApiParamModel model)
        //{
        //    int tryCount = 0;
        //    var errorMsg = string.Empty;
        //    while (tryCount < 5)
        //    {
        //        tryCount++;

        //        //確認訂單
        //        var result = GetOrderStatus(model);

        //        if (result != null && result.Code == (int)APIErrorCode.Success && result.Status == "Approved")
        //        {
        //            LogsManager.Info("远程确认" + (model.ActType == ApiAction.Recharge ? "充值" : "提款") + "成功，billno：" + model.MoneyID);

        //            // 獲取PT电游平台餘額
        //            var balanceData = GetBalance(model);

        //            decimal availableScores = -9999;    //餘額
        //            decimal freezeScores = -9999;       //凍結積分
        //            if (balanceData != null && balanceData.Code == (int)APIErrorCode.Success)
        //            {
        //                availableScores = decimal.Parse(balanceData.Balance);
        //                freezeScores = 0;
        //            }

        //            //本地转账
        //            ResultModel saveDB = CycleTryTransferSuccess(
        //                model.MoneyID,
        //                ((int)model.ActType).ToString(),
        //                model.UserID,
        //                availableScores,
        //                freezeScores);

        //            if (saveDB.IsSuccess)
        //            {
        //                #region 发送转帐成功消息
        //                string summary = string.Empty;

        //                if (model.ActType == ApiAction.Recharge)
        //                {
        //                    summary = "您成功转账 ¥" + model.Amount + " 到PT电游账户";
        //                }
        //                else
        //                {
        //                    summary = "您成功从PT电游账户提款 ¥" + model.Amount;
        //                }

        //                SendTransferMessage(model.UserID.ToString(), decimal.Parse(model.Amount), summary);

        //                LogsManager.Info("本地" + (model.ActType == ApiAction.Recharge ? "充值" : "提款") + "成功，billno：" + model.MoneyID);

        //                #endregion

        //                break;
        //            }
        //            else
        //            {
        //                LogsManager.InfoToTelegram("本地" + (model.ActType == ApiAction.Recharge ? "充值" : "提款") + "失败" +
        //                    "，billno：" + model.MoneyID +
        //                    "，code：" + saveDB.Info +
        //                    "，msg：" + saveDB.Msg);

        //                errorMsg = saveDB.Msg;
        //            }
        //        }
        //        else if (result != null && result.Code == (int)APIErrorCode.Success)
        //        {
        //            errorMsg = EnumHelper<APIErrorCode>.GetEnumDescription(Enum.GetName(typeof(APIErrorCode), result.Code));
        //            string dataResultStr = "";
        //            try
        //            {
        //                dataResultStr = result.ToJsonString();
        //            }
        //            catch (Exception ex)
        //            {
        //            }

        //            LogsManager.InfoToTelegram("远程查询订单状态，类型：" + (model.ActType == ApiAction.Recharge ? "充值" : "提款") + "失败" +
        //                    "，billno：" + model.MoneyID +
        //                    "，code：" + result.Code +
        //                    ", datastatus：" + dataResultStr +
        //                    "，msg (api error code)：" + errorMsg);
        //        }
        //        else
        //        {
        //            errorMsg = "资料回传异常，无法取得数据或解析";

        //            LogsManager.InfoToTelegram("远程查询订单状态，类型：" + (model.ActType == ApiAction.Recharge ? "充值" : "提款") + "失败" +
        //                "，billno：" + model.MoneyID +
        //                "，msg：" + errorMsg);
        //        }

        //        Thread.Sleep(5000);
        //    }

        //    if (tryCount >= 5)
        //    {
        //        //标识订单为失败状态，但不退还积分
        //        ResultModel errorHandle = CycleTryTransferRollback(model.MoneyID, ((int)model.ActType).ToString(), errorMsg, false);

        //        if (errorHandle.IsSuccess)
        //        {
        //            LogsManager.Info("标记「" + (model.ActType == ApiAction.Recharge ? "充值" : "提款") + "」失败-成功" +
        //                "，billno：" + model.MoneyID +
        //                "，message：" + errorMsg);
        //        }
        //        else
        //        {
        //            LogsManager.InfoToTelegram("标记「" + (model.ActType == ApiAction.Recharge ? "充值" : "提款") + "」失败-失败" +
        //                "，billno：" + model.MoneyID +
        //                "，code：" + errorHandle.Info +
        //                "，msg：" + errorHandle.Msg +
        //                "，message：" + errorMsg);
        //        }

        //        #region 转账失败发送消息
        //        string summary = string.Empty;

        //        if (model.ActType == ApiAction.Recharge)
        //        {
        //            summary = "转账¥" + model.Amount + "到PT电游账户失败，请联系客服。";
        //        }
        //        else
        //        {
        //            summary = "从PT电游账户提款¥" + model.Amount + "失败，请重试。";
        //        }

        //        SendTransferMessage(model.UserID.ToString(), decimal.Parse(model.Amount), summary);
        //        #endregion
        //    }
        //}

        /// <summary>
        /// 获取用户余额，返回结果为-9999表示调用失败
        /// </summary>
        /// <returns></returns>
        public static IMBalanceInfo GetBalance(IMPTApiParamModel model)
        {
            IMBalanceInfo result = new IMBalanceInfo();
            var tmpActType = model.ActType;
            string apiResult = string.Empty;
            try
            {
                model.ActType = ApiAction.TotalBalance;

                apiResult = ApiClient.GetBalance(model);

                if (!string.IsNullOrEmpty(apiResult))
                {
                    result = apiResult.Deserialize<IMBalanceInfo>();
                }
            }
            catch (Exception ex)
            {
                LogsManager.InfoToTelegram("用户" + model.PlayerId + "在 IMPT 平台获取余额失败，错误信息：" + ex.Message + "，堆栈信息：" + ex.StackTrace);
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
                    result.IsSuccess = new IMMoneyTransferService()
                        .IMTransferSuccess(TransferID, ActionType, userId, availableScores, freezeScores);

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
                    result.IsSuccess = new IMMoneyTransferService().IMTransferRollback(TransferID, ActionType, Msg, isRollback);

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
        /// 更新餘額
        /// </summary>
        public void RefreshAvailableScores(IMPTApiParamModel model)
        {
            var userInfos = new List<IMUserInfo>();
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

            foreach (IMUserInfo userInfo in userInfos)
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