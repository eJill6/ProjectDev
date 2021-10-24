using JxBackendService.Resource.Element;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.Enums
{
    public class TPGameMoneyInOrderStatus : BaseShortValueModel<TPGameMoneyInOrderStatus>
    {
        private TPGameMoneyInOrderStatus() { }

        public static TPGameMoneyInOrderStatus Unprocessed = new TPGameMoneyInOrderStatus()
        {
            Value = 0,
            ResourceType = typeof(SelectItemElement),
            ResourcePropertyName = nameof(SelectItemElement.TPGameMoneyOrderStatus_Unprocessed)
        };

        public static TPGameMoneyInOrderStatus Processing = new TPGameMoneyInOrderStatus()
        {
            Value = 1,
            ResourceType = typeof(SelectItemElement),
            ResourcePropertyName = nameof(SelectItemElement.TPGameMoneyOrderStatus_Processing)
        };

        public static TPGameMoneyInOrderStatus Success = new TPGameMoneyInOrderStatus()
        {
            Value = 2,
            ResourceType = typeof(SelectItemElement),
            ResourcePropertyName = nameof(SelectItemElement.TPGameMoneyInOrderStatus_Success)
        };

        public static TPGameMoneyInOrderStatus Refund = new TPGameMoneyInOrderStatus()
        {
            Value = 4,
            ResourceType = typeof(SelectItemElement),
            ResourcePropertyName = nameof(SelectItemElement.TPGameMoneyInOrderStatus_Refund)
        };

        public static TPGameMoneyInOrderStatus Manual = new TPGameMoneyInOrderStatus()
        {
            Value = 9,
            ResourceType = typeof(SelectItemElement),
            ResourcePropertyName = nameof(SelectItemElement.TPGameMoneyOrderStatus_Manual)
        };

    }

    public class TPGameMoneyOutOrderStatus : BaseShortValueModel<TPGameMoneyOutOrderStatus>
    {
        private TPGameMoneyOutOrderStatus() { }

        public static TPGameMoneyOutOrderStatus Unprocessed = new TPGameMoneyOutOrderStatus()
        {
            Value = 0,
            ResourceType = typeof(SelectItemElement),
            ResourcePropertyName = nameof(SelectItemElement.TPGameMoneyOrderStatus_Unprocessed)
        };

        public static TPGameMoneyOutOrderStatus Processing = new TPGameMoneyOutOrderStatus()
        {
            Value = 1,
            ResourceType = typeof(SelectItemElement),
            ResourcePropertyName = nameof(SelectItemElement.TPGameMoneyOrderStatus_Processing)
        };

        public static TPGameMoneyOutOrderStatus Success = new TPGameMoneyOutOrderStatus()
        {
            Value = 2,
            ResourceType = typeof(SelectItemElement),
            ResourcePropertyName = nameof(SelectItemElement.TPGameMoneyOutOrderStatus_Success)
        };

        public static TPGameMoneyOutOrderStatus Fail = new TPGameMoneyOutOrderStatus()
        {
            Value = 4,
            ResourceType = typeof(SelectItemElement),
            ResourcePropertyName = nameof(SelectItemElement.TPGameMoneyOutOrderStatus_Fail)
        };

        public static TPGameMoneyOutOrderStatus Manual = new TPGameMoneyOutOrderStatus()
        {
            Value = 9,
            ResourceType = typeof(SelectItemElement),
            ResourcePropertyName = nameof(SelectItemElement.TPGameMoneyOrderStatus_Manual)
        };
    }
}
