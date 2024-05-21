using System;
using Web.Helpers;
using Web.Models.Base;
using Web.Helpers.Security;
using Web.Services;
using System.Web.Http;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using SLPolyGame.Web.Model;

namespace Web.Controllers.Api
{
    [ApiExceptionFilter]
    public class BaseApiController : ApiController
    {
        protected readonly ICacheService _cacheService = null;

        protected readonly IUserService _userService = null;

        public BaseApiController(ICacheService cacheService,
            IUserService userService)
        {
            _cacheService = cacheService;
            _userService = userService;
        }

        protected int GetUserId()
        {
            return GetUserInfo(isForcedRefresh: false).UserId;
        }

        protected UserInfo GetUserInfo(bool isForcedRefresh)
        {
            string userId = AuthenticationUtil.GetTokenModel().UserId.ToString();
            string key = string.Format(CacheKeyHelper.UserInfo, userId);

            //用户信息缓存5分钟
            UserInfo userInfo = _cacheService.Get(key, DateTime.Now.AddMinutes(5),
                () => _userService.GetUserInfo());

            if (userInfo == null || isForcedRefresh)
            {
                userInfo = _userService.GetUserInfo();
                _cacheService.Set(key, userInfo, DateTime.Now.AddSeconds(5));
            }

            return userInfo;
        }

        protected TicketUserData GetUserToken() => AuthenticationUtil.GetLoginUserFromCache();

        protected IHttpActionResult JsonResponse(bool isSuccess, string errorMessage, object data)
        {
            var response = new
            {
                isSuccess,
                errorMessage,
                data
            };

            return Json(response, serializerSettings: new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
        }

        protected IHttpActionResult Ok(object data)
        {
            return JsonResponse(true, string.Empty, data);
        }

        protected new IHttpActionResult BadRequest(string errorMessage)
        {
            return JsonResponse(false, errorMessage, null);
        }
    }
}