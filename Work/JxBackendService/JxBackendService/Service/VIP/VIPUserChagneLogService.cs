using JxBackendService.Common.Util;
using JxBackendService.Interface.Repository.VIP;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.VIP;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.User;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.VIP;
using JxBackendService.Service.Base;

namespace JxBackendService.Service.VIP
{
    public class VIPUserChagneLogService : BaseService, IVIPUserChangeLogService
    {
        private readonly IVIPUserChangeLogRep _vipUserChangeLogRep;
        private readonly IUserInfoRelatedService _userInfoRelatedService;

        public VIPUserChagneLogService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _vipUserChangeLogRep = ResolveJxBackendService<IVIPUserChangeLogRep>();
            _userInfoRelatedService = ResolveJxBackendService<IUserInfoRelatedService>();
        }

        /// <summary><see cref="IVIPUserChangeLogService.GetVIPPointsChangeLogs"/></summary>
        public PagedResultModel<VIPPointsChangeLogModel> GetVIPPointsChangeLogs(BaseUserScoreSearchParam searchParam)
        {
            //TODO 差異化實作判斷是否為下級 vip為一層
            //TODO 是否檢查的設定改到application enum內
            //檢查是否為下級, 
            if (!_userInfoRelatedService.CheckUserIdInUserPath(EnvLoginUser.LoginUser.UserId, searchParam.UserID))
            {
                return new PagedResultModel<VIPPointsChangeLogModel>()
                {
                    PageNo = searchParam.PageNum,
                    PageSize = searchParam.PageSize
                };
            }

            //前台要處理結尾時間
            searchParam.EndDate = searchParam.EndDate.ToQuerySmallEqualThanTime(DatePeriods.Day);

            return _vipUserChangeLogRep.GetVIPPointsChangeLogs(searchParam);
        }

        /// <summary><see cref="IVIPUserChangeLogService.GetVIPFlowChangeLogs"/></summary>
        public PagedResultModel<VIPFlowChangeLogModel> GetVIPFlowChangeLogs(BaseUserScoreSearchParam searchParam)
        {
            //TODO 差異化實作判斷是否為下級 vip為一層
            //TODO 是否檢查的設定改到application enum內
            //檢查是否為下級, 
            if (!_userInfoRelatedService.CheckUserIdInUserPath(EnvLoginUser.LoginUser.UserId, searchParam.UserID))
            {
                return new PagedResultModel<VIPFlowChangeLogModel>()
                {
                    PageNo = searchParam.PageNum,
                    PageSize = searchParam.PageSize
                };
            }

            //前台要處理結尾時間
            searchParam.EndDate = searchParam.EndDate.ToQuerySmallEqualThanTime(DatePeriods.Day);

            return _vipUserChangeLogRep.GetVIPFlowChangeLogs(searchParam);
        }

        /// <summary><see cref="IVIPUserChangeLogService.GetVIPAgentScoreChangeLogs(BaseUserScoreSearchParam)"/></summary>
        public PagedResultModel<VIPAgentAccountLogModel> GetVIPAgentScoreChangeLogs(BaseUserScoreSearchParam searchParam)
        {
            //TODO 差異化實作判斷是否為下級 vip為一層
            //TODO 是否檢查的設定改到application enum內
            //檢查是否為下級, 
            if (!_userInfoRelatedService.CheckUserIdInUserPath(EnvLoginUser.LoginUser.UserId, searchParam.UserID))
            {
                return new PagedResultModel<VIPAgentAccountLogModel>()
                {
                    PageNo = searchParam.PageNum,
                    PageSize = searchParam.PageSize
                };
            }

            //前台要處理結尾時間
            searchParam.EndDate = searchParam.EndDate.ToQuerySmallEqualThanTime(DatePeriods.Day);

            return _vipUserChangeLogRep.GetVIPAgentScoreChangeLogs(searchParam);
        }
    }
}