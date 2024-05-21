using System.Runtime.CompilerServices;

namespace MS.Core.Infrastructure.Redis
{
    public class RedisCacheKey
    {
        public static readonly string RestSetUserUnLock = "RestSetUserUnLock";

        public static string TooFrequent(int userId, [CallerMemberName] string method = "")
        {
            return $"TooFrequent{method}{userId}";
        }
    }
}