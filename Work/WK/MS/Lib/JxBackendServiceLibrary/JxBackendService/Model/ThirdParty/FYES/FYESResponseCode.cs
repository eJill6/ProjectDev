using JxBackendService.Model.Enums;

namespace JxBackendService.Model.ThirdParty.FYES
{
    public class FYESResponseSuccessCode : BaseIntValueModel<FYESResponseSuccessCode>
    {
        public static FYESResponseSuccessCode Unsuccess = new FYESResponseSuccessCode() { Value = 0 };

        public static FYESResponseSuccessCode Success = new FYESResponseSuccessCode() { Value = 1 };
    }

    public class FYESErrorResponseCode : BaseStringValueModel<FYESErrorResponseCode>
    {
        private FYESErrorResponseCode(string value)
        {
            Value = value;
        }

        /// <summary> 用户不存在 </summary>
        public static FYESErrorResponseCode NoUser = new FYESErrorResponseCode("NOUSER");

        /// <summary> 用户名不符合规则,用户名格式为数字+字母+下划线的组合，2~16位 </summary>
        public static FYESErrorResponseCode BadUserName = new FYESErrorResponseCode("BADNAME");

        /// <summary> 密码不符合规则，密码长度位5~16位 </summary>
        public static FYESErrorResponseCode BadPassword = new FYESErrorResponseCode("BADPASSWORD");

        /// <summary> 用户名已经存在 </summary>
        public static FYESErrorResponseCode UserExists = new FYESErrorResponseCode("EXISTSUSER");

        /// <summary> 金额错误,金额支持两位小数。 </summary>
        public static FYESErrorResponseCode BadMoney = new FYESErrorResponseCode("BADMONEY");

        /// <summary> 订单号错误（不符合规则或者不存在） </summary>
        public static FYESErrorResponseCode NoOrder = new FYESErrorResponseCode("NOORDER");

        /// <summary> 订单号已经存在，转账订单号为全局唯一 </summary>
        public static FYESErrorResponseCode ExistsOrder = new FYESErrorResponseCode("EXISTSORDER");

        /// <summary> 未指定转账动作，转账动作必须为 IN 或者  </summary>
        public static FYESErrorResponseCode NoTransferAction = new FYESErrorResponseCode("TRANSFER_NO_ACTION");

        /// <summary> IP未授权 </summary>
        public static FYESErrorResponseCode IPUnauthorized = new FYESErrorResponseCode("IP");

        /// <summary> 用户被锁定，禁止登录 </summary>
        public static FYESErrorResponseCode UserLock = new FYESErrorResponseCode("USERLOCK");

        /// <summary> 余额不足 </summary>
        public static FYESErrorResponseCode NoBalance = new FYESErrorResponseCode("NOBALANCE");

        /// <summary> 平台额度不足（适用于买分商户) </summary>
        public static FYESErrorResponseCode NoCredit = new FYESErrorResponseCode("NOCREDIT");

        /// <summary> API密钥错误 </summary>
        public static FYESErrorResponseCode AuthorizationError = new FYESErrorResponseCode("Authorization");

        /// <summary> 发生错误 </summary>
        public static FYESErrorResponseCode Failed = new FYESErrorResponseCode("Faild");

        /// <summary> 未配置域名（请与客服联系） </summary>
        public static FYESErrorResponseCode DomainError = new FYESErrorResponseCode("DOMAIN");

        /// <summary> 内容错误（提交的参数不符合规则） </summary>
        public static FYESErrorResponseCode ContentError = new FYESErrorResponseCode("CONTENT");

        /// <summary> 签名错误（适用于单一钱包的通信错误提示） </summary>
        public static FYESErrorResponseCode SignError = new FYESErrorResponseCode("Sign");

        /// <summary> 不支持该操作 </summary>
        public static FYESErrorResponseCode NoSupport = new FYESErrorResponseCode("NOSUPPORT");

        /// <summary> 超时请求 </summary>
        public static FYESErrorResponseCode Timeout = new FYESErrorResponseCode("TIMEOUT");

        /// <summary> 状态错误(商户被冻结） </summary>
        public static FYESErrorResponseCode StatusError = new FYESErrorResponseCode("STATUS");

        /// <summary> 商户信息配置错误（请联系客服处理） </summary>
        public static FYESErrorResponseCode ConfigError = new FYESErrorResponseCode("CONFIGERROR");

        /// <summary> 查询日期错误,日期超过了1天或者结束时间大于开始时间 </summary>
        public static FYESErrorResponseCode DateError = new FYESErrorResponseCode("DATEEROOR");

        /// <summary> 查询使用的订单号不存在 </summary>
        public static FYESErrorResponseCode OrderNotFound = new FYESErrorResponseCode("ORDER_NOTFOUND");

        /// <summary> 订单正在处理中 </summary>
        public static FYESErrorResponseCode Proccessing = new FYESErrorResponseCode("PROCCESSING");

        /// <summary> 系统维护中 </summary>
        public static FYESErrorResponseCode Maintenance = new FYESErrorResponseCode("MAINTENANCE");
    }
}