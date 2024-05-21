namespace SLPolyGame.Web.Models.MMModels
{
    /// <summary>
    /// 登入參數
    /// </summary>
    public class SignInData
    {
        /// <summary>
        /// 帳號
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 帳號
        /// </summary>
        public string Nickname { get; set; } = string.Empty;
    }
}