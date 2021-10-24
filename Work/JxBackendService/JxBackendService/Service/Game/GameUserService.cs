using JxBackendService.Common.Util;
using JxBackendService.Interface.Repository;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Enums;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Param;
using JxBackendService.Model.Param.ThirdParty;
using JxBackendService.Model.StoredProcedureParam;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.Game;
using JxBackendService.Service.Base;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace JxBackendService.Service.Game
{
    public class GameUserService : BaseService, IGameUserService
    {
        private readonly IUserReportProfitLossRep _userReportProfitLossRep;

        public GameUserService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _userReportProfitLossRep = ResolveJxBackendService<IUserReportProfitLossRep>();
        }

        public void CreateLoginHistory(PlatformProduct platformProduct, int type, JxIpInformation ipinformation)
        {
            ITPGameStoredProcedureRep tpGameStoredProcedureRep = ResolveJxBackendService<ITPGameStoredProcedureRep>(platformProduct);

            tpGameStoredProcedureRep.CreateLoginHistory(new TPGameCreateLoginHistoryParam()
            {
                Type = type,
                UserID = EnvLoginUser.LoginUser.UserId,
                UserName = EnvLoginUser.LoginUser.UserName,
                Ipinformation = ipinformation
            });
        }

        public UserReportProfitLossResult GetUserReportProfitloss(RequestUserReportProfitlossParam param)
        {
            var platformProductService = ResolveKeyed<IPlatformProductService>(EnvLoginUser.Application);
            var spParam = param.CastByJson<ProGetUserReportProfitlossParam>();
            spParam.CommissionTypeJson = platformProductService.GetSortedPlatformProduct().Select(s => s.Value).ToArray().ToJsonString();
            UserReportProfitLossResult userReportProfitLossResult = _userReportProfitLossRep.GetUserReportProfitloss(spParam);
            return userReportProfitLossResult;
        }
    }
}
