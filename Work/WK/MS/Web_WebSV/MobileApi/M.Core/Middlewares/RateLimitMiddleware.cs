using ControllerShareLib.Helpers.Security;
using JxBackendService.Common.Util;
using JxBackendService.Common.Util.Cache;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Net;
using JxBackendService.Middlewares;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Resource.Element;
using System.Net;
using System.Threading.RateLimiting;

namespace M.Core.Middlewares
{
    public class RateLimitMiddleware : BaseMiddleware
    {
        private static readonly HashSet<string> s_RateLimitPaths = new HashSet<string>()
        {
            "/LotterySpa/GetNextIssueNo"
        };

        private static readonly TokenBucketRateLimiterOptions s_userRequestRateLimiterOption = new()
        {
            QueueLimit = 1,
            ReplenishmentPeriod = TimeSpan.FromSeconds(1),
            TokensPerPeriod = 1,
            TokenLimit = 6,
            AutoReplenishment = false
        };

        private readonly Lazy<IJxCacheService> _jxCacheService;

        public RateLimitMiddleware(RequestDelegate next) : base(next)
        {
            _jxCacheService = DependencyUtil.ResolveService<IJxCacheService>();
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string identity = context.Request.Headers[AuthenticationUtil.MWTHeaderName].ToString();

            if (identity.IsNullOrEmpty())
            {
                var ipUtilService = DependencyUtil.ResolveService<IIpUtilService>().Value;
                string ipAddress = ipUtilService.GetIPAddress();

                if (ipAddress.IsNullOrEmpty())
                {
                    await Next(context);

                    return;
                }

                identity = ipAddress;
            }

            string path = context.Request.Path;

            if (!s_RateLimitPaths.Contains(path))
            {
                await Next(context);

                return;
            }

            TokenBucketRateLimiter tokenBucketRateLimiter = _jxCacheService.Value.GetCache(
                new SearchCacheParam()
                {
                    Key = CacheKey.RequestRateLimiter(identity, path),
                    CacheSeconds = 30 * 60,
                    IsCloneInstance = false,
                    IsSlidingExpiration = true,
                },
                () =>
                {
                    return new TokenBucketRateLimiter(s_userRequestRateLimiterOption);
                });

            tokenBucketRateLimiter.TryReplenish();
            RateLimitLease limit = tokenBucketRateLimiter.AttemptAcquire();

            if (!limit.IsAcquired)
            {
                context.Response.ContentType = $"{HttpWebRequestContentType.Json}; charset=utf-8";
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                string responseJson = new AppResponseModel() { Success = false, Message = CommonElement.BusyWaiting }.ToJsonString(isCamelCaseNaming: true);
                await context.Response.WriteAsync(responseJson);

                return;
            }

            await Next(context);
        }
    }
}