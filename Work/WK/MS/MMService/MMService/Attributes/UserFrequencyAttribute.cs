using Microsoft.AspNetCore.Mvc.Filters;
using MS.Core.Infrastructure.Redis;
using MS.Core.MM.Infrastructures.Exceptions;
using MS.Core.MM.Models.Auth.Enums;
using MS.Core.Models;

namespace MMService.Attributes
{
    /// <summary>
    /// 使用者呼叫頻率限制
    /// </summary>
    public class UserFrequencyAttribute : ActionFilterAttribute
    {
        private int Seconds { get; }
        private int Times { get; }
        /// <summary>
        /// 使用者呼叫頻率限制
        /// </summary>
        /// <param name="seconds">頻率時間(秒)</param>
        /// <param name="times">頻率次數</param>
        public UserFrequencyAttribute(int seconds, int times)
        {
            Seconds = seconds;
            Times = times;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterContext"></param>
        /// <exception cref="MMException"></exception>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var httpContext = filterContext.HttpContext;
            string? userid = httpContext?.User?.Claims?
                .FirstOrDefault(c => c.Type.Equals(ClaimNamesDefine.UserId.ToString(), StringComparison.OrdinalIgnoreCase))?.Value;

            if (int.TryParse(userid, out int userId) == false)
            {
                throw new MMException(ReturnCode.ParameterIsInvalid);
            }

            var redisService = httpContext!.RequestServices.GetService<IRedisService>()!;

            var method = string.Join("-", filterContext.ActionDescriptor.RouteValues.Values);

            if (redisService.Incr(RedisCacheKey.TooFrequent(userId, method), Seconds) > Times)
            {
                throw new MMException(ReturnCode.TooFrequent);
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
