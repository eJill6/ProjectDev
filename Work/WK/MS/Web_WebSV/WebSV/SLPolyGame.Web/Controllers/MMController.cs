using Microsoft.Extensions.Logging;
using RestSharp.Authenticators;
using SLPolyGame.Web.Helpers;
using SLPolyGame.Web.Models.MMModels;
using SLPolyGame.Web.MSSeal.Models;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SLPolyGame.Web.Controllers
{
    public class MMController : MMApiControllerBase
    {
        public MMController(ILogger<MMController> logger) : base(logger)
        {
        }

        [HttpPost]
        public async Task<ResultModel<UserInfoViewModel>> UserInfo(SignInData signIn)
        {
            string token = await SignIn(signIn);

            string url = GetUrl("My", "UserInfo");

            var res = await new RestRequestHelper(url, new JwtAuthenticator(token)).Get()
                .ResponseAsync<ApiResponse<UserInfoViewModel>>();

            return GetResult(res);
        }
    }
}