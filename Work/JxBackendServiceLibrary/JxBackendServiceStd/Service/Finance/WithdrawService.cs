using JxBackendService.Common.Util;
using JxBackendService.Interface.Repository.Finance;
using JxBackendService.Interface.Service.Finance;
using JxBackendService.Interface.Service.MiseLive;
using JxBackendService.Interface.Service.User;
using JxBackendService.Model.Entity.Finance;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.Finance;
using JxBackendService.Model.MiseLive.Request;
using JxBackendService.Model.MiseLive.Response;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.StoredProcedureParam.Finance;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;
using JxBackendService.Service.Base;
using System;
using System.Collections.Generic;

namespace JxBackendService.Service.Finance
{
    public class WithdrawService : BaseService, IWithdrawService
    {
        private readonly ICMoneyOutInfoRep _cmoneyOutInfoRep;

        private readonly IProfitLossRep _profitLossRep;

        private readonly IBudgetLogsRep _budgetLogsRep;

        private readonly IMiseLiveApiService _miseLiveApiService;

        private readonly IUserInfoRelatedService _userInfoRelatedService;

        public WithdrawService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _cmoneyOutInfoRep = ResolveJxBackendService<ICMoneyOutInfoRep>();
            _profitLossRep = ResolveJxBackendService<IProfitLossRep>();
            _budgetLogsRep = ResolveJxBackendService<IBudgetLogsRep>();
            _miseLiveApiService = ResolveJxBackendService<IMiseLiveApiService>();
            _userInfoRelatedService = ResolveJxBackendService<IUserInfoRelatedService>();
        }

        public BaseReturnModel WithdrawToMiseLive(decimal amount, string productCode)
        {
            int userId = EnvLoginUser.LoginUser.UserId;

            //判斷餘額
            decimal userAvailableScores = _userInfoRelatedService.GetUserAvailableScores(userId);

            if (userAvailableScores < amount)
            {
                return new BaseReturnModel(ReturnCode.BalanceInsufficient, userAvailableScores.ToString());
            }

            var createParam = new CreateCMoneyOutInfoParam
            {
                UserID = userId,
                Amount = amount,
                ProductCode = productCode
            };

            BaseReturnDataModel<BasicCMoneyOutInfo> returnDataModel = CreateProcessingCMoneyOut(createParam);

            if (!returnDataModel.IsSuccess)
            {
                return returnDataModel.CastByJson<BaseReturnModel>();
            }

            var miseLiveTransferRequestParam = new MiseLiveTransferRequestParam()
            {
                Amount = amount,
                OrderNo = returnDataModel.DataModel.OrderID,
                UserId = userId
            };

            MiseLiveResponse<MiseLiveBalance> response = _miseLiveApiService.TransferIn(miseLiveTransferRequestParam);
            MoneyOutDealType moneyOutDealType = MoneyOutDealType.Refunded;
            BudgetType budgetType = BudgetType.WithdrawRefund;

            if (response.Success)
            {
                moneyOutDealType = MoneyOutDealType.Done;
                budgetType = BudgetType.WithdrawConfirm;
            }

            BaseReturnModel processReturnModel = ProcessCMoneyOut(new ProcessCMoneyOutParam()
            {
                UserID = userId,
                OrderID = miseLiveTransferRequestParam.OrderNo,
                MoneyOutDealType = moneyOutDealType,
                BudgetType = budgetType,
                ProfitLossType = ProfitLossTypeName.TX
            });

            if (!processReturnModel.IsSuccess)
            {
                return processReturnModel;
            }

            if (!response.Success)
            {
                return new BaseReturnModel(response.Error);
            }

            return new BaseReturnModel(ReturnCode.TransferMoneySuccess);
        }

