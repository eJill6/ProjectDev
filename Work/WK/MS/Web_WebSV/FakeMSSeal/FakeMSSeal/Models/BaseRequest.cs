using System.Security.Cryptography;
using System.Text;

namespace FakeMSSeal.Models
{
    public abstract class BaseRequest
    {
        /// <summary>
        /// 時間戳
        /// </summary>
        public long? Ts { get; set; } = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();

        /// <summary>
        /// 秘鑰
        /// </summary>
        public string? Salt { get; set; }

        public virtual bool IsSkipSignSalt() => true;

        /// <summary>
        /// 取得簽章
        /// </summary>
        /// <returns>簽章</returns>
        public string GetSign()
        {
            var elements = GetType().GetProperties().OrderBy(x => x.Name).ToArray();
            var list = new List<string>();

            foreach (var element in elements)
            {
                if (IsSkipSignSalt() && string.Equals(element.Name, nameof(Salt)))
                {
                    continue;
                }

                if (element.PropertyType.IsArray || element.PropertyType.Name == "JsonArray")
                {
                    continue;
                }

                list.Add($"{ToCamelCase(element.Name)}={element.GetValue(this)}");
            }

            list.Add($"{ToCamelCase(nameof(Salt))}={Salt}");

            return ToHexString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(string.Join("&", list)))).ToLower();
        }

        private string ToHexString(byte[] vs)
        {
            return string.Join(string.Empty, vs.Select(x => x.ToString("X2")));
        }

        /// <summary>
        /// 轉成camel
        /// </summary>
        /// <param name="text">元字串</param>
        /// <returns>camel字串</returns>
        private string ToCamelCase(string text)
        {
            if (!string.IsNullOrEmpty(text) && text.Length > 1)
            {
                return char.ToLowerInvariant(text[0]) + text.Substring(1);
            }
            return text.ToLowerInvariant();
        }
    }
}