using MS.Core.MMModel.Models.Auth.Enums;
using System.ComponentModel.DataAnnotations;

namespace MS.Core.MMModel.Models.Auth
{
    /// <summary>
    /// 登入參數
    /// </summary>
    public class SignInData
    {
        /// <summary>
        /// 帳號
        /// </summary>
        [Required]
        public int UserId { get; set; }

        /// <summary>
        /// 帳號
        /// </summary>
        [Required]
        public string Nickname { get; set; } = string.Empty;

        /// <summary>
        /// 紀錄類型
        /// </summary>
        public VisitType Type { get; set; } = VisitType.Login;
    }
}