using Microsoft.AspNetCore.Mvc;
using PolyDataBase.Extensions;
using RestSharp.Authenticators;
using SLPolyGame.Web.Helpers;
using SLPolyGame.Web.Models.MMModels;
using SLPolyGame.Web.MSSeal.Models;
using System.Runtime.Caching;

namespace SLPolyGame.Web.Core.Controllers
{
    public class MMController : MMApiControllerBase
    {
        public MMController()
        {
        }

        protected static MemoryCache MemoryCache { get; } = MemoryCache.Default;

        [HttpPost]
        public async Task<ResultModel<UserInfoViewModel>> UserInfo(SignInData signIn)
        {
            var cacheKey = signIn.UserId.ToString();

            return await MemoryCache.GetOrSetAsync(cacheKey, () => GetUserInfo(signIn), 30000);
        }

        private async Task<ResultModel<UserInfoViewModel>> GetUserInfo(SignInData signIn)
        {
            string token = await SignIn(signIn);

            string url = GetUrl("My", "UserInfo");

            var res = await new RestRequestHelper(url, new JwtAuthenticator(token)).Get()
                .ResponseAsync<ApiResponse<UserInfoViewModel>>();

            return GetResult(res);
        }
    }
}