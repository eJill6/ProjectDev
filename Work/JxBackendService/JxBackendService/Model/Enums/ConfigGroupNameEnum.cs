using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.Enums
{
    public enum ConfigGroupNameEnum
    {
        ///<summary></summary>
        AdminBankInfoStatus,
        ///<summary>黑名单</summary>
        BlackList,
        ///<summary></summary>
        MoneyInType,
        ///<summary></summary>
        WhiteUserListIType,
        ///<summary>会员级别</summary>
        UserLevel,
        ///<summary>上级对下级转帐开关</summary>
        ParentToSonTransfer,
        ///<summary>上级转帐给下级是否需要简讯认证</summary>
        ParentToSonSMS,
        ///<summary>上级转帐给下级手机绑定验证开关</summary>
        ParentToSonPhoneRotection,
        ///<summary>Operation類別</summary>
        OperationLogCategory,
        ///<summary>代付帳戶金額分類</summary>
        AccountDFCategory,
        ///<summary>銀行卡分群開關</summary>
        MoneyCardGroup,
        ///<summary>代付/卡對卡提現金額條件區間</summary>
        AccountLimitRange,
        ///<summary>手機綁定(含初始化)</summary>
        CheckSendSMSToPhoneRetection,
        ///<summary>手機解綁</summary>
        CheckSendSMSToPhoneUnRetection,
        ///<summary>銀行卡刪除</summary>
        CheckSendSMSToBankCardDel,
        ///<summary>忘記密碼</summary>
        CheckSendSMSToForgetPwd,
        ///<summary>下線轉帳</summary>
        CheckSendSMSToTransfer,
        ///<summary>用户银行卡状态</summary>
        UserBankInfoStat,
        ///<summary>微信充值区间</summary>
        Wechart_MoneyRange,
        ///<summary>BetType</summary>
        BetType,
        ///<summary>BudgetType</summary>
        BudgetType,
        ///<summary>CancelType</summary>
        CancelType,
        ///<summary>LotteryType</summary>
        LotteryType,
        ///<summary>RefundType</summary>
        RefundType,
        ///<summary>SportType</summary>
        SportType,
        ///<summary>PT_HS_Logs-GameType</summary>
        PT_HS_Logs_GameType,
        ///<summary>GameLogType</summary>
        GameLogType,
        ///<summary>GiftHandselAudit</summary>
        GiftHandselAudit,
        ///<summary>GoogleVerify</summary>
        GoogleVerify,
        ///<summary>MoneyBankVerify_PasswordReset</summary>
        MoneyBankVerify_PasswordReset
    }
}
