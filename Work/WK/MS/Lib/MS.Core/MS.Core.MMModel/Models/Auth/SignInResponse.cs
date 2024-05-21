namespace MS.Core.MMModel.Models.Auth
{
    /// <summary>
    /// 登入回應
    /// </summary>
    public class SignInResponse
    {
        /// <summary>
        /// Access token
        /// </summary>
        public string AccessToken { get; set; } = string.Empty;
    }
}