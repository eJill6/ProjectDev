using Microsoft.Extensions.Logging;
using SLPolyGame.Web.Controllers.Base;
using SLPolyGame.Web.Helpers;
using SLPolyGame.Web.Models.MMModels;
using SLPolyGame.Web.MSSeal.Models;
using System;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;

namespace SLPolyGame.Web.Controllers
{
    public class MMApiControllerBase : BaseApiController
    {
        protected string BaseUrl { get; }
        protected ILogger<MMController> Logger { get; }
        public MMApiControllerBase(ILogger<MMController> logger)
        {
            Logger = logger;
            BaseUrl = ConfigurationManager.AppSettings["MSServiceUrl"];
        }
        protected async Task<string> SignIn(SignInData signIn)
        {
            string url = GetUrl("Auth", "SignIn");

            var res = await new RestRequestHelper(url).Post(e => e.AddParameter(signIn)).ResponseAsync<ApiResponse<SignInResponse>>();

            ResultModel<SignInResponse> signInRes = GetResult(res);

            if(signInRes.Success == false)
            {
                throw new Exception(signInRes.Error);
            }

            return signInRes.Data.AccessToken;
        }
        protected ResultModel<T> GetResult<T>(RestResponseModel<ApiResponse<T>> res) where T : class
        {
            Guid guid = Guid.NewGuid();

            if (res.IsSuccess == false)
            {
                if(res.Exception == null)
                {
                    Logger.LogError($"{res.Response.ErrorException}({guid})");
                    return new ResultModel<T>
                    {
                        Error = $"系統錯誤({guid})",
                        Success = false,
                    };
                }
                Logger.LogError(res.Exception, $"{res.Exception.Message}({guid})");
                return new ResultModel<T>
                {
                    Error = $"系統錯誤({guid})",
                    Success = false,
                };
            }

            if (res.Result.IsSuccess == false)
            {
                Logger.LogError($"{res.Result.Message}({guid})");
                return new ResultModel<T>
                {
                    Error = $"Api回傳失敗({guid})",
                    Success = false,
                };
            }

            return new ResultModel<T>
            {
                Data = res.Result.Data,
                Success = true,
            };
        }

        protected string GetUrl(string controllerName, string actionPath)
        {
            return UrlCombine(BaseUrl, controllerName, actionPath);
        }

        protected string UrlCombine(params string[] path)
        {
            return Path.Combine(path).Replace("\\", "/");
        }

    }
}