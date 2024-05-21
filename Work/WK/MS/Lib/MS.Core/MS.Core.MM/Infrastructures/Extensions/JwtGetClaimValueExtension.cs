using MS.Core.MM.Infrastructures.Exceptions;
using MS.Core.MM.Models.Auth.Enums;
using MS.Core.Models;
using System.Security.Claims;

namespace MS.Core.MM.Infrastructures.Extensions
{
    public static class JwtGetClaimValueExtension
    {
        /// <summary>
        /// 從 jwt 中取得 UserId
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public static int? UserId(this ClaimsPrincipal principal)
        {
            string? userid = principal?.Claims?
                .FirstOrDefault(c => c.Type.Equals(ClaimNamesDefine.UserId.ToString(), StringComparison.OrdinalIgnoreCase))?
                .Value;

            if(int.TryParse(userid, out var id))
            {
                return id;
            }

            return null;
        }

        /// <summary>
        /// 從 jwt 中取得 Nickname
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public static string? Nickname(this ClaimsPrincipal principal)
        {
            return principal?.Claims?
                .FirstOrDefault(c => c.Type.Equals(ClaimNamesDefine.Nickname.ToString(), StringComparison.OrdinalIgnoreCase))?
                .Value;
        }

        public static string GetNickname(this ClaimsPrincipal principal)
        {
            return Nickname(principal) ?? string.Empty;
        }

        public static int GetUserId(this ClaimsPrincipal principal)
        {
            int? userId = UserId(principal);
            if (userId.HasValue == false)
            {
                throw new MMException(ReturnCode.ParameterIsInvalid);
            }
            return userId.Value;
        }
    }
}