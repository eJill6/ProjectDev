using JxBackendService.Model.Param.Audit;
using JxBackendService.Resource.Element;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.Enums
{
    public class AuditTypeValue : BaseIntValueModel<AuditTypeValue>
    {
        private AuditTypeValue() { }

        public string CheckFieldName { get; set; }

        public static readonly AuditTypeValue GivePrize = new AuditTypeValue()
        {
            Value = 0,
            ResourceType = typeof(AuditElement),
            ResourcePropertyName = nameof(AuditElement.AuditTypeGivePrize)
        };

        public static readonly AuditTypeValue BankName = new AuditTypeValue()
        {
            Value = 1,
            ResourceType = typeof(AuditElement),
            ResourcePropertyName = nameof(AuditElement.AuditTypeBankName),
            CheckFieldName = AuditElement.UserBankCard
        };

        public static readonly AuditTypeValue USDT = new AuditTypeValue()
        {
            Value = 2,
            ResourceType = typeof(AuditElement),
            ResourcePropertyName = nameof(AuditElement.AuditTypeUsdt),
            CheckFieldName = AuditElement.AuditTypeUsdt
        };

        public static readonly AuditTypeValue Mobile = new AuditTypeValue()
        {
            Value = 3,
            ResourceType = typeof(AuditElement),
            ResourcePropertyName = nameof(AuditElement.AuditTypeMobile),
            CheckFieldName = UserRelatedElement.PhoneNumber
        };

        public static readonly AuditTypeValue Email = new AuditTypeValue()
        {
            Value = 4,
            ResourceType = typeof(AuditElement),
            ResourcePropertyName = nameof(AuditElement.AuditTypeEmail),
            CheckFieldName = UserRelatedElement.Email
        };

        public static readonly AuditTypeValue UnbindUserAuthenticator = new AuditTypeValue()
        {
            Value = 5,
            ResourceType = typeof(AuditElement),
            ResourcePropertyName = nameof(AuditElement.UnbindUserAuthenticator),
        };

        public static readonly AuditTypeValue LoginPassword = new AuditTypeValue()
        {
            Value = 6,
            ResourceType = typeof(AuditElement),
            ResourcePropertyName = nameof(AuditElement.LoginPassword),
        };

        public static readonly AuditTypeValue MoneyPassword = new AuditTypeValue()
        {
            Value = 7,
            ResourceType = typeof(AuditElement),
            ResourcePropertyName = nameof(AuditElement.MoneyPassword),
        };

        public static readonly AuditTypeValue RegisterVIPAgent = new AuditTypeValue()
        {
            Value = 8,
            ResourceType = typeof(AuditElement),
            ResourcePropertyName = nameof(AuditElement.RegisterVIPAgent),
        };
    }

    public class AuditStatusType : BaseIntValueModel<AuditStatusType>
    {
        private AuditStatusType() { }

        public static readonly AuditStatusType None = new AuditStatusType()
        {
            Value = -1,
            ResourceType = typeof(AuditElement),
            ResourcePropertyName = nameof(AuditElement.None)
        };

        /// <summary>
        /// 待审核
        /// </summary>
        public static readonly AuditStatusType Unprocessed = new AuditStatusType()
        {
            Value = 0,
            ResourceType = typeof(AuditElement),
            ResourcePropertyName = nameof(AuditElement.AuditProcess)
        };

        /// <summary>
        /// 审核通过
        /// </summary>
        public static readonly AuditStatusType Pass = new AuditStatusType()
        {
            Value = 1,
            ResourceType = typeof(AuditElement),
            ResourcePropertyName = nameof(AuditElement.AuditPass)
        };

        /// <summary>
        /// 审核拒绝
        /// </summary>
        public static readonly AuditStatusType Reject = new AuditStatusType()
        {
            Value = 2,
            ResourceType = typeof(AuditElement),
            ResourcePropertyName = nameof(AuditElement.AuditReject)
        };

    }

    //public class GivePrizeTypes : BaseIntValueModel<GivePrizeTypes>
    //{
    //    private GivePrizeTypes() { } 

    //    public static readonly GivePrizeTypes Gift = new GivePrizeTypes()
    //    {
    //        Value = 0,
    //        ResourceType = typeof(AuditElement),
    //        ResourcePropertyName = nameof(AuditElement.Gift),
    //    };

    //    public static readonly GivePrizeTypes Commission = new GivePrizeTypes()
    //    {
    //        Value = 1,
    //        ResourceType = typeof(AuditElement),
    //        ResourcePropertyName = nameof(AuditElement.Commission),
    //    };

    //    public static readonly GivePrizeTypes Prize = new GivePrizeTypes()
    //    {
    //        Value = 2,
    //        ResourceType = typeof(AuditElement),
    //        ResourcePropertyName = nameof(AuditElement.Prize),
    //    };

    //    public static readonly GivePrizeTypes Deposit = new GivePrizeTypes()
    //    {
    //        Value = 3,
    //        ResourceType = typeof(AuditElement),
    //        ResourcePropertyName = nameof(AuditElement.Deposit),
    //    };

    //}
 }
