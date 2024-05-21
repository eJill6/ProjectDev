using JxBackendService.Common.Util;
using JxBackendService.Interface.Repository.Finance;
using JxBackendService.Interface.Service.Finance;
using JxBackendService.Interface.Service.MiseLive;
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
    public class RechargeService : BaseService, IRechargeService
    {
        private readonly ICMoneyInInfoRep _cmoneyInInfoRep;

        private readonly ICMoneyInInfoRep _cmoneyInInfoReadRep;

        private readonly IProfitLossRep _profitLossRep;

        private readonly IBudgetLogsRep _budgetLogsRep;

        private readonly IMiseLiveApiService _miseLiveApiService;

        public RechargeService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _cmoneyInInfoRep = ResolveJxBackendService<ICMoneyInInfoRep>();
            _cmoneyInInfoReadRep = ResolveJxBackendService<ICMoneyInInfoRep>(DbConnectionTypes.Slave);
            _profitLossRep = ResolveJxBackendService<IProfitLossRep>();
            _budgetLogsRep = ResolveJxBackendService<IBudgetLogsRep>();
            _miseLiveApiService = ResolveJxBackendService<IMiseLiveApiService>();
        }

        public BaseReturnModel RechargeAllFromMiseLive()
        {
            int userId = EnvLoginUser.LoginUser.UserId;
            MiseLiveResponse<MiseLiveBalance> balanceResponse = _miseLiveApiService.GetUserBalance(new MiseLiveUserBalanceRequestParam() { UserId = userId });

            if (!balanceResponse.Success)
            {
                return new BaseReturnModel(balanceResponse.Error);
            }

            decimal amount = balanceResponse.Data.Balance;

            if (amount < GlobalVariables.TPTransferAmountBound.MinTPGameTransferAmount)
            {
                return new BaseReturnModel(ReturnCode.Success);
            }

            if (amount > GlobalVariables.TPTransferAmountBound.MaxTPGameTransferAmount)
            {
                amount = GlobalVariables.TPTransferAmountBound.MaxTPGameTransferAmount;
            }

            BaseReturnDataModel<CMoneyInInfo> returnDataModel = CreateProcessingCMoneyIn(amount);

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

            MiseLiveResponse<MiseLiveBalance> response = _miseLiveApiService.TransferOut(miseLiveTransferRequestParam);
            MoneyInDealType moneyInDealType = MoneyInDealType.Fail;

            if (response.Success)
            {
                moneyInDealType = MoneyInDealType.Done;
            }

            var processCMoneyInParam = new ProcessCMoneyInParam()
            {
                UserID = userId,
                OrderID = miseLiveTransferRequestParam.OrderNo,
                MoneyInDealType = moneyInDealType,
                BudgetType = BudgetType.Recharge,
                ProfitLossType = ProfitLossTypeName.CZ
            };

            BaseReturnModel processReturnModel = ProcessCMoneyIn(processCMoneyInParam);

            if (!processReturnModel.IsSuccess)
            {
                return processReturnModel;
            }

            if (!response.Success)
            {
                return new BaseReturnModel(response.Error);
            }

            return new BaseReturnModel(ReturnCode.Success);
        }

        public void RecheckOrdersFromMiseLive()
        {
            List<CMoneyInInfo> cmoneyInInfos = _cmoneyInInfoReadRep.GetProcessingOrders3DaysAgo();

            foreach (CMoneyInInfo cmoneyInInfo in cmoneyInInfos)
            {
                var request = new MiseLiveTransferOrderRequestParam() { OrderNo = cmoneyInInfo.OrderID };
                MiseLiveResponse<MiseLiveTransferOrder> response = _miseLiveApiService.GetTransferOrderResult(request);
                MoneyInDealType moneyInDealType = MoneyInDealType.Fail;

                if (response.Success && response.Data.Success)
                {
                    moneyInDealType = MoneyInDealType.Done;
                }

                var processCMoneyInParam = new ProcessCMoneyInParam()
                {
                    UserID = cmoneyInInfo.UserID,
                    OrderID = cmoneyInInfo.OrderID,
                    MoneyInDealType = moneyInDealType,
                    BudgetType = BudgetType.Recharge,
                    ProfitLossType = ProfitLossTypeName.CZ
                };

                BaseReturnModel processReturnModel = ProcessCMoneyIn(processCMoneyInParam);

                if (!processReturnModel.IsSuccess)
                {
                    ErrorMsgUtil.ErrorHandle(
                        new Exception($"ProcessCMoneyIn Fail: OrderID={cmoneyInInfo.OrderID},Msg={processReturnModel.Message}"),
                        EnvLoginUser);
                }
            }
        }

        private BaseReturnDataModel<CMoneyInInfo> CreateProcessingCMoneyIn(decimal amount)
        {
            string moneyInId = _cmoneyInInfoRep.CreateMoneyID();
            string orderId = CreateOrderID(moneyInId);

            var cmoneyInInfo = new CMoneyInInfo()
            {
                MoneyInID = moneyInId,
                UserID = EnvLoginUser.LoginUser.UserId,
                Amount = amount,
                OrderID = orderId,
                OrderTime = DateTime.Now,
                DealType = MoneyInDealType.Processing.Value,
                Handler = EnvLoginUser.LoginUser.UserId.ToString()
            };

            BaseReturnModel returnDataModel = _cmoneyInInfoRep.CreateByProcedure(cmoneyInInfo);

            if (!returnDataModel.IsSuccess)
            {
                return new BaseReturnDataModel<CMoneyInInfo>(returnDataModel.Message);
            }

            return new BaseReturnDataModel<CMoneyInInfo>(ReturnCode.Success, cmoneyInInfo);
        }

        private BaseReturnModel ProcessCMoneyIn(ProcessCMoneyInParam param)
        {
            string budgetId = _budgetLogsRep.CreateBudgetID();
            string profitLossId = _profitLossRep.CreateProfitLossID();

            var proProcessCMoneyInParam = new ProProcessCMoneyInParam()
            {
                UserID = param.UserID,
                OrderID = param.OrderID,
                Handler = EnvLoginUser.LoginUser.UserId.ToString(),
                Memo = param.BudgetType.Name,
                BudgetID = budgetId,
                ProfitLossID = profitLossId,
            };

            proProcessCMoneyInParam.SetMoneyInDealType(param.MoneyInDealType);
            proProcessCMoneyInParam.SetProfitLossType(param.ProfitLossType);
            proProcessCMoneyInParam.SetBudgetType(param.BudgetType);

            return _cmoneyInInfoRep.ProcessCMoneyIn(proProcessCMoneyInParam);
        }

        private string CreateOrderID(string moneyInId)
        {
            return $"{Merchant.Value}{BaseTPGameStoredProcedureRep.DepositActionCode}{EnvLoginUser.EnvironmentCode.OrderPrefixCode}{moneyInId}";
        }
    }
}