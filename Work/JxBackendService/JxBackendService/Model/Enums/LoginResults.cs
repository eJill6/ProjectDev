using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.Enums
{
    public enum LoginStatuses
    {
        /// <summary>
        /// 成功
        /// </summary>
        Success = 1,
        /// <summary>
        /// 帳密錯誤
        /// </summary>
        UserNameOrPasswordIsNotValid = 2,
        /// <summary>
        /// 動態密碼錯誤
        /// </summary>
        AuthenticatorPinIsNotValid = 3,
        /// <summary>
        /// 驗證碼過期
        /// </summary>
        LoginCodeExpired = 4,
        /// <summary>
        /// 未綁定驗證身分
        /// </summary>
        NoAuthenticator = 5,
        /// <summary>
        /// 驗證身分過期
        /// </summary>
        AuthenticatorExpired = 6,
    }
}
