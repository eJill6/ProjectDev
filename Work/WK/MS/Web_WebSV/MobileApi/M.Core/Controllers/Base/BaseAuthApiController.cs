using ControllerShareLib.Helpers;
using ControllerShareLib.Helpers.Cache;
using ControllerShareLib.Helpers.Security;
using ControllerShareLib.Interfaces.Service;
using ControllerShareLib.Models.Base;
using JxBackendService.Common.Util;
using JxBackendService.Interface.Service.Web;
using JxBackendService.Model.Exceptions;
using JxBackendService.Model.ViewModel;
using M.Core.Filters;
using SLPolyGame.Web.Model;

namespace M.Core.Controllers.Base
{
    [MobileApiAuthorize]
    public class BaseAuthApiController : BaseApiController
    {
        private readonly Lazy<IHttpContextUserService> _httpContextUserService;
        private readonly Lazy<ICacheService> _cacheService;
        private readonly Lazy<IUserService> _userService;
        private readonly Lazy<ICache> _localInstance;

        protected ICacheService CacheService => _cacheService.Value;

        protected IUserService UserService => _userService.Value;

        public BaseAuthApiController()
        {
            _cacheService = ResolveService<ICacheService>();
            _userService = ResolveService<IUserService>();
            _httpContextUserService = ResolveService<IHttpContextUserService>();
            _localInstance = ResolveService<ICache>();
        }

        protected UserInfo GetUserInfo()
        {
            int userId = _httpContextUserService.Value.GetUserId();
            string key = string.Format(CacheKeyHelper.UserInfo, userId);

            UserInfo userInfo = _localInstance.Value.GetOrAddAsync(nameof(GetUserInfo),
                key,
                async () => UserService.GetUserInfo(),
                DateTime.Now.AddSeconds(1)).ConfigureAwait(false).GetAwaiter().GetResult();

            return userInfo;
        }

        protected async Task<UserInfo> GetUserInfoWithoutAvailable(int userId)
        {
            string key = string.Format(CacheKeyHelper.UserInfoWithoutAvailable, userId);
            UserInfo userInfo = null;
            userInfo = await _localInstance.Value.GetOrAddAsync(nameof(GetUserInfoWithoutAvailable), key, async () => await UserService.GetUserInfoWithoutAvailable(userId), DateTime.Now.AddHours(1));

            if (userInfo == null)
            {
                throw new HttpStatusException(System.Net.HttpStatusCode.Unauthorized);
            }

            return userInfo;
        }

        protected TicketUserData GetUserToken() => AuthenticationUtil.GetLoginUserFromCache();

        protected int GetUserId() => AuthenticationUtil.GetUserId();

        private EnvironmentUser _envLoginUser;

        protected override EnvironmentUser EnvLoginUser
        {
            get
            {
                _envLoginUser = AssignValueOnceUtil.GetAssignValueOnce(_envLoginUser, () =>
                {
                    BasicUserInfo basicUserInfo = _httpContextUserService.Value.GetBasicUserInfo();

                    return new EnvironmentUser()
                    {
                        Application = Application,
                        LoginUser = basicUserInfo
                    };
                });

                return _envLoginUser;
            }
        }
    }
}