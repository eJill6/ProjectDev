using JxBackendService.Common.Util;
using JxBackendService.Resource.Element;
using System;

namespace JxBackendService.Model.Enums
{
    public class AssociationDataType : BaseIntValueModel<AssociationDataType>
    {
        public Func<string, string> ProcessAssociationData { get; private set; } = (associationData) => { return associationData; };

        public AssociationDataType()
        {
            ResourceType = typeof(SelectItemElement);
        }

        public static readonly AssociationDataType LoginIP = new AssociationDataType()
        {
            Value = (int)AssociationDataTypeEnum.LoginIP,
            ResourcePropertyName = nameof(SelectItemElement.AssociationDataType_LoginIP),
        };

        public static readonly AssociationDataType DeviceCode = new AssociationDataType()
        {
            Value = (int)AssociationDataTypeEnum.DeviceCode,
            ResourcePropertyName = nameof(SelectItemElement.AssociationDataType_DeviceCode),
        };

        public static readonly AssociationDataType CardUser = new AssociationDataType()
        {
            Value = (int)AssociationDataTypeEnum.CardUser,
            ResourcePropertyName = nameof(SelectItemElement.AssociationDataType_CardUser),
        };

        public static readonly AssociationDataType CardNo = new AssociationDataType()
        {
            Value = (int)AssociationDataTypeEnum.CardNo,
            ResourcePropertyName = nameof(SelectItemElement.AssociationDataType_CardNo),
            ProcessAssociationData = (cardNo) => { return cardNo.ToMaskBankCardNo(); }
        };
    }
}