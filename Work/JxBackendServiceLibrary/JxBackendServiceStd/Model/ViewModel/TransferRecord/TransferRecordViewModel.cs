using JxBackendService.Common.Util;
using JxBackendService.Model.Param.ThirdParty;
using System;

namespace JxBackendService.Model.ViewModel.TransferRecord
{
    public class TransferRecordViewModel
    {
        public string TransferSource { get; set; }

        public string TransferTarget { get; set; }

        public int TransferType { get; set; }

        public SearchTransferType SearchTransferType => SearchTransferType.GetSingle(TransferType);

        public string TransferTypeText => SearchTransferType?.Name;

        public string TransferTypeTextCss => SearchTransferType?.DisplayTextCssClass;

        public string OrderID { get; set; }

        public int UserID { get; set; }

        public decimal Amount { get; set; }

        public string AmountText => Amount.ToCurrency();

        public DateTime OrderTime { get; set; }

        public string OrderTimeText => OrderTime.ToFormatDateTimeString();

        public short Status { get; set; }

        public string StatusText { get; set; }

        public DateTime? HandTime { get; set; }

        public string HandTimeText => HandTime.ToFormatDateTimeString();

        public string Memo { get; set; }

    }
}