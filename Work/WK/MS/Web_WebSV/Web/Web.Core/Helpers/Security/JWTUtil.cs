using Jose;
using System.Text;

namespace Web.Helpers.Security
{
    public static class JWTUtil
    {
        public static string EncodeByHS256(object payload, string key)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            string token = JWT.Encode(payload, keyBytes, JwsAlgorithm.HS256);

            return token;
        }

        public static T DecodeByHS256<T>(string token, string key)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            T payloadModel = JWT.Decode<T>(token, keyBytes, JwsAlgorithm.HS256);

            return payloadModel;
        }
    }
}
