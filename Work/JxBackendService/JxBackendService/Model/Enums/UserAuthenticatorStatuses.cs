using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.Enums
{
    public enum UserAuthenticatorStatuses
    {
        /// <summary>
        /// 已綁定
        /// </summary>
        Verified,
        /// <summary>
        /// 未綁定
        /// </summary>
        NoVerified,
        /// <summary>
        /// 綁定已過期
        /// </summary>
        Expired,
    }

    /// <summary>
    /// 使用二次驗證的功能項目
    /// </summary>
    public enum UserAuthenticatorSettingTypes
    {
        /// <summary>下線轉帳強制谷歌驗證</summary>
        TransferToChild = 1,
        /// <summary>修改銀行卡強制谷歌驗證</summary>
        ModifyBankCard = 2,
        /// <summary>綁定USDT賬戶強制谷歌驗證</summary>
        ModifyUSDTAccount = 3,
        /// <summary>提現前檢查</summary>
        Withdraw = 4,
        /// <summary>驗證是否需要重設資金密碼</summary>
        MoneyPassword = 5,
    }
}
