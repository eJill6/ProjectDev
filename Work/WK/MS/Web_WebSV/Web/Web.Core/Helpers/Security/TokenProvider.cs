using ControllerShareLib.Helpers.Security;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Util;

namespace Web.Helpers.Security
{
    public class TokenProvider
    {
        private const string _key = "IllIlIll";

        private const int _expiredMinutes = 5;

        private static readonly DESTool _desTool = new DESTool(_key);

        /// <summary>
        /// 取得Token
        /// </summary>
        /// <param name="value">Token值</param>
        /// <returns></returns>
        public static TokenModel GetToken(string value)
        {
            try
            {
                var decrypedResult = _desTool.DESDeCode(value);
                var target = decrypedResult.Split('|');
                if (target.Length != 3)
                {
                    return null;
                }

                var result = new TokenModel
                {
                    Key = target[0],
                    UserName = target[2],
                    ExpiryTime = Convert.ToDateTime(target[1])
                };

                result.IsExpired = IsTokenExpired(result.ExpiryTime);

                return result;
            }
            catch (Exception ex)
            {
                DependencyUtil.ResolveService<ILogUtilService>().Value.Error(ex);

                return null;
            }
        }

        /// <summary>
        /// 產生Token
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="userName">userName</param>
        /// <returns></returns>
        public static string GenerateTokenString(string key, string userName)
        {
            var result = new TokenModel
            {
                Key = key,
                UserName = userName,
                ExpiryTime = DateTime.Now.AddMinutes(_expiredMinutes)
            };

            return _desTool.DESEnCode(result.ToString());
        }

        /// <summary>
        /// 檢查token是否過期
        /// </summary>
        /// <param name="expiryTime">過期時間</param>
        /// <returns></returns>
        private static bool IsTokenExpired(DateTime expiryTime)
        {
            var remainSeconds = expiryTime.Subtract(DateTime.Now).TotalSeconds;
            var remainMinutes = Math.Ceiling(remainSeconds / 60);

            //LogHelper.LogError(string.Format("秒:{0}, 分:{1}", remainSeconds, remainMinutes));

            if (remainMinutes <= 0 || remainMinutes > _expiredMinutes)
            {
                //LogHelper.LogError("Token已過期!");
                return true;
            }

            return false;
        }

        /// <summary>
        /// 建立Jwt Token (sha256)
        /// </summary>
        /// <param name="claimList">claim list</param>
        /// <param name="secretSalt">加解密鑰</param>
        /// <returns>
        /// jwt token
        /// </returns>
        public static string GenerateJWTTokenWithHS256(Dictionary<string, Object> claimList, string secretSalt)
        {
            string token = JWTUtil.EncodeByHS256(claimList, secretSalt);

            return token;
        }

        /// <summary>
        /// 解 Jwt Token (sha256)
        /// </summary>
        public static Dictionary<string, object> DecryptJWTTokenWithHS256(string token, string secretSalt)
        {
            var jwtObject = JWTUtil.DecodeByHS256<Dictionary<string, object>>(token, secretSalt);

            return jwtObject;
        }
    }

    public class TokenModel
    {
        public string Key { get; set; }

        public string UserName { get; set; }

        public DateTime ExpiryTime { get; set; }

        public bool IsExpired { get; set; }

        public override string ToString()
        {
            return string.Format("{0}|{1}|{2}", this.Key, this.ExpiryTime.ToString("yyyy-MM-dd HH:mm:ss.fff"), UserName);
        }
    }
}