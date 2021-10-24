using JxBackendService.Resource.Element;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.Enums
{
    public class ReturnCode : BaseStringValueModel<ReturnCode>
    {
        public bool IsSuccess { get; private set; } = false;

        private ReturnCode(string value)
        {
            Value = value;
        }

        private ReturnCode(string value, Type resourceType, string resourcePropertyName) : this(value)
        {
            ResourceType = resourceType;
            ResourcePropertyName = resourcePropertyName;
        }

        private ReturnCode(string value, Type resourceType, string resourcePropertyName, bool isSuccess) : this(value, resourceType, resourcePropertyName)
        {
            IsSuccess = isSuccess;
        }

        /// <summary>成功</summary>
        public static readonly ReturnCode Success = new ReturnCode("000000", typeof(ReturnCodeElement), nameof(ReturnCodeElement.Success), true);

        /// <summary>
        /// 特殊ReturnMessage或ExceptionMessage
        /// </summary>
        public static readonly ReturnCode CustomizedMessage = new ReturnCode("E09999");

        /// <summary>系統錯誤</summary>
        public static readonly ReturnCode SystemError = new ReturnCode("E00001", typeof(ReturnCodeElement), nameof(ReturnCodeElement.SystemError));

        /// <summary>傳入參數有誤</summary>
        public static readonly ReturnCode ParameterIsInvalid = new ReturnCode("E00002", typeof(ReturnCodeElement), nameof(ReturnCodeElement.ParameterIsInvalid));

        /// <summary>Dial位置未設置</summary>
        public static readonly ReturnCode DialApiUrlIsEmpty = new ReturnCode("E00003", typeof(ReturnCodeElement), nameof(ReturnCodeElement.DialApiUrlIsEmpty));

        /// <summary>查無用戶手機號</summary>
        public static readonly ReturnCode PhoneNumberNotFound = new ReturnCode("E00004", typeof(ReturnCodeElement), nameof(ReturnCodeElement.PhoneNumberNotFound));

        /// <summary>资料已经存在</summary>
        public static readonly ReturnCode DataIsExist = new ReturnCode("E00005", typeof(ReturnCodeElement), nameof(ReturnCodeElement.DataIsExist));

        /// <summary>验证码过期,请重新产生验证码</summary>
        public static readonly ReturnCode LoginCodeExpired = new ReturnCode("E00006", typeof(ReturnCodeElement), nameof(ReturnCodeElement.LoginCodeExpired));

        /// <summary>Google身分验证已过期,请重新绑定</summary>
        public static ReturnCode GoogleAuthenticatorExpired = new ReturnCode("E00007", typeof(ReturnCodeElement), nameof(ReturnCodeElement.GoogleAuthenticatorExpired));

        /// <summary>登入失败</summary>
        public static ReturnCode LoginFail = new ReturnCode("E00008", typeof(ReturnCodeElement), nameof(ReturnCodeElement.LoginFail));

        /// <summary>该用户未绑定银行卡无法进行身份验证</summary>
        public static ReturnCode UserUnboundBankCard = new ReturnCode("E00009", typeof(MessageElement), nameof(MessageElement.UserUnboundBankCard));

        /// <summary>该用户尚未进行初始化，请用户于前台初始化设定设置邮箱</summary>
        public static ReturnCode UserInitializeIncomplete = new ReturnCode("E00010", typeof(MessageElement), nameof(MessageElement.UserInitializeIncomplete));

        /// <summary>身份验证失败</summary>
        public static ReturnCode UserVaildFailed = new ReturnCode("E00011", typeof(MessageElement), nameof(MessageElement.UserVaildFailed));

        /// <summary>资料填写不完整</summary>
        public static readonly ReturnCode DataIsNotCompleted = new ReturnCode("E00012", typeof(ReturnCodeElement), nameof(ReturnCodeElement.DataIsNotCompleted));

        /// <summary>资料更新失败</summary>
        public static readonly ReturnCode UpdateFailed = new ReturnCode("E00013", typeof(ReturnCodeElement), nameof(ReturnCodeElement.UpdateFailed));

        /// <summary>操作失败</summary>
        public static readonly ReturnCode OperationFailed = new ReturnCode("E00014", typeof(ReturnCodeElement), nameof(ReturnCodeElement.OperationFailed));

        /// <summary>手机号码格式错误</summary>
        public static readonly ReturnCode MobileFormatFail = new ReturnCode("E00015", typeof(MessageElement), nameof(MessageElement.MobileFormatFail));

        /// <summary>邮箱格式错误</summary>
        public static readonly ReturnCode EmailRuleFail = new ReturnCode("E00016", typeof(MessageElement), nameof(MessageElement.EmailRuleFail));

        /// <summary>审核列表已存在此笔资料，请通知上级进行审核</summary>
        public static readonly ReturnCode AlreadyExistAuditInfo = new ReturnCode("E00017", typeof(ReturnCodeElement), nameof(ReturnCodeElement.AlreadyAuditInfo));

        /// <summary>该手机号码已被绑定，请重新输入</summary>
        public static readonly ReturnCode ModifyMobileUserDataIsExist = new ReturnCode("E00018", typeof(ReturnCodeElement), nameof(ReturnCodeElement.ModifyMobileUserDataIsExist));

        /// <summary>该邮箱已被绑定，请重新输入</summary>
        public static readonly ReturnCode ModifyEmailUserDataIsExist = new ReturnCode("E00019", typeof(ReturnCodeElement), nameof(ReturnCodeElement.ModifyEmailUserDataIsExist));

        /// <summary>审核列表已存在相同的新待审核，请再确认</summary>
        public static readonly ReturnCode AuditDataIsExists = new ReturnCode("E00020", typeof(ReturnCodeElement), nameof(ReturnCodeElement.AuditDataIsExists));

        /// <summary>查无资料</summary>
        public static readonly ReturnCode SearchResultIsEmpty = new ReturnCode("E00021", typeof(ReturnCodeElement), nameof(ReturnCodeElement.SearchResultIsEmpty));

        /// <summary>此下级的转账功能已被关闭，请联系客服</summary>
        public static readonly ReturnCode TransferToChildForcedClose = new ReturnCode("E00022", typeof(ReturnCodeElement), nameof(ReturnCodeElement.TransferToChildForcedClose));

        /// <summary>没有任何资料异动</summary>
        public static ReturnCode NoDataChanged = new ReturnCode("E00023", typeof(ReturnCodeElement), nameof(ReturnCodeElement.NoDataChanged));

        /// <summary>您已被移出群聊</summary>
        public static ReturnCode BeRemovedFromGroupChatRoom = new ReturnCode("E00024", typeof(ReturnCodeElement), nameof(ReturnCodeElement.BeRemovedFromGroupChatRoom));

        /// <summary>温馨提示：您没有权限在该群聊发言</summary>
        public static ReturnCode NotEnabledToSendGroupChatRoomMessage = new ReturnCode("E00025", typeof(ReturnCodeElement), nameof(ReturnCodeElement.NotEnabledToSendGroupChatRoomMessage));

        /// <summary>此群聊已解散</summary>
        public static ReturnCode CroupChatRoomBeDeleted = new ReturnCode("E00026", typeof(ReturnCodeElement), nameof(ReturnCodeElement.CroupChatRoomBeDeleted));

        /// <summary>非群聊创建者</summary>
        public static ReturnCode NotCreaterForCroupChatRoom = new ReturnCode("E00027", typeof(ReturnCodeElement), nameof(ReturnCodeElement.NotCreaterForCroupChatRoom));

        /// <summary>群聊名称重覆</summary>
        public static ReturnCode CroupChatRoomNameDuplicate = new ReturnCode("E00028", typeof(ReturnCodeElement), nameof(ReturnCodeElement.CroupChatRoomNameDuplicate));

        /// <summary>已达群聊上限数</summary>
        public static ReturnCode CroupChatRoomCountReachedTheMaxLimit = new ReturnCode("E00029", typeof(ReturnCodeElement), nameof(ReturnCodeElement.CroupChatRoomCountReachedTheMaxLimit));

        /// <summary>该群聊下级人数已达上限数，无法再选取！</summary>
        public static ReturnCode CroupChatRoomMemberCountReachedTheMaxLimit = new ReturnCode("E00030", typeof(ReturnCodeElement), nameof(ReturnCodeElement.CroupChatRoomMemberCountReachedTheMaxLimit));

        /// <summary>非群聊内成员</summary>
        public static ReturnCode MemberNotInThisCroupChatRoom = new ReturnCode("E00031", typeof(ReturnCodeElement), nameof(ReturnCodeElement.MemberNotInThisCroupChatRoom));

        /// <summary>您的帳戶已凍結</summary>
        public static ReturnCode YourAccountIsDisabled = new ReturnCode("E00032", typeof(ReturnCodeElement), nameof(ReturnCodeElement.YourAccountIsDisabled));

        /// <summary>站内信未启用</summary>
        public static ReturnCode ChatRoomServiceNotActive = new ReturnCode("E00033", typeof(ReturnCodeElement), nameof(ReturnCodeElement.ChatRoomServiceNotActive));

        /// <summary>群聊名称限制{0}個字元內</summary>
        public static ReturnCode ChatRoomNameOutOfRange = new ReturnCode("E00034", typeof(ReturnCodeElement), nameof(ReturnCodeElement.ChatRoomNameOutOfRange));

        /// <summary>讯息限制{0}個字元內</summary>
        public static ReturnCode ChatRoomMessageOutOfRange = new ReturnCode("E00035", typeof(ReturnCodeElement), nameof(ReturnCodeElement.ChatRoomNameOutOfRange));

        /// <summary>超过每分钟内发信次数 10 次！</summary>
        public static ReturnCode ChatRoomPerMinuteMessageCountOutOfRange = new ReturnCode("E00036", typeof(ReturnCodeElement), nameof(ReturnCodeElement.ChatRoomPerMinuteMessageCountOutOfRange));

        /// <summary>用户不存在</summary>
        public static ReturnCode UserNotFound = new ReturnCode("E00037", typeof(ReturnCodeElement), nameof(ReturnCodeElement.UserNotFound));

        /// <summary>请重设您的资金密码</summary>
        public static ReturnCode UserMustResetMoneyPwd = new ReturnCode("E00038", typeof(ReturnCodeElement), nameof(ReturnCodeElement.UserMustResetMoneyPwd));

        /// <summary>請先綁定谷歌身份驗證</summary>
        public static ReturnCode UserMustUserAuth = new ReturnCode("E00039", typeof(ReturnCodeElement), nameof(ReturnCodeElement.UserMustUserAuth));

        /// <summary>资金密码错误，错误次数过多，会导致账号被锁定 的當地語系化字串(配合sp代碼改為中文)</summary>
        public static ReturnCode YourMoneyPasswordIsWrong = new ReturnCode("您的资金密码不正确", typeof(ReturnCodeElement), nameof(ReturnCodeElement.YourMoneyPasswordIsWrong));

        /// <summary>用戶暱稱重複</summary>
        public static ReturnCode DuplicateUserNickname = new ReturnCode("E00041", typeof(ReturnCodeElement), nameof(ReturnCodeElement.DuplicateUserNickname));

        /// <summary>昵称不能为空</summary>
        public static readonly ReturnCode NicknameIsNotCompleted = new ReturnCode("E00042", typeof(ReturnCodeElement), nameof(ReturnCodeElement.NicknameIsNotCompleted));

        /// <summary>昵称长度过长</summary>
        public static readonly ReturnCode NicknameTooManyWords = new ReturnCode("E00043", typeof(ReturnCodeElement), nameof(ReturnCodeElement.NicknameTooManyWords));

        /// <summary>请按照正常流程进入页面</summary>
        public static readonly ReturnCode EntranceIsNotAllowed = new ReturnCode("E00044", typeof(ReturnCodeElement), nameof(ReturnCodeElement.EntranceIsNotAllowed));

        /// <summary>请重新进入页面</summary>
        public static readonly ReturnCode EntranceIsTimeOut = new ReturnCode("E00045", typeof(ReturnCodeElement), nameof(ReturnCodeElement.EntranceIsTimeOut));

        /// <summary>已是密保状态</summary>
        public static readonly ReturnCode AlreadySecurityStatus = new ReturnCode("E00046", typeof(ReturnCodeElement), nameof(ReturnCodeElement.AlreadySecurityStatus));

        /// <summary>邮箱已被使用！</summary>
        public static readonly ReturnCode EmailIsUsed = new ReturnCode("E00047", typeof(ReturnCodeElement), nameof(ReturnCodeElement.EmailIsUsed));

        /// <summary>用户已被冻结</summary>
        public static readonly ReturnCode UserIsFrozen = new ReturnCode("E00048", typeof(ReturnCodeElement), nameof(ReturnCodeElement.UserIsFrozen));

        /// <summary>密碼不可與資金密碼相同</summary>
        public static readonly ReturnCode SamePassword = new ReturnCode("E00049", typeof(ReturnCodeElement), nameof(ReturnCodeElement.SamePassword));

        /// <summary>用戶已初始化設置過</summary>
        public static readonly ReturnCode InitHasFinished = new ReturnCode("E00050", typeof(ReturnCodeElement), nameof(ReturnCodeElement.InitHasFinished));

        /// <summary>您已绑定谷歌身份验证</summary>
        public static ReturnCode GoogleAuthenticatorVerified = new ReturnCode("E00051", typeof(ReturnCodeElement), nameof(ReturnCodeElement.GoogleAuthenticatorVerified));

        /// <summary>資金密碼過期</summary>
        public static ReturnCode MoneyPasswordExpired = new ReturnCode("E00052", typeof(ReturnCodeElement), nameof(ReturnCodeElement.MoneyPasswordExpired));

        /// <summary>該用戶已解綁谷歌驗證，無法通過審核</summary>
        public static ReturnCode UserAuthenticatorUnbinded = new ReturnCode("E00053", typeof(ReturnCodeElement), nameof(ReturnCodeElement.UserAuthenticatorUnbinded));

        /// <summary>該用戶已更換新的驗證密鑰，無法通過審核</summary>
        public static ReturnCode UserAuthenticatorChanged = new ReturnCode("E00054", typeof(ReturnCodeElement), nameof(ReturnCodeElement.UserAuthenticatorChanged));

        /// <summary>老用户才能参与红包领取活动</summary>
        public static ReturnCode ColourEggOnlyForOldUser = new ReturnCode("E00055", typeof(ReturnCodeElement), nameof(ReturnCodeElement.ColourEggOnlyForOldUser));

        /// <summary>重覆领取红包</summary>
        public static ReturnCode ColourEggSameIPOpened = new ReturnCode("E00056", typeof(ReturnCodeElement), nameof(ReturnCodeElement.ColourEggSameIPOpened));

        /// <summary>请绑定银行卡以便领取红包</summary>
        public static ReturnCode ColourEggNoBindBankCard = new ReturnCode("E00057", typeof(ReturnCodeElement), nameof(ReturnCodeElement.ColourEggNoBindBankCard));

        /// <summary>成功领取</summary>
        public static ReturnCode OpenedSuccess = new ReturnCode("E00058", typeof(ReturnCodeElement), nameof(ReturnCodeElement.OpenedSuccess));

        /// <summary>现已不支援此登录方式，请更新您的APP版本</summary>
        public static ReturnCode LoginTypeNoSupported = new ReturnCode("E00059", typeof(ReturnCodeElement), nameof(ReturnCodeElement.LoginTypeNoSupported));

        /// <summary>邮件发送失败,请联系管理员或者稍后重试</summary>
        public static ReturnCode SendEmailFailed = new ReturnCode("E00060", typeof(ReturnCodeElement), nameof(ReturnCodeElement.SendEmailFailed));

        /// <summary>资金密码不正确(用於找回密碼)</summary>
        public static ReturnCode MoneyPasswordIncorrectByFindPassword = new ReturnCode("E00061", typeof(ReturnCodeElement), nameof(ReturnCodeElement.MoneyPasswordIncorrectByFindPassword));

        /// <summary>密保邮箱未完善或不正确</summary>
        public static ReturnCode RecoveryEmailIncompleteOrIncorrect = new ReturnCode("E00062", typeof(ReturnCodeElement), nameof(ReturnCodeElement.RecoveryEmailIncompleteOrIncorrect));

        /// <summary>验证码错误</summary>
        public static ReturnCode ValidateCodeIncorrect = new ReturnCode("E00063", typeof(ReturnCodeElement), nameof(ReturnCodeElement.ValidateCodeIncorrect));

        /// <summary>验证码失效，请刷新验证码</summary>
        public static ReturnCode ValidateCodeIsExpired = new ReturnCode("E00064", typeof(ReturnCodeElement), nameof(ReturnCodeElement.ValidateCodeIsExpired));

        /// <summary>尝试过于频繁，请稍后再试</summary>
        public static ReturnCode TryTooOften = new ReturnCode("E00065", typeof(ReturnCodeElement), nameof(ReturnCodeElement.TryTooOften));

        /// <summary>非VIP用户</summary>
        public static ReturnCode NotVIPUser = new ReturnCode("E00066", typeof(ReturnCodeElement), nameof(ReturnCodeElement.NotVIPUser));

        /// <summary>不存在的VIP等级 </summary>
        public static ReturnCode NotExistVIPLevel = new ReturnCode("E00067", typeof(ReturnCodeElement), nameof(ReturnCodeElement.NotExistVIPLevel));

        /// <summary> 生日礼金已领取</summary>
        public static ReturnCode BirthdayGiftMoneyReceived = new ReturnCode("E00068", typeof(ReturnCodeElement), nameof(ReturnCodeElement.BirthdayGiftMoneyReceived));

        /// <summary>礼金领取时间过期</summary>
        public static ReturnCode GiftMoneyReceiveExpired = new ReturnCode("E00069", typeof(ReturnCodeElement), nameof(ReturnCodeElement.GiftMoneyReceiveExpired));

        /// <summary>不符合活动参与资格</summary>
        public static ReturnCode NoQualifyForActivity = new ReturnCode("E00070", typeof(VIPContentElement), nameof(VIPContentElement.NoQualifiedForActivity));

        /// <summary>尚有正在审核的资料，请等待审核完成后才可再次申请活动</summary>
        public static ReturnCode HasUnprocessedAuditActivity = new ReturnCode("E00071", typeof(VIPContentElement), nameof(VIPContentElement.HasUnprocessedAuditActivity));

        /// <summary>本月可参与活动次数已满</summary>
        public static ReturnCode EnoughMonthlyActivity = new ReturnCode("E00072", typeof(VIPContentElement), nameof(VIPContentElement.EnoughMonthlyActivity));

        /// <summary>审核已完成，无需再更新</summary>
        public static ReturnCode AuditIsAlreadyCompleted = new ReturnCode("E00073", typeof(VIPContentElement), nameof(VIPContentElement.AuditIsAlreadyCompleted));

        #region 註冊頁使用的 Register Page

        /// <summary>注册失败，请联系你的上线</summary>
        public static ReturnCode RegisterFailedPleaseCallYourUpline = new ReturnCode("E00074", typeof(ReturnCodeElement), nameof(ReturnCodeElement.RegisterFailedPleaseCallYourUpline));

        /// <summary>用户名已经存在！</summary>
        public static ReturnCode UsernameExists = new ReturnCode("E00075", typeof(ReturnCodeElement), nameof(ReturnCodeElement.UsernameExists));

        /// <summary>用户名包含非法字符！</summary>
        public static ReturnCode UsernameHaveInvalidChars = new ReturnCode("E00076", typeof(ReturnCodeElement), nameof(ReturnCodeElement.UsernameHaveInvalidChars));

        /// <summary>密码包含非法字符！</summary>
        public static ReturnCode PasswordHaveInvalidChars = new ReturnCode("E00077", typeof(ReturnCodeElement), nameof(ReturnCodeElement.PasswordHaveInvalidChars));

        /// <summary>用户名字符过长！</summary>
        public static ReturnCode UsernameTooLong = new ReturnCode("E00078", typeof(ReturnCodeElement), nameof(ReturnCodeElement.UsernameTooLong));

        /// <summary>密码字符请输入6~16个字符！</summary>
        public static ReturnCode PasswordLenthInvalid = new ReturnCode("E00079", typeof(ReturnCodeElement), nameof(ReturnCodeElement.PasswordLenthInvalid));

        /// <summary>链接已失效！</summary>
        public static ReturnCode LinkExpired = new ReturnCode("E00080", typeof(ReturnCodeElement), nameof(ReturnCodeElement.LinkExpired));

        /// <summary>注册失败，稍后重试！</summary>
        public static ReturnCode RegisterFailedPleaseTryLater = new ReturnCode("E00081", typeof(ReturnCodeElement), nameof(ReturnCodeElement.RegisterFailedPleaseTryLater));

        #endregion

        /// <summary>未设置手机号码</summary>
        public static ReturnCode NoPhoneNumber = new ReturnCode("E00082", typeof(VIPContentElement), nameof(VIPContentElement.NoPhoneNumber));

        /// <summary>未设置真实姓名</summary>
        public static ReturnCode NoRealName = new ReturnCode("E00083", typeof(VIPContentElement), nameof(VIPContentElement.NoRealName));

        #region Information Code
        /// <summary>资料已进审核列表，请通知上级进行审核</summary>
        public static ReturnCode AuditInfoSubmit = new ReturnCode("I00001", typeof(ReturnCodeElement), nameof(ReturnCodeElement.AuditInfoSubmit), true);

        /// <summary>转账请求已提交，请耐心等候</summary>
        public static ReturnCode TransferSubmit = new ReturnCode("I00002", typeof(ReturnCodeElement), nameof(ReturnCodeElement.TransferSubmit), true);

        /// <summary>提款请求已提交，请耐心等候</summary>
        public static ReturnCode TransferOutSubmit = new ReturnCode("I00003", typeof(ReturnCodeElement), nameof(ReturnCodeElement.TransferOutSubmit), true);

        /// <summary>已赠送彩金</summary>
        public static ReturnCode GivePrizeAlready = new ReturnCode("I00004", typeof(ReturnCodeElement), nameof(ReturnCodeElement.GivePrizeAlready), true);
        #endregion
    }

    public class SuccessMessage
    {
        public SuccessMessage()
        {
            Text = SuccessCode.Name;
        }

        public SuccessMessage(string text)
        {
            Text = text;
        }

        public ReturnCode SuccessCode => ReturnCode.Success;

        public string Text { get; set; }
    }

}