using JxBackendService.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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