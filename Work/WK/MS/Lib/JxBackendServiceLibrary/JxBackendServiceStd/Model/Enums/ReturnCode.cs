using JxBackendService.Resource.Element;
using System;

namespace JxBackendService.Model.Enums
{
    public class ReturnCode : BaseStringValueModel<ReturnCode>
    {
        public bool IsSuccess { get; private set; } = false;

        private ReturnCode(string value)
        {
            Value = value;
        }

        private ReturnCode(string value, string resourcePropertyName) : this(value, typeof(ReturnCodeElement), resourcePropertyName)
        {
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
        public static readonly ReturnCode SystemError = new ReturnCode("E00001", nameof(ReturnCodeElement.SystemError));

        /// <summary>傳入參數有誤</summary>
        public static readonly ReturnCode ParameterIsInvalid = new ReturnCode("E00002", nameof(ReturnCodeElement.ParameterIsInvalid));

        /// <summary>Dial位置未設置</summary>
        public static readonly ReturnCode DialApiUrlIsEmpty = new ReturnCode("E00003", nameof(ReturnCodeElement.DialApiUrlIsEmpty));

        /// <summary>查無用戶手機號</summary>
        public static readonly ReturnCode PhoneNumberNotFound = new ReturnCode("E00004", nameof(ReturnCodeElement.PhoneNumberNotFound));

        /// <summary>资料已经存在</summary>
        public static readonly ReturnCode DataIsExist = new ReturnCode("E00005", nameof(ReturnCodeElement.DataIsExist));

        /// <summary>验证码过期,请重新产生验证码</summary>
        public static readonly ReturnCode LoginCodeExpired = new ReturnCode("E00006", nameof(ReturnCodeElement.LoginCodeExpired));

        /// <summary>身分验证已过期,请重新绑定</summary>
        public static ReturnCode AuthenticatorExpired = new ReturnCode("E00007", nameof(ReturnCodeElement.AuthenticatorExpired));

        /// <summary>登入失败</summary>
        public static ReturnCode LoginFail = new ReturnCode("E00008", nameof(ReturnCodeElement.LoginFail));

        /// <summary>身份验证失败</summary>
        public static ReturnCode UserVaildFailed = new ReturnCode("E00011", typeof(MessageElement), nameof(MessageElement.UserVaildFailed));

        /// <summary>资料填写不完整</summary>
        public static readonly ReturnCode DataIsNotCompleted = new ReturnCode("E00012", nameof(ReturnCodeElement.DataIsNotCompleted));

        /// <summary>资料更新失败</summary>
        public static readonly ReturnCode UpdateFailed = new ReturnCode("E00013", nameof(ReturnCodeElement.UpdateFailed));

        /// <summary>操作失败</summary>
        public static readonly ReturnCode OperationFailed = new ReturnCode("E00014", nameof(ReturnCodeElement.OperationFailed));

        /// <summary>手机号码格式错误</summary>
        public static readonly ReturnCode MobileFormatFail = new ReturnCode("E00015", typeof(MessageElement), nameof(MessageElement.MobileFormatFail));

        /// <summary>邮箱格式错误</summary>
        public static readonly ReturnCode EmailRuleFail = new ReturnCode("E00016", typeof(MessageElement), nameof(MessageElement.EmailRuleFail));

        /// <summary>审核列表已存在此笔资料，请通知上级进行审核</summary>
        public static readonly ReturnCode AlreadyExistAuditInfo = new ReturnCode("E00017", nameof(ReturnCodeElement.AlreadyAuditInfo));

        /// <summary>手机号码已被绑定</summary>
        public static readonly ReturnCode ModifyMobileUserDataIsExist = new ReturnCode("E00018", nameof(ReturnCodeElement.ModifyMobileUserDataIsExist));

        /// <summary>邮箱已被绑定</summary>
        public static readonly ReturnCode ModifyEmailUserDataIsExist = new ReturnCode("E00019", nameof(ReturnCodeElement.ModifyEmailUserDataIsExist));

        /// <summary>审核列表已存在相同的新待审核，请再确认</summary>
        public static readonly ReturnCode AuditDataIsExists = new ReturnCode("E00020", nameof(ReturnCodeElement.AuditDataIsExists));

        /// <summary>查无资料</summary>
        public static readonly ReturnCode SearchResultIsEmpty = new ReturnCode("E00021", nameof(ReturnCodeElement.SearchResultIsEmpty));

        /// <summary>此下级的转账功能已被关闭，请联系客服</summary>
        public static readonly ReturnCode TransferToChildForcedClose = new ReturnCode("E00022", nameof(ReturnCodeElement.TransferToChildForcedClose));

        /// <summary>没有任何资料异动</summary>
        public static ReturnCode NoDataChanged = new ReturnCode("E00023", nameof(ReturnCodeElement.NoDataChanged));

        /// <summary>温馨提示：您没有权限在该群聊发言</summary>
        public static ReturnCode NotEnabledToSendGroupChatRoomMessage = new ReturnCode("E00025", nameof(ReturnCodeElement.NotEnabledToSendGroupChatRoomMessage));

        /// <summary>您的帳戶已凍結</summary>
        public static ReturnCode YourAccountIsDisabled = new ReturnCode("E00032", nameof(ReturnCodeElement.YourAccountIsDisabled));

        /// <summary>用户不存在</summary>
        public static ReturnCode UserNotFound = new ReturnCode("E00037", nameof(ReturnCodeElement.UserNotFound));

        /// <summary>请设置/重设您的资金密码</summary>
        public static ReturnCode UserMustResetMoneyPwd = new ReturnCode("E00038", nameof(ReturnCodeElement.UserMustResetMoneyPwd));

        /// <summary>請先綁定谷歌身份驗證</summary>
        public static ReturnCode UserMustUserAuth = new ReturnCode("E00039", nameof(ReturnCodeElement.UserMustUserAuth));

        /// <summary>资金密码错误，错误次数过多，会导致账号被锁定 的當地語系化字串(配合sp代碼改為中文)</summary>
        public static ReturnCode YourMoneyPasswordIsWrong = new ReturnCode("您的资金密码不正确", nameof(ReturnCodeElement.YourMoneyPasswordIsWrong));

        /// <summary>用戶暱稱重複</summary>
        public static ReturnCode DuplicateUserNickname = new ReturnCode("E00041", nameof(ReturnCodeElement.DuplicateUserNickname));

        /// <summary>昵称不能为空</summary>
        public static readonly ReturnCode NicknameIsNotCompleted = new ReturnCode("E00042", nameof(ReturnCodeElement.NicknameIsNotCompleted));

        /// <summary>昵称长度过长</summary>
        public static readonly ReturnCode NicknameTooManyWords = new ReturnCode("E00043", nameof(ReturnCodeElement.NicknameTooManyWords));

        /// <summary>请按照正常流程进入页面</summary>
        public static readonly ReturnCode EntranceIsNotAllowed = new ReturnCode("E00044", nameof(ReturnCodeElement.EntranceIsNotAllowed));

        /// <summary>请重新进入页面</summary>
        public static readonly ReturnCode EntranceIsTimeOut = new ReturnCode("E00045", nameof(ReturnCodeElement.EntranceIsTimeOut));

        /// <summary>已是密保状态</summary>
        public static readonly ReturnCode AlreadySecurityStatus = new ReturnCode("E00046", nameof(ReturnCodeElement.AlreadySecurityStatus));

        /// <summary>邮箱已被使用！</summary>
        public static readonly ReturnCode EmailIsUsed = new ReturnCode("E00047", nameof(ReturnCodeElement.EmailIsUsed));

        /// <summary>用户已被冻结</summary>
        public static readonly ReturnCode UserIsFrozen = new ReturnCode("E00048", nameof(ReturnCodeElement.UserIsFrozen));

        /// <summary>密碼不可與資金密碼相同</summary>
        public static readonly ReturnCode SamePassword = new ReturnCode("E00049", nameof(ReturnCodeElement.SamePassword));

        /// <summary>用戶已初始化設置過</summary>
        public static readonly ReturnCode InitHasFinished = new ReturnCode("E00050", nameof(ReturnCodeElement.InitHasFinished));

        /// <summary>您已绑定身份验证</summary>
        public static ReturnCode AuthenticatorVerified = new ReturnCode("E00051", nameof(ReturnCodeElement.AuthenticatorVerified));

        /// <summary>資金密碼過期</summary>
        public static ReturnCode MoneyPasswordExpired = new ReturnCode("E00052", nameof(ReturnCodeElement.MoneyPasswordExpired));

        /// <summary>該用戶已解綁谷歌驗證，無法通過審核</summary>
        public static ReturnCode UserAuthenticatorUnbinded = new ReturnCode("E00053", nameof(ReturnCodeElement.UserAuthenticatorUnbinded));

        /// <summary>該用戶已更換新的驗證密鑰，無法通過審核</summary>
        public static ReturnCode UserAuthenticatorChanged = new ReturnCode("E00054", nameof(ReturnCodeElement.UserAuthenticatorChanged));

        /// <summary>成功领取</summary>
        public static ReturnCode OpenedSuccess = new ReturnCode("E00058", nameof(ReturnCodeElement.OpenedSuccess));

        /// <summary>现已不支援此登录方式，请更新您的APP版本</summary>
        public static ReturnCode LoginTypeNoSupported = new ReturnCode("E00059", nameof(ReturnCodeElement.LoginTypeNoSupported));

        /// <summary>邮件发送失败,请联系管理员或者稍后重试</summary>
        public static ReturnCode SendEmailFailed = new ReturnCode("E00060", nameof(ReturnCodeElement.SendEmailFailed));

        /// <summary>资金密码不正确(用於找回密碼)</summary>
        public static ReturnCode MoneyPasswordIncorrectByFindPassword = new ReturnCode("E00061", nameof(ReturnCodeElement.MoneyPasswordIncorrectByFindPassword));

        /// <summary>密保邮箱未完善或不正确</summary>
        public static ReturnCode RecoveryEmailIncompleteOrIncorrect = new ReturnCode("E00062", nameof(ReturnCodeElement.RecoveryEmailIncompleteOrIncorrect));

        /// <summary>验证码错误</summary>
        public static ReturnCode ValidateCodeIncorrect = new ReturnCode("E00063", nameof(ReturnCodeElement.ValidateCodeIncorrect));

        /// <summary>验证码失效，请刷新验证码</summary>
        public static ReturnCode ValidateCodeIsExpired = new ReturnCode("E00064", nameof(ReturnCodeElement.ValidateCodeIsExpired));

        /// <summary>尝试过于频繁，请稍后再试</summary>
        public static ReturnCode TryTooOften = new ReturnCode("E00065", nameof(ReturnCodeElement.TryTooOften));

        /// <summary>发送次数频繁，请稍候再试</summary>
        public static ReturnCode SendRateTooFrequently = new ReturnCode("E00066", nameof(ReturnCodeElement.SendRateTooFrequently));

        /// <summary>您尚未绑定身份验证</summary>
        public static ReturnCode AuthenticatorUnverified = new ReturnCode("E00067", nameof(ReturnCodeElement.AuthenticatorUnverified));

        /// <summary>非VIP用户</summary>
        public static ReturnCode NotVIPUser = new ReturnCode("E00068", typeof(ReturnCodeElement), nameof(ReturnCodeElement.NotVIPUser));

        /// <summary>不存在的VIP等级 </summary>
        public static ReturnCode NotExistVIPLevel = new ReturnCode("E00069", typeof(ReturnCodeElement), nameof(ReturnCodeElement.NotExistVIPLevel));

        /// <summary> 已领取生日礼金</summary>
        public static ReturnCode BirthdayGiftMoneyReceived = new ReturnCode("E00070", typeof(ReturnCodeElement), nameof(ReturnCodeElement.BirthdayGiftMoneyReceived));

        /// <summary>礼金领取时间过期</summary>
        public static ReturnCode GiftMoneyReceiveExpired = new ReturnCode("E00071", typeof(ReturnCodeElement), nameof(ReturnCodeElement.GiftMoneyReceiveExpired));

        /// <summary>綁定失敗，戶名需與最近一次綁定銀行卡同名(配合sp代碼改為中文)</summary>
        public static ReturnCode MustTheSameCardUser = new ReturnCode("绑定失败，只能绑定同名银行卡", nameof(ReturnCodeElement.MustTheSameCardUser));

        // *************************************************************************************************************************************************

        #region 註冊頁使用的 Register Page

        /// <summary>注册失败，请联系你的上线</summary>
        public static ReturnCode RegisterFailedPleaseCallYourUpline = new ReturnCode("E00076", typeof(ReturnCodeElement), nameof(ReturnCodeElement.RegisterFailedPleaseCallYourUpline));

        /// <summary>用户名已经存在！</summary>
        public static ReturnCode UsernameExists = new ReturnCode("E00077", typeof(ReturnCodeElement), nameof(ReturnCodeElement.UsernameExists));

        /// <summary>用户名包含非法字符！</summary>
        public static ReturnCode UsernameHaveInvalidChars = new ReturnCode("E00078", typeof(ReturnCodeElement), nameof(ReturnCodeElement.UsernameHaveInvalidChars));

        /// <summary>密码包含非法字符！</summary>
        public static ReturnCode PasswordHaveInvalidChars = new ReturnCode("E00079", typeof(ReturnCodeElement), nameof(ReturnCodeElement.PasswordHaveInvalidChars));

        /// <summary>用户名字符过长！</summary>
        public static ReturnCode UsernameTooLong = new ReturnCode("E00080", typeof(ReturnCodeElement), nameof(ReturnCodeElement.UsernameTooLong));

        /// <summary>密码字符请输入6~16个字符！</summary>
        public static ReturnCode PasswordLenthInvalid = new ReturnCode("E00081", typeof(ReturnCodeElement), nameof(ReturnCodeElement.PasswordLenthInvalid));

        /// <summary>链接已失效！</summary>
        public static ReturnCode LinkExpired = new ReturnCode("E00082", typeof(ReturnCodeElement), nameof(ReturnCodeElement.LinkExpired));

        /// <summary>注册失败，稍后重试！</summary>
        public static ReturnCode RegisterFailedPleaseTryLater = new ReturnCode("E00083", typeof(ReturnCodeElement), nameof(ReturnCodeElement.RegisterFailedPleaseTryLater));

        #endregion 註冊頁使用的 Register Page

        /// <summary>该用户未绑定手机</summary>
        public static ReturnCode UserUnboundPhoneNumber = new ReturnCode("E00086", typeof(ReturnCodeElement), nameof(ReturnCodeElement.UserUnboundPhoneNumber));

        /// <summary>该用户已解绑手机，无法通过审核</summary>
        public static ReturnCode UserAlreadyUnbindPhoneNumber = new ReturnCode("E00087", typeof(ReturnCodeElement), nameof(ReturnCodeElement.UserAlreadyUnbindPhoneNumber));

        /// <summary>真实姓名必须与银行卡姓名一致</summary>
        public static ReturnCode RealNameMustSameAsBankCardName = new ReturnCode("E00088", typeof(ReturnCodeElement), nameof(ReturnCodeElement.RealNameMustSameAsBankCardName));

        /// <summary>扣减分数过大！扣减分数不能大于可用积分</summary>
        public static ReturnCode InsufficientBalance = new ReturnCode("E00089", typeof(ReturnCodeElement), nameof(ReturnCodeElement.InsufficientBalance));

        /// <summary>请先绑定手机号码</summary>
        public static ReturnCode PleaseBoundPhoneNumber = new ReturnCode("E00090", typeof(ReturnCodeElement), nameof(ReturnCodeElement.PleaseBoundPhoneNumber));

        /// <summary>验证码失效，请重新发送</summary>
        public static ReturnCode ValidateCodeTimeout = new ReturnCode("E00091", nameof(ReturnCodeElement.ValidateCodeTimeout));

        /// <summary>验证码不能为空</summary>
        public static ReturnCode ValidateCodeIsNotCompleted = new ReturnCode("E00092", nameof(ReturnCodeElement.ValidateCodeIsNotCompleted));

        /// <summary>验证失败，请重新操作</summary>
        public static ReturnCode ValidateFailedPleaseRetry = new ReturnCode("E00093", nameof(ReturnCodeElement.ValidateFailedPleaseRetry));

        /// <summary>没有记录被更新</summary>
        public static ReturnCode NoRecordIsUpdated = new ReturnCode("E00094", nameof(ReturnCodeElement.NoRecordIsUpdated));

        /// <summary>已领取月红包</summary>
        public static ReturnCode MonthlyRedEnvelopeReceived = new ReturnCode("E00095", nameof(ReturnCodeElement.MonthlyRedEnvelopeReceived));

        /// <summary>此手机号码绑定超过上限值</summary>
        public static ReturnCode PhoneBoundExceedMaxLimitCount = new ReturnCode("E00104", nameof(ReturnCodeElement.PhoneBoundExceedMaxLimitCount));

        /// <summary>此邮箱绑定超过上限值</summary>
        public static ReturnCode EmailBoundExceedMaxLimitCount = new ReturnCode("E00105", nameof(ReturnCodeElement.EmailBoundExceedMaxLimitCount));

        /// <summary>此帐号已被使用</summary>
        public static ReturnCode UserNameAlreadyUsed = new ReturnCode("E00106", nameof(ReturnCodeElement.UserNameAlreadyUsed));

        /// <summary>非前台用戶不可操作！</summary>
        public static ReturnCode NonFrontSideUserCannotOperate = new ReturnCode("E00107", nameof(ReturnCodeElement.NonFrontSideUserCannotOperate));

        /// <summary>HttpStatusCode不是200,HttpStatus為{0}</summary>
        public static ReturnCode HttpStatusCodeNotOK = new ReturnCode("E00108", nameof(ReturnCodeElement.HttpStatusCodeNotOK));

        /// <summary>系统检测到您的注单存在异常，正在审核过程中，请联系客服或稍后重试(配合sp代碼改為中文)</summary>
        public static ReturnCode WithdrawHasRiskStatus = new ReturnCode(
            "系统检测到您的注单存在异常，正在审核过程中，请联系客服或稍后重试",
            nameof(ReturnCodeElement.WithdrawHasRiskStatus))
        {
            IsSuccess = true
        };

        /// <summary>查无订单号</summary>
        public static ReturnCode OrderNotFound = new ReturnCode("E00110", typeof(ReturnCodeElement), nameof(ReturnCodeElement.OrderNotFound));

        /// <summary>系统检测到您的申请存在异常，正在审核过程中，请联系客服咨询详情</summary>
        public static ReturnCode AppliedFinanceHasRiskStatus = new ReturnCode("E00111", typeof(ReturnCodeElement), nameof(ReturnCodeElement.AppliedFinanceHasRiskStatus));

        /// <summary>密码不能为空</summary>
        public static ReturnCode PasswordCannotBeEmpty = new ReturnCode("E00113", nameof(ReturnCodeElement.PasswordCannotBeEmpty));

        /// <summary>密码前后不可输入空格</summary>
        public static ReturnCode PasswordCannotInputSpaces = new ReturnCode("E00114", nameof(ReturnCodeElement.PasswordCannotInputSpaces));

        /// <summary>您当前访问的平台正在维护中(0)</summary>
        public static ReturnCode Maintaining_0 = new ReturnCode("E00125", typeof(CommonElement), nameof(CommonElement.Maintaining_0));

        /// <summary>無可用提現方式</summary>
        public static ReturnCode NoAvailableWithdrawTypes = new ReturnCode("E00126", typeof(ReturnCodeElement), nameof(ReturnCodeElement.NoAvailableWithdrawTypes));

        /// <summary>手机号已被使用</summary>
        public static ReturnCode PhoneNumberInUse = new ReturnCode("E00127", typeof(ReturnCodeElement), nameof(ReturnCodeElement.PhoneNumberInUse));

        /// <summary>您未设置邮箱</summary>
        public static ReturnCode EmailIsEmpty = new ReturnCode("E00128", typeof(ReturnCodeElement), nameof(ReturnCodeElement.EmailIsEmpty));

        /// <summary>联系客服</summary>
        public static ReturnCode ContactCustomerService = new ReturnCode("E00129", typeof(ReturnCodeElement), nameof(ReturnCodeElement.ContactCustomerService));

        /// <summary>异动金额有误，请重新输入</summary>
        public static ReturnCode AdjustAmountError = new ReturnCode("E00130", typeof(ReturnCodeElement), nameof(ReturnCodeElement.AdjustAmountError));

        /// <summary>該用戶已更換新的代理登錄密碼，無法通過審核</summary>
        public static ReturnCode VIPAgentLoginPasswordChanged = new ReturnCode("E00131", nameof(ReturnCodeElement.VIPAgentLoginPasswordChanged));

        /// <summary>查无此用户名</summary>
        public static ReturnCode UserNameNotFound = new ReturnCode("E00132", nameof(ReturnCodeElement.UserNameNotFound));

        /// <summary>资金密码不能为空</summary>
        public static ReturnCode MoneyPasswordCannotBeEmpty = new ReturnCode("E00134", nameof(ReturnCodeElement.MoneyPasswordCannotBeEmpty));

        /// <summary>提款流水倍数格式错误</summary>
        public static ReturnCode FlowMultipleFormatIncorrect = new ReturnCode("E00137", nameof(ReturnCodeElement.FlowMultipleFormatIncorrect));

        /// <summary>请输入1到500000</summary>
        public static ReturnCode PleaseInsertAccurateAmount = new ReturnCode("E00138", nameof(ReturnCodeElement.PleaseInsertAccurateAmount));

        /// <summary>余额不足，当前余额：¥{0}</summary>
        public static ReturnCode BalanceInsufficient = new ReturnCode("E00139", nameof(ReturnCodeElement.BalanceInsufficient));

        /// <summary>{0}已经存在</summary>
        public static readonly ReturnCode SomeDataTypeIsExists = new ReturnCode("E00140", nameof(ReturnCodeElement.SomeDataTypeIsExists));

        /// <summary>测试环境不允许此操作</summary>
        public static ReturnCode TestingEnvironmentIsNoPermission = new ReturnCode("E00141", nameof(ReturnCodeElement.NoTestingEnvironment));

        /// <summary>验证签章失败</summary>
        public static readonly ReturnCode ValidateSignFailed = new ReturnCode("E00142", nameof(ReturnCodeElement.ValidateSignFailed));

        /// <summary>提现金额格式错误</summary>
        public static ReturnCode WithdrawAmountError = new ReturnCode("E00143", nameof(ReturnCodeElement.WithdrawAmountError));

        /// <summary>账户已存在</summary>
        public static readonly ReturnCode UserInfoIsExist = new ReturnCode("E00145", nameof(ReturnCodeElement.UserInfoIsExist));

        /// <summary>奖金已领取</summary>
        public static readonly ReturnCode BonusMoneyReceived = new ReturnCode("E00146", nameof(ReturnCodeElement.BonusMoneyReceived));

        /// <summary>您输入的用户非代理身分</summary>
        public static readonly ReturnCode UserInfoIsNotVIPAgent = new ReturnCode("E00147", nameof(ReturnCodeElement.UserInfoIsNotVIPAgent));

        /// <summary>您输入的用户不可为此用户本身</summary>
        public static readonly ReturnCode ParentCanNotBeUserSelf = new ReturnCode("E00148", nameof(ReturnCodeElement.ParentCanNotBeUserSelf));

        /// <summary>您输入的用户不可为目前用户线下的下级代理</summary>
        public static readonly ReturnCode ParentCanNotBeNextLevelUsers = new ReturnCode("E00149", nameof(ReturnCodeElement.ParentCanNotBeNextLevelUsers));

        /// <summary>活动已结束</summary>
        public static readonly ReturnCode ActivityIsClosed = new ReturnCode("E00150", nameof(ReturnCodeElement.ActivityIsClosed));

        /// <summary>Invalid Signature</summary>
        public static readonly ReturnCode InvalidSignature = new ReturnCode("E00160", nameof(ReturnCodeElement.InvalidSignature));

        /// <summary>新密码与旧密码不能相同</summary>
        public static readonly ReturnCode NewPasswordCantSameAsOldPassword = new ReturnCode("E00161", nameof(ReturnCodeElement.NewPasswordCantSameAsOldPassword));

        /// <summary>资料已封存</summary>
        public static readonly ReturnCode DataIsArchived = new ReturnCode("E00162", nameof(ReturnCodeElement.DataIsArchived));

        /// <summary>本周红包已领取</summary>
        public static readonly ReturnCode ReceivedWeeklyRedEnvelope = new ReturnCode("E00163", nameof(ReturnCodeElement.ReceivedWeeklyRedEnvelope));

        /// <summary>已过期</summary>
        public static readonly ReturnCode Expired = new ReturnCode("E00164", nameof(ReturnCodeElement.Expired));

        /// <summary>角色不存在</summary>
        public static readonly ReturnCode RoleIsNotExist = new ReturnCode("E00165", nameof(ReturnCodeElement.RoleIsNotExist));

        /// <summary>有人员还使用此角色，不允许删除</summary>
        public static readonly ReturnCode NoDeletionForBWRole = new ReturnCode("E00166", nameof(ReturnCodeElement.NoDeletionForBWRole));

        /// <summary>未綁定GoogleAuthenticator</summary>
        public static ReturnCode UnboundGoogleAuthenticator = new ReturnCode("E00167", nameof(ReturnCodeElement.UnboundGoogleAuthenticator));

        /// <summary>GoogleAuthenticator过期</summary>
        public static ReturnCode ExpiredGoogleAuthenticator = new ReturnCode("E00168", nameof(ReturnCodeElement.ExpiredGoogleAuthenticator));

        /// <summary>Google身份验证即将到期！</summary>
        public static ReturnCode GoogleAuthExpiryNotice = new ReturnCode("E00169", nameof(ReturnCodeElement.GoogleAuthExpiryNotice));

        /// <summary>{0}格式错误</summary>
        public static readonly ReturnCode SomeDataTypeFormatFail = new ReturnCode("E00170", typeof(MessageElement), nameof(MessageElement.SomeDataTypeFormatFail));

        #region Information Code

        /// <summary>资料已进审核列表，请通知上级进行审核</summary>
        public static ReturnCode AuditInfoSubmit = new ReturnCode("I00001", typeof(ReturnCodeElement), nameof(ReturnCodeElement.AuditInfoSubmit), true);

        /// <summary>转账请求已提交，请耐心等候</summary>
        public static ReturnCode TransferSubmit = new ReturnCode("I00002", typeof(ReturnCodeElement), nameof(ReturnCodeElement.TransferSubmit), true);

        /// <summary>提款请求已提交，请耐心等候</summary>
        public static ReturnCode TransferOutSubmit = new ReturnCode("I00003", typeof(ReturnCodeElement), nameof(ReturnCodeElement.TransferOutSubmit), true);

        /// <summary>已赠送彩金</summary>
        public static ReturnCode GivePrizeAlready = new ReturnCode("I00004", typeof(ReturnCodeElement), nameof(ReturnCodeElement.GivePrizeAlready), true);

        /// <summary>转帐完成</summary>
        public static ReturnCode TransferMoneySuccess = new ReturnCode("I00005", typeof(ReturnCodeElement), nameof(ReturnCodeElement.TransferMoneySuccess), true);

        /// <summary>提现申请成功！</summary>
        public static ReturnCode WithdrawalApplicationSuccessful = new ReturnCode("I00006", typeof(UserRelatedElement), nameof(UserRelatedElement.WithdrawalApplicationSuccessful), isSuccess: true);

        /// <summary>已进行余额回收，因第三方转账处理时间较长，请耐心等候。可于【第三方账号与余额】查看转账结果</summary>
        public static ReturnCode ApplyRecycleTPGameAvailableScoresCompleted = new ReturnCode("I00007", typeof(ReturnCodeElement), nameof(ReturnCodeElement.ApplyTPGameRecycleScoresCompleted), isSuccess: true);

        /// <summary>代理代存申请成功！</summary>
        public static ReturnCode VIPAgentDepositForChildSuccess = new ReturnCode("I00008", typeof(ReturnCodeElement), nameof(ReturnCodeElement.VIPAgentDepositForChildSuccess), isSuccess: true);

        #endregion Information Code
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