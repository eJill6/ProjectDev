using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Repository.Base;
using JxBackendService.Repository.Extensions;
using System.Data;

namespace JxBackendService.Repository.StoredProcedure
{
    public abstract class OldTPGameStoredProcedureRep : BaseTPGameStoredProcedureRep
    {
        public OldTPGameStoredProcedureRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        public override string GetTransferSpActionType(bool isMoneyIn)
        {
            if (isMoneyIn)
            {
                return "2";
            }
            else
            {
                return "3";
            }
        }

        public override bool DoTransferSuccess(bool isMoneyIn, BaseTPGameMoneyInfo tpGameMoneyInfo, UserScore userScore)
        {
            string sql = $"{InlodbType.Inlodb.Value}.dbo.{TransferSuccessSpName}";

            return DbHelper.ExecuteScalar<string>(
                sql,
                new
                {
                    TransferID = tpGameMoneyInfo.GetMoneyID().ToVarchar(32),
                    ActionType = GetTransferSpActionType(isMoneyIn).ToNVarchar(20),
                    tpGameMoneyInfo.UserID,
                    userScore.AvailableScores,
                    userScore.FreezeScores
                },
                CommandType.StoredProcedure) == _transferSpSuccessCode;
        }

        public override bool DoTransferRollback(bool isMoneyIn, BaseTPGameMoneyInfo tpGameMoneyInfo, string msg)
        {
            string sql = $"{InlodbType.Inlodb.Value}.dbo.{TransferRollbackSpName}";

            return DbHelper.ExecuteScalar<string>(
                sql,
                new
                {
                    TransferID = tpGameMoneyInfo.GetMoneyID().ToVarchar(32),
                    ActionType = GetTransferSpActionType(isMoneyIn),
                    msg = msg.ToNVarchar(1024),
                    RollBack = true
                },
                CommandType.StoredProcedure) == _transferSpSuccessCode;
        }
    }
}