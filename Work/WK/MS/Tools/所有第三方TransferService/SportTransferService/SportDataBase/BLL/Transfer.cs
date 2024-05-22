using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Model.ViewModel.ThirdParty.Old;
using JxBackendService.Service.ThirdPartyTransfer.Old;
using SportDataBase.Common;
using SportDataBase.DLL;
using SportDataBase.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading;

namespace SportDataBase.BLL
{
    public class Transfer : OldBaseTPGameApiService<SportApiParamModel>
    {
        public Transfer(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {

        }

        protected override PlatformProduct Product => PlatformProduct.Sport;

        protected override BaseReturnModel GetRemoteOrderStatus(SportApiParamModel apiParam)
        {
            var client = DependencyUtil.ResolveService<IApiClient>();

            ApiResult<FundTransferResult> transferResult = client.CheckFundTransfer(apiParam.OrderID);

            if (transferResult == null)
            {
                return null;
            }

            if (transferResult.error_code == 0 && transferResult.Data.status == 0)
            {
                try
                {
                    if (apiParam.Type == "1")
                    {
                        //转账成功，设置会员投注限额
                        SetUserBetAmountLimit(apiParam);
                    }
                }
                catch (Exception ex)
                {
                    LogsManager.Error(ex);
                }

                return new BaseReturnModel(ReturnCode.Success);
            }
            else
            {
                LogsManager.ForcedDebug("远程查询订单状态失败" +
                                "，billno：" + apiParam.OrderID +
                                "，transferResult：" + transferResult.ToJsonString());

                if (transferResult.error_code == 0 &&  // 第三方执行成功
                    (transferResult.Data.status == 1 || transferResult.Data.status == 2)) // 1:执行过程中失败 2:交易纪录不存在
                {                    
                    return new BaseReturnModel(transferResult.message);
                }

                // 非成功狀態皆進行重查訂單
                return null;
            }
        }

        public override BaseReturnDataModel<UserScore> GetRemoteUserScore(SportApiParamModel apiParam)
        {
            BaseReturnDataModel<UserScore> returnModel = null;
            ApiResult<UserBalanceItem> balanceData = Utility.GetBalance(apiParam.TPGameUserID);

            if (balanceData != null && balanceData.Data != null && balanceData.error_code == 0)
            {
                returnModel = new BaseReturnDataModel<UserScore>(ReturnCode.Success, new UserScore()
                {
                    AvailableScores = balanceData.Data.balance.GetValueOrDefault(),
                    FreezeScores = balanceData.Data.outstanding.GetValueOrDefault()
                });
            }
            else
            {
                if (balanceData != null)
                {
                    returnModel = new BaseReturnDataModel<UserScore>(balanceData.message, null);
                }
                else
                {
                    returnModel = new BaseReturnDataModel<UserScore>("GetRemoteUserScore Fail", null);
                }
            }

            return returnModel;
        }

        protected override BaseReturnModel SaveTransferOrderSuccess(OldTPGameOrderParam<SportApiParamModel> oldTPGameOrderParam, UserScore userScore)
        {
            ResultModel resultModel = CycleTryTransferSuccess(
                oldTPGameOrderParam.TPGameMoneyInfo.GetMoneyID().ToInt32(),
                oldTPGameOrderParam.ApiParam.Type,
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

        protected override BaseReturnModel SaveTransferOrderFail(OldTPGameOrderParam<SportApiParamModel> oldTPGameOrderParam, string errorMsg)
        {
            ResultModel errorHandle = CycleTryTransferRollback(
                oldTPGameOrderParam.TPGameMoneyInfo.GetMoneyID().ToInt32(),
                oldTPGameOrderParam.ApiParam.Type,
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


        protected override BaseReturnDataModel<TPGameTransferReturnResult> GetTransferReturnModel(OldTPGameOrderParam<SportApiParamModel> oldTPGameOrderParam)
        {
            SportApiParamModel apiParam = oldTPGameOrderParam.ApiParam;
            string apiResult = null;
            ApiResult<FundTransferResult> fundTransferResult = FundTransfer(apiParam, ref apiResult);

            var tpGameTransferReturnResult = new TPGameTransferReturnResult()
            {
                ApiResult = apiResult
            };

            if (fundTransferResult == null || fundTransferResult.Data == null)
            {
                string errorMsg = "远程" + (apiParam.Type == "1" ? "充值" : "提款") +
                    "失败，billno：" + apiParam.OrderID + "，api return null.";

                LogsManager.Error(errorMsg);
                return new BaseReturnDataModel<TPGameTransferReturnResult>(errorMsg, tpGameTransferReturnResult);
            }

            if (fundTransferResult.error_code == 0 && fundTransferResult.Data.status == 0)
            {
                tpGameTransferReturnResult.RemoteUserScore = new UserScore()
                {
                    AvailableScores = fundTransferResult.Data.after_amount,
                    FreezeScores = 0
                };

                return new BaseReturnDataModel<TPGameTransferReturnResult>(ReturnCode.Success, tpGameTransferReturnResult);
            }
            else
            {
                string errorCodeText = fundTransferResult.message;
                string errorMsg = "远程" + (apiParam.Type == "1" ? "充值" : "提款") + "失败" +
                    "，billno：" + apiParam.OrderID +
                    "，error_code：" + fundTransferResult.error_code + $"({errorCodeText})" +
                    "，fundTransferResult：" + fundTransferResult.ToJsonString();

                LogsManager.Error(errorMsg);
                return new BaseReturnDataModel<TPGameTransferReturnResult>(errorMsg, tpGameTransferReturnResult);
            }
        }

        //public void TransferHandle(object _obj)
        //{
        //    try
        //    {
        //        object[] obj = _obj as object[];
        //        string Url = obj[0].ToString();
        //        string VendorID = obj[1].ToString();
        //        string Currency = obj[2].ToString();
        //        string type = obj[3].ToString();
        //        string TransferID = obj[4].ToString();
        //        string OrderID = obj[5].ToString();
        //        string UserID = obj[6].ToString();
        //        string Money = obj[7].ToString();
        //        string TPGameUserID = obj[8].ToString();

        //        //去打沙巴體育的API拿充值／提現單資訊回來
        //        ApiResult<FundTransferResult> model = FundTransfer(VendorID, Url, Currency, type, TPGameUserID, OrderID, Money);

        //        if (model.error_code != 0 && model.error_code != 1)
        //        {
        //            LogsManager.InfoToEmail("远程" + (type == "1" ? "充值" : "提款") + "失败，billno：" + OrderID
        //                + "，error_code：" + model.error_code.ToString() + "，message：" + model.message
        //                + "，datastatus：" + (model.Data != null ? model.Data.status.ToString() : string.Empty));

        //            //退还积分
        //            ResultModel model1 = CycleTryTransferRollback(int.Parse(TransferID), type, model.message, true);

        //            if (model1.IsSuccess)
        //            {
        //                LogsManager.Info("回滚" + (type == "1" ? "充值" : "提款") + "成功，billno：" + OrderID
        //                     + "，error_code：" + model.error_code.ToString() + "，message：" + model.message
        //                     + "，datastatus：" + (model.Data != null ? model.Data.status.ToString() : string.Empty));
        //            }
        //            else
        //            {
        //                LogsManager.InfoToEmail("回滚" + (type == "1" ? "充值" : "提款") + "失败，billno：" + OrderID
        //                     + "，code：" + model1.Info + "，msg：" + model1.Msg
        //                     + "，error_code：" + model.error_code.ToString() + "，message：" + model.message
        //                     + "，datastatus：" + (model.Data != null ? model.Data.status.ToString() : string.Empty));
        //            }

        //            #region 转账失败发送消息

        //            try
        //            {
        //                string summary = string.Empty;

        //                if (type == "1")
        //                {
        //                    if (model1.IsSuccess)
        //                    {
        //                        summary = "转账¥" + decimal.Parse(Money).ToString("N4") + "到体育账户失败，请重试。";
        //                    }
        //                    else
        //                    {
        //                        summary = "转账¥" + decimal.Parse(Money).ToString("N4") + "到体育账户失败，请联系客服。";
        //                    }
        //                }
        //                else
        //                {
        //                    summary = "从体育账户提款¥" + decimal.Parse(Money).ToString("N4") + "失败，请重试。";
        //                }

        //                MessageEntity entity = new MessageEntity();
        //                entity.MessageType = RabbitMessageType.TransferNotice;
        //                entity.SendType = RQSettings.RQ_DIRECT_TYPE;
        //                entity.SendTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        //                entity.SendExchange = RQSettings.RQ_WCF_EXCHANGE;
        //                entity.SendRoutKey = RQSettings.RQ_WCF_ROUTKEY;
        //                var Item = new TransferNoticeEntity() { UserId = UserID, Amount = decimal.Parse(Money), Summary = summary };
        //                entity.SendContent = Item;
        //                _Manager.SendMessage(entity);
        //            }
        //            catch (Exception ex)
        //            {
        //                LogsManager.Info("发送转账消息异常，详细信息：" + ex.Message + "，堆栈：" + ex.StackTrace);
        //            }

        //            #endregion
        //        }
        //        else
        //        {

        //            LogsManager.Info("远程" + (type == "1" ? "充值" : "提款") + "成功，等待确认，billno：" + OrderID
        //                + "，error_code：" + model.error_code.ToString() + "，message：" + model.message
        //                + "，datastatus：" + (model.Data != null ? model.Data.status.ToString() : string.Empty));

        //            CheckFundTransferFundTransfer(VendorID, Url, OrderID, TransferID, type, Money, UserID, TPGameUserID);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogsManager.Info("未知异常，详细信息：" + ex.Message + "，堆栈：" + ex.StackTrace);
        //    }
        //}

        //public void CheckFundTransferFundTransfer(string VendorID, string Url, string OrderID, string TransferID, string type, string Money, string UserID, string TPGameUserID)
        //{
        //    int tryCount = 0;

        //    while (true)
        //    {
        //        tryCount++;

        //        ApiClient client = new ApiClient(VendorID, Url);

        //        var model = client.CheckFundTransfer(OrderID);

        //        if (model.error_code == 0)
        //        {
        //            if (model.Data.status == 0)
        //            {
        //                LogsManager.Info("远程确认" + (type == "1" ? "充值" : "提款") + "成功，billno：" + OrderID);

        //                var balanceData = Utility.GetBalance(TPGameUserID).Data;

        //                var availableScores = balanceData != null ? balanceData.balance.GetValueOrDefault() : -9999;
        //                var freezeScores = balanceData != null ? balanceData.outstanding.GetValueOrDefault() : -9999;

        //                //本地转账
        //                ResultModel model1 = CycleTryTransferSuccess(int.Parse(TransferID), type, Convert.ToInt32(UserID), availableScores, freezeScores);

        //                if (model1.IsSuccess)
        //                {
        //                    LogsManager.Info("本地" + (type == "1" ? "充值" : "提款") + "成功，billno：" + OrderID);

        //                    try
        //                    {
        //                        string summary = string.Empty;

        //                        if (type == "1")
        //                        {
        //                            #region 转账成功，设置会员投注限额

        //                            var result = client.GetMemberBetSetting(TPGameUserID);

        //                            if (result.error_code == 0)
        //                            {
        //                                foreach (var data in result.Data)
        //                                {
        //                                    data.min_bet = 20;
        //                                    data.max_bet = 20000;
        //                                    data.max_bet_per_match = 80000;
        //                                }

        //                                var result1 = client.SetMemberBetSetting(TPGameUserID, result.Data);

        //                                if (result1.error_code != 0)
        //                                {
        //                                    LogsManager.Info("设置会员 " + UserID.ToString() + $" 投注限额失败，详细信息：{result1.error_code}," + result1.message);
        //                                }
        //                                else
        //                                {
        //                                    LogsManager.Info("设置会员 " + UserID.ToString() + " 投注限额成功");
        //                                }
        //                            }
        //                            else
        //                            {
        //                                LogsManager.Info("获取会员 " + UserID.ToString() + " 投注限额失败，详细信息：" + result.message);
        //                            }

        //                            #endregion

        //                            summary = "您成功转账¥" + decimal.Parse(Money).ToString("N4") + "到体育账户";
        //                        }
        //                        else
        //                        {
        //                            summary = "您成功从体育账户提款¥" + decimal.Parse(Money).ToString("N4");
        //                        }

        //                        MessageEntity entity = new MessageEntity();
        //                        entity.MessageType = RabbitMessageType.TransferNotice;
        //                        entity.SendType = RQSettings.RQ_DIRECT_TYPE;
        //                        entity.SendTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        //                        entity.SendExchange = RQSettings.RQ_WCF_EXCHANGE;
        //                        entity.SendRoutKey = RQSettings.RQ_WCF_ROUTKEY;
        //                        var Item = new TransferNoticeEntity() { UserId = UserID, Amount = decimal.Parse(Money), Summary = summary };
        //                        entity.SendContent = Item;
        //                        _Manager.SendMessage(entity);
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        LogsManager.Info("发送转账消息异常，详细信息：" + ex.Message + "，堆栈：" + ex.StackTrace);
        //                    }
        //                }
        //                else
        //                {
        //                    LogsManager.InfoToEmail("本地" + (type == "1" ? "充值" : "提款") + "失败，billno：" + OrderID
        //                         + "，code：" + model1.Info + "，msg：" + model1.Msg);
        //                }

        //                break;
        //            }
        //            else if (model.Data.status == 1)
        //            {
        //                LogsManager.InfoToEmail("远程确认" + (type == "1" ? "充值" : "提款") + "失败，billno：" + OrderID
        //                     + "，error_code：" + model.error_code.ToString() + "，message：" + model.message
        //                     + "，datastatus：" + (model.Data != null ? model.Data.status.ToString() : string.Empty));

        //                //退还积分
        //                ResultModel model1 = CycleTryTransferRollback(int.Parse(TransferID), type, model.message, true);

        //                if (model1.IsSuccess)
        //                {
        //                    LogsManager.Info("回滚" + (type == "1" ? "充值" : "提款") + "成功，billno：" + OrderID
        //                         + "，error_code：" + model.error_code.ToString() + "，message：" + model.message
        //                         + "，datastatus：" + (model.Data != null ? model.Data.status.ToString() : string.Empty));
        //                }
        //                else
        //                {
        //                    LogsManager.InfoToEmail("回滚" + (type == "1" ? "充值" : "提款") + "失败，billno：" + OrderID
        //                         + "，code：" + model1.Info + "，msg：" + model1.Msg
        //                         + "，error_code：" + model.error_code.ToString() + "，message：" + model.message
        //                         + "，datastatus：" + (model.Data != null ? model.Data.status.ToString() : string.Empty));
        //                }

        //                #region 转账失败发送消息

        //                try
        //                {
        //                    string summary = string.Empty;

        //                    if (type == "1")
        //                    {
        //                        if (model1.IsSuccess)
        //                        {
        //                            summary = "转账¥" + decimal.Parse(Money).ToString("N4") + "到体育账户失败，请重试。";
        //                        }
        //                        else
        //                        {
        //                            summary = "转账¥" + decimal.Parse(Money).ToString("N4") + "到体育账户失败，请联系客服。";
        //                        }
        //                    }
        //                    else
        //                    {
        //                        summary = "从体育账户提款¥" + decimal.Parse(Money).ToString("N4") + "失败，请重试。";
        //                    }

        //                    MessageEntity entity = new MessageEntity();
        //                    entity.MessageType = RabbitMessageType.TransferNotice;
        //                    entity.SendType = RQSettings.RQ_DIRECT_TYPE;
        //                    entity.SendTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        //                    entity.SendExchange = RQSettings.RQ_WCF_EXCHANGE;
        //                    entity.SendRoutKey = RQSettings.RQ_WCF_ROUTKEY;
        //                    var Item = new TransferNoticeEntity() { UserId = UserID, Amount = decimal.Parse(Money), Summary = summary };
        //                    entity.SendContent = Item;
        //                    _Manager.SendMessage(entity);
        //                }
        //                catch (Exception ex)
        //                {
        //                    LogsManager.Info("发送转账消息异常，详细信息：" + ex.Message + "，堆栈：" + ex.StackTrace);
        //                }

        //                #endregion

        //                break;
        //            }
        //            else if (model.Data.status == 2)
        //            {
        //                if (tryCount >= 5)
        //                {
        //                    LogsManager.InfoToEmail("远程确认" + (type == "1" ? "充值" : "提款") + "失败，billno：" + OrderID
        //                                             + "，error_code：" + model.error_code.ToString() + "，message：" + model.message
        //                                             + "，datastatus：" + (model.Data != null ? model.Data.status.ToString() : string.Empty));

        //                    //标识订单为失败状态，但不退还积分
        //                    ResultModel model1 = CycleTryTransferRollback(int.Parse(TransferID), type, model.message, false);

        //                    if (model1.IsSuccess)
        //                    {
        //                        LogsManager.Info("标记失败" + (type == "1" ? "充值" : "提款") + "成功，billno：" + OrderID
        //                             + "，error_code：" + model.error_code.ToString() + "，message：" + model.message
        //                             + "，datastatus：" + (model.Data != null ? model.Data.status.ToString() : string.Empty));
        //                    }
        //                    else
        //                    {
        //                        LogsManager.InfoToEmail("标记失败" + (type == "1" ? "充值" : "提款") + "失败，billno：" + OrderID
        //                             + "，code：" + model1.Info + "，msg：" + model1.Msg
        //                             + "，error_code：" + model.error_code.ToString() + "，message：" + model.message
        //                             + "，datastatus：" + (model.Data != null ? model.Data.status.ToString() : string.Empty));
        //                    }

        //                    #region 转账失败发送消息

        //                    try
        //                    {
        //                        string summary = string.Empty;

        //                        if (type == "1")
        //                        {
        //                            summary = "转账¥" + decimal.Parse(Money).ToString("N4") + "到体育账户失败，请联系客服。";
        //                        }
        //                        else
        //                        {
        //                            summary = "从体育账户提款¥" + decimal.Parse(Money).ToString("N4") + "失败，请重试。";
        //                        }

        //                        MessageEntity entity = new MessageEntity();
        //                        entity.MessageType = RabbitMessageType.TransferNotice;
        //                        entity.SendType = RQSettings.RQ_DIRECT_TYPE;
        //                        entity.SendTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        //                        entity.SendExchange = RQSettings.RQ_WCF_EXCHANGE;
        //                        entity.SendRoutKey = RQSettings.RQ_WCF_ROUTKEY;
        //                        var Item = new TransferNoticeEntity() { UserId = UserID, Amount = decimal.Parse(Money), Summary = summary };
        //                        entity.SendContent = Item;
        //                        _Manager.SendMessage(entity);
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        LogsManager.Info("发送转账消息异常，详细信息：" + ex.Message + "，堆栈：" + ex.StackTrace);
        //                    }

        //                    #endregion

        //                    break;
        //                }
        //                else
        //                {
        //                    System.Threading.Thread.Sleep(5000);
        //                }
        //            }
        //            else
        //            {
        //                LogsManager.InfoToEmail("远程确认" + (type == "1" ? "充值" : "提款") + "失败，billno：" + OrderID
        //                     + "，error_code：" + model.error_code.ToString() + "，message：" + model.message
        //                     + "，datastatus：" + (model.Data != null ? model.Data.status.ToString() : string.Empty));

        //                //未知状态，标识订单为失败状态，但不退还积分
        //                ResultModel model1 = CycleTryTransferRollback(int.Parse(TransferID), type, model.message, false);

        //                if (model1.IsSuccess)
        //                {
        //                    LogsManager.Info("标记失败" + (type == "1" ? "充值" : "提款") + "成功，billno：" + OrderID
        //                         + "，error_code：" + model.error_code.ToString() + "，message：" + model.message
        //                         + "，datastatus：" + (model.Data != null ? model.Data.status.ToString() : string.Empty));
        //                }
        //                else
        //                {
        //                    LogsManager.InfoToEmail("标记失败" + (type == "1" ? "充值" : "提款") + "失败，billno：" + OrderID
        //                         + "，code：" + model1.Info + "，msg：" + model1.Msg
        //                         + "，error_code：" + model.error_code.ToString() + "，message：" + model.message
        //                         + "，datastatus：" + (model.Data != null ? model.Data.status.ToString() : string.Empty));
        //                }

        //                #region 转账失败发送消息

        //                try
        //                {
        //                    string summary = string.Empty;

        //                    if (type == "1")
        //                    {
        //                        summary = "转账¥" + decimal.Parse(Money).ToString("N4") + "到体育账户失败，请联系客服。";
        //                    }
        //                    else
        //                    {
        //                        summary = "从体育账户提款¥" + decimal.Parse(Money).ToString("N4") + "失败，请重试。";
        //                    }

        //                    MessageEntity entity = new MessageEntity();
        //                    entity.MessageType = RabbitMessageType.TransferNotice;
        //                    entity.SendType = RQSettings.RQ_DIRECT_TYPE;
        //                    entity.SendTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        //                    entity.SendExchange = RQSettings.RQ_WCF_EXCHANGE;
        //                    entity.SendRoutKey = RQSettings.RQ_WCF_ROUTKEY;
        //                    var Item = new TransferNoticeEntity() { UserId = UserID, Amount = decimal.Parse(Money), Summary = summary };
        //                    entity.SendContent = Item;
        //                    _Manager.SendMessage(entity);
        //                }
        //                catch (Exception ex)
        //                {
        //                    LogsManager.Info("发送转账消息异常，详细信息：" + ex.Message + "，堆栈：" + ex.StackTrace);
        //                }

        //                #endregion

        //                break;
        //            }
        //        }
        //        else if (model.error_code == -1)//网络异常
        //        {
        //            if (tryCount >= 5)
        //            {
        //                LogsManager.InfoToEmail("远程确认" + (type == "1" ? "充值" : "提款") + "失败，billno：" + OrderID
        //                     + "，error_code：" + model.error_code.ToString() + "，message：" + model.message
        //                     + "，datastatus：" + (model.Data != null ? model.Data.status.ToString() : string.Empty));

        //                //未知状态，标识订单为失败状态，但不退还积分
        //                ResultModel model1 = CycleTryTransferRollback(int.Parse(TransferID), type, model.message, false);

        //                if (model1.IsSuccess)
        //                {
        //                    LogsManager.Info("标记失败" + (type == "1" ? "充值" : "提款") + "成功，billno：" + OrderID
        //                         + "，error_code：" + model.error_code.ToString() + "，message：" + model.message
        //                         + "，datastatus：" + (model.Data != null ? model.Data.status.ToString() : string.Empty));
        //                }
        //                else
        //                {
        //                    LogsManager.InfoToEmail("标记失败" + (type == "1" ? "充值" : "提款") + "失败，billno：" + OrderID
        //                         + "，code：" + model1.Info + "，msg：" + model1.Msg
        //                         + "，error_code：" + model.error_code.ToString() + "，message：" + model.message
        //                         + "，datastatus：" + (model.Data != null ? model.Data.status.ToString() : string.Empty));
        //                }

        //                #region 转账失败发送消息

        //                try
        //                {
        //                    string summary = string.Empty;

        //                    if (type == "1")
        //                    {
        //                        summary = "转账¥" + decimal.Parse(Money).ToString("N4") + "到体育账户失败，请联系客服。";
        //                    }
        //                    else
        //                    {
        //                        summary = "从体育账户提款¥" + decimal.Parse(Money).ToString("N4") + "失败，请重试。";
        //                    }

        //                    MessageEntity entity = new MessageEntity();
        //                    entity.MessageType = RabbitMessageType.TransferNotice;
        //                    entity.SendType = RQSettings.RQ_DIRECT_TYPE;
        //                    entity.SendTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        //                    entity.SendExchange = RQSettings.RQ_WCF_EXCHANGE;
        //                    entity.SendRoutKey = RQSettings.RQ_WCF_ROUTKEY;
        //                    var Item = new TransferNoticeEntity() { UserId = UserID, Amount = decimal.Parse(Money), Summary = summary };
        //                    entity.SendContent = Item;
        //                    _Manager.SendMessage(entity);
        //                }
        //                catch (Exception ex)
        //                {
        //                    LogsManager.Info("发送转账消息异常，详细信息：" + ex.Message + "，堆栈：" + ex.StackTrace);
        //                }

        //                #endregion

        //                break;
        //            }
        //            else
        //            {
        //                Thread.Sleep(5000);
        //            }
        //        }
        //        else
        //        {
        //            LogsManager.InfoToEmail("远程确认" + (type == "1" ? "充值" : "提款") + "失败，billno：" + OrderID
        //                     + "，error_code：" + model.error_code.ToString() + "，message：" + model.message
        //                     + "，datastatus：" + (model.Data != null ? model.Data.status.ToString() : string.Empty));

        //            //退还积分
        //            ResultModel model1 = CycleTryTransferRollback(int.Parse(TransferID), type, model.message, true);

        //            if (model1.IsSuccess)
        //            {
        //                LogsManager.Info("回滚" + (type == "1" ? "充值" : "提款") + "成功，billno：" + OrderID
        //                     + "，error_code：" + model.error_code.ToString() + "，message：" + model.message
        //                     + "，datastatus：" + (model.Data != null ? model.Data.status.ToString() : string.Empty));
        //            }
        //            else
        //            {
        //                LogsManager.InfoToEmail("回滚" + (type == "1" ? "充值" : "提款") + "失败，billno：" + OrderID
        //                     + "，code：" + model1.Info + "，msg：" + model1.Msg
        //                     + "，error_code：" + model.error_code.ToString() + "，message：" + model.message
        //                     + "，datastatus：" + (model.Data != null ? model.Data.status.ToString() : string.Empty));
        //            }

        //            #region 转账失败发送消息

        //            try
        //            {
        //                string summary = string.Empty;

        //                if (type == "1")
        //                {
        //                    if (model1.IsSuccess)
        //                    {
        //                        summary = "转账¥" + decimal.Parse(Money).ToString("N4") + "到体育账户失败，请重试。";
        //                    }
        //                    else
        //                    {
        //                        summary = "转账¥" + decimal.Parse(Money).ToString("N4") + "到体育账户失败，请联系客服。";
        //                    }
        //                }
        //                else
        //                {
        //                    summary = "从体育账户提款¥" + decimal.Parse(Money).ToString("N4") + "失败，请重试。";
        //                }

        //                MessageEntity entity = new MessageEntity();
        //                entity.MessageType = RabbitMessageType.TransferNotice;
        //                entity.SendType = RQSettings.RQ_DIRECT_TYPE;
        //                entity.SendTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        //                entity.SendExchange = RQSettings.RQ_WCF_EXCHANGE;
        //                entity.SendRoutKey = RQSettings.RQ_WCF_ROUTKEY;
        //                var Item = new TransferNoticeEntity() { UserId = UserID, Amount = decimal.Parse(Money), Summary = summary };
        //                entity.SendContent = Item;
        //                _Manager.SendMessage(entity);
        //            }
        //            catch (Exception ex)
        //            {
        //                LogsManager.Info("发送转账消息异常，详细信息：" + ex.Message + "，堆栈：" + ex.StackTrace);
        //            }

        //            #endregion

        //            break;
        //        }
        //    }
        //}

        public ApiResult<FundTransferResult> FundTransfer(SportApiParamModel apiParam, ref string apiResult)
        {
            //string VendorID, string Url, string Currency, string type, string Userid, string OrderID, string Money
            ApiClient client = new ApiClient();

            ApiResult<FundTransferResult> result = null;
            result = client.FundTransfer(apiParam.TPGameUserID, apiParam.OrderID, apiParam.Money, apiParam.Type, ref apiResult);
            return result;
        }

        public void RefreshAvailableScores(bool isConsole)
        {
            if (isConsole)
            {
                ConfigurationManager.RefreshSection("appSettings");
            }

            var userInfos = new List<SportUserInfo>();

            try
            {
                userInfos = UserDal.GetUserInfo();
            }
            catch (Exception ex)
            {
                if (!isConsole)
                {
                    LogsManager.InfoToEmail("获取待更新用户失败，详细信息：" + ex.Message);
                }
                else
                {
                    Console.WriteLine("获取待更新用户失败，详细信息：" + ex.Message);
                }
            }
            if (!isConsole)
            {
                LogsManager.Info("获取到 " + userInfos.Count.ToString() + " 个待更新用户");
            }
            else
            {
                Console.WriteLine("获取到 " + userInfos.Count.ToString() + " 个待更新用户");
            }

            foreach (SportUserInfo userInfo in userInfos)
            {
                //分環境取得要打過去沙巴的UserId
                BaseReturnDataModel<string> returnModel = TPGameAccountReadService.GetTPGameAccountByLocalAccount(userInfo.UserID, PlatformProduct.Sport);
                
                if (!returnModel.IsSuccess)
                {
                    continue;
                }

                decimal availableScores = -9999;
                decimal freezeScores = -9999;

                GetUserBalanceInfo(returnModel.DataModel, ref availableScores, ref freezeScores);

                if (availableScores != -9999)
                {
                    userInfo.AvailableScores = availableScores;
                    userInfo.FreezeScores = freezeScores;

                    try
                    {
                        if (UserDal.UpdateAvailableScores(userInfo))
                        {
                            if (!isConsole)
                            {
                                LogsManager.Info("更新用户 " + userInfo.UserName + " 余额为 " + availableScores.ToString() + "，冻结金额为 " + freezeScores.ToString());
                            }
                            else
                            {
                                Console.WriteLine("更新用户 " + userInfo.UserName + " 余额为 " + availableScores.ToString() + "，冻结金额为 " + freezeScores.ToString());
                            }
                        }
                        else
                        {
                            if (!isConsole)
                            {
                                LogsManager.InfoToEmail("更新用户 " + userInfo.UserName + " 余额失败，未能成功更新余额");
                            }
                            else
                            {
                                Console.WriteLine("更新用户 " + userInfo.UserName + " 余额失败，未能成功更新余额");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        if (!isConsole)
                        {
                            LogsManager.InfoToEmail("更新用户 " + userInfo.UserName + " 余额失败，详细信息：" + ex.Message);
                        }
                        else
                        {
                            Console.WriteLine("更新用户 " + userInfo.UserName + " 余额失败，详细信息：" + ex.Message);
                        }
                    }
                }
                else
                {
                    if (!isConsole)
                    {
                        LogsManager.InfoToEmail("获取用户 " + userInfo.UserName + " 余额失败，未能成功更新余额");
                    }
                    else
                    {
                        Console.WriteLine("获取用户 " + userInfo.UserName + " 余额失败，未能成功更新余额");
                    }
                }
                Thread.Sleep(1000);
            }

            if (!isConsole)
            {
                LogsManager.Info("更新用户余额完毕");
            }
            else
            {
                Console.WriteLine("更新用户余额完毕");
                Console.ReadLine();
            }
        }

        private void GetUserBalanceInfo(string userId, ref decimal availableScores, ref decimal freezeScores)
        {
            ApiResult<UserBalanceItem> balance = Utility.GetBalance(userId);

            if (balance != null && balance.Data != null && balance.Data.error_code == 0)
            {
                availableScores = balance.Data.balance.GetValueOrDefault();
                freezeScores = balance.Data.outstanding.GetValueOrDefault();
            }
        }

        /// <summary>
        /// 循环尝试数据回滚
        /// </summary>
        private ResultModel CycleTryTransferRollback(int TransferID, string ActionType, string Msg)
        {
            int tryCount = 0;
            ResultModel result = new ResultModel();

            while (true)
            {
                tryCount++;

                try
                {
                    bool isRollback = true;
                    result.IsSuccess = new SportDataBase.DLL.SportMoneyInInfo().SportTransferRollback(TransferID, ActionType, Msg, isRollback);

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

                if (!result.IsSuccess)
                {
                    if (tryCount >= 5)
                    {
                        break;
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(5000);
                    }
                }
                else
                {
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// 循环尝试转账成功时数据处理
        /// </summary>
        /// <param name="TransferID"></param>
        /// <param name="ActionType"></param>
        private ResultModel CycleTryTransferSuccess(int TransferID, string ActionType, int userId, decimal availableScores, decimal freezeScores)
        {
            int tryCount = 0;
            ResultModel result = new ResultModel();

            while (true)
            {
                tryCount++;

                try
                {
                    result.IsSuccess = new SportDataBase.DLL.SportMoneyOutInfo().SportTransferSuccess(TransferID, ActionType, userId, availableScores, freezeScores);

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

                if (!result.IsSuccess)
                {
                    if (tryCount >= 5)
                    {
                        break;
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(5000);
                    }
                }
                else
                {
                    break;
                }
            }

            return result;
        }

        private void SetUserBetAmountLimit(SportApiParamModel apiParam)
        {
            var client = new ApiClient();
            ApiResult<List<BetSettingItem>> betSettingResult = client.GetMemberBetSetting(apiParam.TPGameUserID);

            if (betSettingResult == null || betSettingResult.error_code != 0)
            {
                string errorMsg = $"获取会员 {apiParam.UserID} 投注限额失败";

                if (betSettingResult != null)
                {
                    errorMsg += $"，详细信息：{betSettingResult.error_code},{betSettingResult.message}";
                }

                LogsManager.Error(errorMsg);
                return;
            }

            if (betSettingResult.error_code == 0)
            {
                foreach (BetSettingItem data in betSettingResult.Data)
                {
                    data.min_bet = 20;
                    data.max_bet = 20000;
                    data.max_bet_per_match = 80000;
                }

                ApiResult setMemberBetSettingResult = client.SetMemberBetSetting(apiParam.TPGameUserID, betSettingResult.Data);

                if (setMemberBetSettingResult == null || setMemberBetSettingResult.error_code != 0)
                {
                    string errorMsg = $"设置会员 {apiParam.UserID} 投注限额失败";

                    if (setMemberBetSettingResult != null)
                    {
                        errorMsg += $"，详细信息：{setMemberBetSettingResult.error_code},{setMemberBetSettingResult.message}";
                    }

                    LogsManager.Error(errorMsg);
                }
                else
                {
                    LogsManager.ForcedDebug($"设置会员 {apiParam.UserID} 投注限额成功");
                }
            }
        }
    }
}
