using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JxBackendService.Model.MiseLive.Response
{
    public class MiseLiveTransferOrder
    {
        public bool Success { get; set; }

        public int UserId { get; set; }

        public int TransferType { get; set; }

        public decimal Amount { get; set; }

        public string CreateTime { get; set; }
    }
}