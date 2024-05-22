using IMBGDataBase.Common;
using IMBGDataBase.DLL;
using IMBGDataBase.Enums;
using IMBGDataBase.Model;
using JxBackendService.Common.Util;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Model.ViewModel.ThirdParty.Old;
using JxBackendService.Service.ThirdPartyTransfer.Old;
using JxBackendServiceNF.Common.Util;
using System;
using System.Collections.Generic;
using System.Threading;

namespace IMBGDataBase.BLL
{
    public class Transfer : OldBaseTPGameApiService<IMBGApiParamModel>
    {
        public Transfer(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        protected override PlatformProduct Product => PlatformProduct.IMBG;

        protected override BaseReturnModel GetRemoteOrderStatus(IMBGApiParamModel apiParam)
        {
            string errorMsg = null;
            errorMsg = string.Empty;
            var success = "2";
            var fail = "3";

            //確認訂單
            IMBGResp<IMBGTransferStatusResp> result = GetOrderStatusByApi(apiParam);

            if (result == null || result.Data == null)
            {
                errorMsg = "远程查询订单状态，类型：" + (apiParam.ActType == ApiAction.Recharge ? "充值" : "提款") + "失败" +
                    "，billno：" + apiParam.MoneyID +
                    "，msg：资料回传异常，无法取得数据或解析";
                return null;
            }

            bool isSuccess = false;

            if (result.Data.Code == (int)APIErrorCode.Success)
            {
                if (result.Data.Status == success)
                {
                    isSuccess = true;
                }
                else if (result.Data.Status == fail)
                {
                    errorMsg = "远程查询订单状态，类型：" + (apiParam.ActType == ApiAction.Recharge ? "充值" : "提款") + "失败" +
                        "，billno：" + apiParam.MoneyID +
                        "，code：" + result.Data.Code +
                        ", datastatus：" + result.ToJsonString();
                }
                else
                {
                    errorMsg = "远程查询订单状态，类型：" + (apiParam.ActType == ApiAction.Recharge ? "充值" : "提款") + "处理中" +
                        "，billno：" + apiParam.MoneyID +
                        "，code：" + result.Data.Code +
                        ", datastatus：" + result.ToJsonString();
                }
            }
            else
            {
                errorMsg = "远程查询订单状态，类型：" + (apiParam.ActType == ApiAction.Recharge ? "充值" : "提款") + "失败" +
                    "，billno：" + apiParam.MoneyID +
                    "，code：" + result.Data.Code +
                    ", datastatus：" + result.ToJsonString() +
                    "，msg (api error code)：" + EnumHelper<APIErrorCode>.GetEnumDescription(Enum.GetName(typeof(APIErrorCode), result.Data.Code));
            }

            if (isSuccess)
            {
                return new BaseReturnModel(ReturnCode.Success);
            }
            else
            {
                return new BaseReturnModel(errorMsg);
            }
        }

        public override BaseReturnDataModel<UserScore> GetRemoteUserScore(IMBGApiParamModel apiParam)
        {
            IMBGResp<IMBGBalanceResp> imbgResp = GetBalance(apiParam);

            BaseReturnDataModel<UserScore> returnModel = null;

            if (imbgResp != null && imbgResp.Data != null && imbgResp.Data.Code == (int)APIErrorCode.Success)
            {
                returnModel = new BaseReturnDataModel<UserScore>(ReturnCode.Success, new UserScore()
                {
                    AvailableScores = imbgResp.Data.FreeMoney,
                    FreezeScores = 0
                });
            }
            else
            {
                returnModel = new BaseReturnDataModel<UserScore>("Get Remote API Fail", null);
            }

            return returnModel;
        }

        protected override BaseReturnModel SaveTransferOrderSuccess(OldTPGameOrderParam<IMBGApiParamModel> oldTPGameOrderParam, UserScore userScore)
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

        protected override BaseReturnModel SaveTransferOrderFail(OldTPGameOrderParam<IMBGApiParamModel> oldTPGameOrderParam, string errorMsg)
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

        protected override BaseReturnDataModel<TPGameTransferReturnResult> GetTransferReturnModel(OldTPGameOrderParam<IMBGApiParamModel> oldTPGameOrderParam)
        {
            IMBGApiParamModel model = oldTPGameOrderParam.ApiParam;
            string apiResult = null;
            IMBGResp<IMBGTransferResp> result = FundTransferWithRetry(model, ref apiResult);

            var tpGameTransferReturnResult = new TPGameTransferReturnResult()
            {
                ApiResult = apiResult
            };

            if (result == null)
            {
                string errorMsg = "远程" + (model.ActType == ApiAction.Recharge ? "充值" : "提款") +
                    "失败，billno：" + model.MoneyID + "，api retun null.";

                LogUtil.Error(errorMsg);
                return new BaseReturnDataModel<TPGameTransferReturnResult>(errorMsg, tpGameTransferReturnResult);
            }

            if (result.Data.Code == (int)APIErrorCode.Success)
            {
                return new BaseReturnDataModel<TPGameTransferReturnResult>(ReturnCode.Success, tpGameTransferReturnResult);
            }
            else
            {
                string errorCodeText = EnumHelper<APIErrorCode>.GetEnumDescription(Enum.GetName(typeof(APIErrorCode), result.Data.Code));
                string errorMsg = "远程" + (model.ActType == ApiAction.Recharge ? "充值" : "提款") + "失败" +
                    "，billno：" + model.MoneyID +
                    "，error_code：" + result.Data.Code + $"({errorCodeText})" +
                    "，datastatus：" + result.ToJsonString();

                LogUtil.Error(errorMsg);
                return new BaseReturnDataModel<TPGameTransferReturnResult>(errorMsg, tpGameTransferReturnResult);
            }
        }

        /// <summary>
        /// 上下分
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private IMBGResp<IMBGTransferResp> FundTransferWithRetry(IMBGApiParamModel model, ref string apiResult)
        {
            IMBGResp<IMBGTransferResp> result = null;
            apiResult = null;

            int tryCount = 0;
            while (tryCount < 5)
            {
                tryCount++;

                apiResult = ApiClient.Transfer(model);

                if (!string.IsNullOrEmpty(apiResult))
                {
                    result = apiResult.Deserialize<IMBGResp<IMBGTransferResp>>();
                    if (result != null)
                    {
                        break;
                    }
                }

                Thread.Sleep(5000);
            }

            return result;
        }

        /// <summary>
        /// 获取用户订单状态，返回结果为-9999表示呼叫远程失败
        /// </summary>
        /// <returns></returns>
        private static IMBGResp<IMBGTransferStatusResp> GetOrderStatusByApi(IMBGApiParamModel model)
        {
            IMBGResp<IMBGTransferStatusResp> result = null;
            var tmpActType = model.ActType;
            string apiResult = string.Empty;
            try
            {
                model.ActType = ApiAction.OrderStatus;

                //確認訂單
                apiResult = ApiClient.CheckTransferStatus(model);
                if (!string.IsNullOrEmpty(apiResult))
                {
                    result = apiResult.Deserialize<IMBGResp<IMBGTransferStatusResp>>();
                }
            }
            catch (Exception ex)
            {
                LogsManager.InfoToTelegram("远程确认" + (model.ActType == ApiAction.Recharge ? "充值" : "提款") + "失败" +
                    "，billno：" + model.MoneyID +
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
        public static IMBGResp<IMBGBalanceResp> GetBalance(IMBGApiParamModel model)
        {
            IMBGResp<IMBGBalanceResp> result = null;
            var tmpActType = model.ActType;

            try
            {
                model.ActType = ApiAction.TotalBalance;

                var apiResult = ApiClient.GetBalance(model);

                if (!string.IsNullOrEmpty(apiResult))
                {
                    result = apiResult.Deserialize<IMBGResp<IMBGBalanceResp>>();
                }
            }
            catch (Exception ex)
            {
                LogsManager.InfoToTelegram("用户" + model.PlayerId + "在 IMBG 平台获取余额失败，错误信息：" + ex.Message + "，堆栈信息：" + ex.StackTrace);
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
                    result.IsSuccess = new IMBGDataBase.DLL.IMBGMoneyTransferService()
                        .IMBGTransferSuccess(TransferID, ActionType, userId, availableScores, freezeScores);

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
                    bool isRollBack = true;
                    result.IsSuccess = new IMBGDataBase.DLL.IMBGMoneyTransferService().IMBGTransferRollback(TransferID, ActionType, Msg, isRollBack);

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
        /// 更新棋牌總餘額
        /// </summary>
        public void RefreshAvailableScores(IMBGApiParamModel model)
        {
            var userInfos = new List<IMBGUserInfo>();
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

            foreach (IMBGUserInfo userInfo in userInfos)
            {
                string tpGameAccount = TPGameAccountReadService.GetTPGameAccountByRule(Product, userInfo.UserID);
                model.PlayerId = tpGameAccount;

                try
                {
                    IMBGResp<IMBGBalanceResp> balanceData = GetBalance(model);

                    if (balanceData != null && balanceData.Data != null && balanceData.Data.Code == (int)APIErrorCode.Success)
                    {
                        userInfo.AvailableScores = balanceData.Data.FreeMoney;
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