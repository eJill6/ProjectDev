using JxBackendService.Common.Util;
using JxBackendService.Model.Attributes;
using JxBackendService.Model.Param.ThirdParty;
using JxBackendService.Resource.Element;
using System;

namespace JxBackendService.Model.ViewModel.TransferRecord
{
    public class TransferRecordViewModel
    {
        [Export(ResourcePropertyName = nameof(DisplayElement.TransferSource), ResourceType = typeof(DisplayElement), Sort = 1)]
        public string TransferSource { get; set; }

        [Export(ResourcePropertyName = nameof(DisplayElement.TransferTarget), ResourceType = typeof(DisplayElement), Sort = 3)]
        public string TransferTarget { get; set; }

        public int TransferType { get; set; }

        public SearchTransferType SearchTransferType => SearchTransferType.GetSingle(TransferType);

        [Export(ResourcePropertyName = nameof(DisplayElement.TransferType), ResourceType = typeof(DisplayElement), Sort = 2)]
        public string TransferTypeText => SearchTransferType?.Name;

        public string TransferTypeTextCss => SearchTransferType?.DisplayTextCssClass;

        [Export(ResourcePropertyName = nameof(DisplayElement.OrderNo), ResourceType = typeof(DisplayElement), Sort = 4)]
        public string OrderID { get; set; }

        [Export(ResourcePropertyName = nameof(DisplayElement.UserID), ResourceType = typeof(DisplayElement), Sort = 5)]
        public int UserID { get; set; }

        public decimal Amount { get; set; }

        [Export(ResourcePropertyName = nameof(DisplayElement.Amount), ResourceType = typeof(DisplayElement), Sort = 6)]
        public string AmountText => Amount.ToCurrency();

        public DateTime OrderTime { get; set; }

        [Export(ResourcePropertyName = nameof(DisplayElement.OrderTime), ResourceType = typeof(DisplayElement), Sort = 7)]
        public string OrderTimeText => OrderTime.ToFormatDateTimeString();

        public short Status { get; set; }

        [Export(ResourcePropertyName = nameof(DisplayElement.Status), ResourceType = typeof(DisplayElement), Sort = 8)]
        public string StatusText { get; set; }

        public DateTime? HandTime { get; set; }

        [Export(ResourcePropertyName = nameof(DisplayElement.HandleTime), ResourceType = typeof(DisplayElement), Sort = 9)]
        public string HandTimeText => HandTime.ToFormatDateTimeString();

        [Export(ResourcePropertyName = nameof(DisplayElement.Remark), ResourceType = typeof(DisplayElement), Sort = 10)]
        public string Memo { get; set; }

    }
}