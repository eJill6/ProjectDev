using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Web;
using JxBackendService.Model.ViewModel;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Security.Claims;

namespace JxBackendServiceN6.Service.Web
{
    public class HttpContextUserService : IHttpContextUserService
    {
        protected IHttpContextAccessor HttpContextAccessor { get; private set; }

        public HttpContextUserService()
        {
            HttpContextAccessor = DependencyUtil.ResolveService<IHttpContextAccessor>();
        }

        public string GetUserKey()
        {
            return GetBasicUserInfo().UserKey;
        }

        public int GetUserId()
        {
            return GetBasicUserInfo().UserId;
        }

        public BasicUserInfo GetBasicUserInfo()
        {
            string userKey = null;
            int userId = 0;

            if (HttpContextAccessor.HttpContext != null && HttpContextAccessor.HttpContext.User != null)
            {
                ClaimsIdentity claimsIdentity = HttpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
                userKey = claimsIdentity.Name;

                if (claimsIdentity != null && claimsIdentity.Claims.Any())
                {
                    Claim claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

                    if (claim != null)
                    {
                        userId = claim.Value.ToInt32();
                    }
                }
            }

            return new BasicUserInfo()
            {
                UserId = userId,
                UserKey = userKey
            };
        }
    }
}