        public void RecheckWithdrawOrdersFromMiseLive()
        {
            List<CMoneyOutInfo> cmoneyOutInfos = _cmoneyOutInfoRep.GetProcessingOrders3DaysAgo();

            foreach (CMoneyOutInfo cmoneyOutInfo in cmoneyOutInfos)
            {
                var request = new MiseLiveTransferOrderRequestParam() { OrderNo = cmoneyOutInfo.OrderID };
                MiseLiveResponse<MiseLiveTransferOrder> response = _miseLiveApiService.GetTransferOrderResult(request);
                MoneyOutDealType moneyOutDealType = MoneyOutDealType.Refunded;
                BudgetType budgetType = BudgetType.WithdrawRefund;

                if (response.Success && response.Data.Success)
                {
                    moneyOutDealType = MoneyOutDealType.Done;
                    budgetType = BudgetType.WithdrawConfirm;
                }

                var processCMoneyOutParam = new ProcessCMoneyOutParam()
                {
                    UserID = cmoneyOutInfo.UserID,
                    OrderID = cmoneyOutInfo.OrderID,
                    MoneyOutDealType = moneyOutDealType,
                    BudgetType = budgetType,
                    ProfitLossType = ProfitLossTypeName.TX
                };

                BaseReturnModel processReturnModel = ProcessCMoneyOut(processCMoneyOutParam);

                if (!processReturnModel.IsSuccess)
                {
                    ErrorMsgUtil.ErrorHandle(
                        new Exception($"ProcessCMoneyOut Fail: OrderID={cmoneyOutInfo.OrderID},Msg={processReturnModel.Message}"),
                        EnvLoginUser);
                }
            }
        }

        private BaseReturnModel ProcessCMoneyOut(ProcessCMoneyOutParam param)
        {
            string budgetId = _budgetLogsRep.CreateBudgetID();
            string profitLossId = _profitLossRep.CreateProfitLossID();

            var proProcessCMoneyOutParam = new ProProcessCMoneyOutParam()
            {
                UserID = param.UserID,
                OrderID = param.OrderID,
                Handler = EnvLoginUser.LoginUser.UserId.ToString(),
                Memo = param.BudgetType.Name,
                BudgetID = budgetId,
                ProfitLossID = profitLossId,
            };

            proProcessCMoneyOutParam.SetMoneyOutDealType(param.MoneyOutDealType);
            proProcessCMoneyOutParam.SetProfitLossType(param.ProfitLossType);
            proProcessCMoneyOutParam.SetBudgetType(param.BudgetType);

            return _cmoneyOutInfoRep.ProcessCMoneyOut(proProcessCMoneyOutParam);
        }

        private BaseReturnDataModel<BasicCMoneyOutInfo> CreateProcessingCMoneyOut(CreateCMoneyOutInfoParam createParam)
        {
            string moneyOutId = _cmoneyOutInfoRep.CreateMoneyID();
            string budgetId = _budgetLogsRep.CreateBudgetID();
            string orderId = CreateOrderID(moneyOutId);
            BudgetType budgetType = BudgetType.WithdrawApply;

            var proCreateCMoneyOutInfoParam = new ProCreateCMoneyOutInfoParam()
            {
                MoneyOutID = moneyOutId,
                UserID = createParam.UserID,
                Amount = createParam.Amount,
                OrderID = orderId,
                Handler = createParam.UserID.ToString(),
                BudgetID = budgetId,
                Memo = budgetType.Name,
                ProductCode = createParam.ProductCode,
            };

            proCreateCMoneyOutInfoParam.SetMoneyOutDealType(MoneyOutDealType.Processing);
            proCreateCMoneyOutInfoParam.SetBudgetType(budgetType);

            BaseReturnModel returnDataModel = _cmoneyOutInfoRep.CreateCMoneyOutInfo(proCreateCMoneyOutInfoParam);

            if (!returnDataModel.IsSuccess)
            {
                return new BaseReturnDataModel<BasicCMoneyOutInfo>(returnDataModel.Message);
            }

            return new BaseReturnDataModel<BasicCMoneyOutInfo>(
                ReturnCode.Success,
                new BasicCMoneyOutInfo()
                {
                    UserID = proCreateCMoneyOutInfoParam.UserID,
                    MoneyOutID = proCreateCMoneyOutInfoParam.MoneyOutID,
                    DealType = proCreateCMoneyOutInfoParam.MoneyOutDealType,
                    OrderID = proCreateCMoneyOutInfoParam.OrderID
                });
        }

        private string CreateOrderID(string moneyOutId)
        {
            return $"{Merchant.Value}{BaseTPGameStoredProcedureRep.WithdrawActionCode}{EnvLoginUser.EnvironmentCode.OrderPrefixCode}{moneyOutId}";
        }
    }
}