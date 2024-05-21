namespace MMService.Models.Auth
{
    /// <summary>
    /// Jwt的設定檔
    /// </summary>
    public class JwtSettings
    {
        /// <summary>
        /// 過期秒數
        /// </summary>
        public int ExpirationSec { get; set; }

        /// <summary>
        /// 發行者
        /// </summary>
        public string Issuer { get; set; } = string.Empty;

        /// <summary>
        /// 密鑰
        /// </summary>
        public string SignKey { get; set; } = string.Empty;
    }
}