using JxBackendService.Common.Util;
using JxBackendService.Interface.Repository.User;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.User;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.User;
using JxBackendService.Model.ViewModel.VIP;
using JxBackendService.Repository.User;
using JxBackendService.Service.Base;

namespace JxBackendService.Service.User
{
    public class UserAccountService : BaseService, IUserAccountService
    {
        private readonly IUserAccountRep _userAccountRep;
        private readonly IUserInfoRelatedService _userInfoRelatedService;

        public UserAccountService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _userAccountRep = ResolveJxBackendService<IUserAccountRep>();
            _userInfoRelatedService = ResolveJxBackendService<IUserInfoRelatedService>();
        }

        public PagedResultModel<UserScoreLog> GetBackendUserAccountScoreLog(BackendUserScoreSearchParam searchParam)
        {
            var userScoreSearchParam = searchParam.CastByJson<UserScoreSearchParam>();

            ConfigGroupSettings settings = searchParam.QueryScoreKey.Deserialize<ConfigGroupSettings>();

            if (settings != null)
            {
                userScoreSearchParam.GroupName = settings.GroupName;
                userScoreSearchParam.ItemKey = settings.ItemKey;
            }

            return _userAccountRep.GetAccountScoreLogs(userScoreSearchParam);
        }

        /// <summary>
        /// 前台查詢帳變
        /// </summary>
        public PagedResultModel<UserScoreLog> GetUserAccountScoreLog(BaseUserScoreSearchParam searchParam)
        {
            // 檢查是否為下級
            if (!_userInfoRelatedService.CheckUserIdInUserPath(EnvLoginUser.LoginUser.UserId, searchParam.UserID))
            {
                return new PagedResultModel<UserScoreLog>()
                {
                    PageNo = searchParam.PageNum,
                    PageSize = searchParam.PageSize
                };
            }

            var userScoreSearchParam = searchParam.CastByJson<UserScoreSearchParam>();
            userScoreSearchParam.GroupName = string.Empty;
            userScoreSearchParam.ItemKey = 0;
            //前台要處理結尾時間
            userScoreSearchParam.EndDate = userScoreSearchParam.EndDate.ToQuerySmallEqualThanTime(DatePeriods.Day);

            return _userAccountRep.GetAccountScoreLogs(userScoreSearchParam);
        }
    }
}
