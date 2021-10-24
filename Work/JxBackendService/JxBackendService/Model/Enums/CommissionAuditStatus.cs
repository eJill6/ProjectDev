using JxBackendService.Resource.Element;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.Enums
{
    public class CommissionAuditStatus : BaseValueModel<byte, CommissionAuditStatus>
    {
        private CommissionAuditStatus() { }

        /// <summary>審核中</summary>
        public static readonly CommissionAuditStatus Verifing = new CommissionAuditStatus()
        {
            Value = 0,
            ResourceType = typeof(SelectItemElement),
            ResourcePropertyName = nameof(SelectItemElement.AuditStatus_Verifing)
        };

        /// <summary>可領取</summary>
        public static readonly CommissionAuditStatus Availabled = new CommissionAuditStatus()
        {
            Value = 1,
            ResourceType = typeof(SelectItemElement),
            ResourcePropertyName = nameof(SelectItemElement.AuditStatus_Availabled)
        };

        /// <summary>欠款</summary>
        public static readonly CommissionAuditStatus Debt = new CommissionAuditStatus()
        {
            Value = 2,
            ResourceType = typeof(SelectItemElement),
            ResourcePropertyName = nameof(SelectItemElement.AuditStatus_Debt)
        };

        /// <summary>欠款結清</summary>
        public static readonly CommissionAuditStatus NoDebt = new CommissionAuditStatus()
        {
            Value = 3,
            ResourceType = typeof(SelectItemElement),
            ResourcePropertyName = nameof(SelectItemElement.AuditStatus_NoDebt)
        };

        /// <summary>處理中</summary>
        public static readonly CommissionAuditStatus Processing = new CommissionAuditStatus()
        {
            Value = 4,
            ResourceType = typeof(SelectItemElement),
            ResourcePropertyName = nameof(SelectItemElement.AuditStatus_Processing)
        };

        /// <summary>已領取</summary>
        public static readonly CommissionAuditStatus Received = new CommissionAuditStatus()
        {
            Value = 9,
            ResourceType = typeof(SelectItemElement),
            ResourcePropertyName = nameof(SelectItemElement.AuditStatus_Received)
        };
    }    
}
