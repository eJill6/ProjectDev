using JxBackendService.Model.Enums;

namespace JxBackendService.Model.Param.ThirdParty
{
    public class CreateTransferOutOrderParam
    {
        public int UserId { get; set; }

        public decimal Amount { get; set; }

        public string TPGameAccount { get; set; }

        public TPGameMoneyOutOrderStatus TransferOutStatus { get; set; }
    }
}