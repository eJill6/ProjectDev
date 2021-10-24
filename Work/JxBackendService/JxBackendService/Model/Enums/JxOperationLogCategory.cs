using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.Enums
{
    /// <summary>
    /// 此為為了讓元件內也能使用的列舉
    /// </summary>
    public class JxOperationLogCategory : BaseIntValueModel<JxOperationLogCategory>
    {
        private JxOperationLogCategory() { }

        ///<summary>后台-会员管理</summary>
        public static JxOperationLogCategory Member = new JxOperationLogCategory()
        {
            Value = 0,
        };

        ///<summary>后台-充值</summary>
        public static JxOperationLogCategory Recharge = new JxOperationLogCategory()
        {
            Value = 1,
        };

        ///<summary>后台-提现</summary>
        public static JxOperationLogCategory Withdraw = new JxOperationLogCategory()
        {
            Value = 2,
        };

        ///<summary>后台-平台设置</summary>
        public static JxOperationLogCategory PlatSettings = new JxOperationLogCategory()
        {
            Value = 3,
        };

        ///<summary>后台-系统设置</summary>
        public static JxOperationLogCategory SystemSettings = new JxOperationLogCategory()
        {
            Value = 4,
        };

        ///<summary>后台-风险控制</summary>
        public static JxOperationLogCategory RiskControl = new JxOperationLogCategory()
        {
            Value = 5,
        };

        ///<summary>后台-开奖历史</summary>
        public static JxOperationLogCategory Lottery = new JxOperationLogCategory()
        {
            Value = 6,
        };

        ///<summary>后台-支付宝手动处理</summary>
        public static JxOperationLogCategory HandleAlipay = new JxOperationLogCategory()
        {
            Value = 7,
        };

        ///<summary>前台-会员资料异动</summary>
        public static JxOperationLogCategory ChangeUserInfo = new JxOperationLogCategory()
        {
            Value = 8,
        };

        ///<summary>前台-异常行为</summary>
        public static JxOperationLogCategory ExceptionalBehavior = new JxOperationLogCategory()
        {
            Value = 9,
        };
    }
}
