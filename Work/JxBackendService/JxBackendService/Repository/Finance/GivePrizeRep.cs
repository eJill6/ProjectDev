using JxBackendService.Common.Util;
using JxBackendService.Interface.Repository.Finance;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Param.Finance;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace JxBackendService.Repository.Finance
{
    public class GivePrizeRep : BaseDbRepository, IGivePrizeRep
    {
        public GivePrizeRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {

        }

        public BaseReturnModel GivePrizesByCustomerType(GivePrizesByCustomerTypeParam saveParam)
        {
            string sql = $"{InlodbType.Inlodb}.dbo.Pro_GivePrizesByCustomerType";

            int? bankTypeID = null;
            string bankTypeName = null;

            if (saveParam.BankType != null)
            {
                bankTypeID = saveParam.BankType.BankTypeID;
                bankTypeName = saveParam.BankType.BankTypeName;
            }

            object param = new
            {
                saveParam.UserID,
                WalletType = saveParam.WalletType.Value,
                saveParam.PrizesMoney,
                saveParam.FlowMultiple,
                bankTypeID,
                bankTypeName,
                RefundType = saveParam.RefundTypeParam.Value,
                RefundTypeName = saveParam.RefundTypeParam.Name,
                ProfitLossType = saveParam.ProfitLossType.Value,
                Memo = saveParam.MemoJsonParam.ToLocalizationContent(),
                MemoJson = saveParam.MemoJsonParam.ToJsonString(),
                UpdateUser = EnvLoginUser.LoginUser.UserName,
                RC_Success = ReturnCode.Success.Value,
                RC_SystemError = ReturnCode.SystemError.Value
            };

            SPReturnModel returnModel = DbHelper.QuerySingle<SPReturnModel>(sql, param, CommandType.StoredProcedure);

            return new BaseReturnModel(ReturnCode.GetSingle(returnModel.ReturnCode));
        }
    }
}
