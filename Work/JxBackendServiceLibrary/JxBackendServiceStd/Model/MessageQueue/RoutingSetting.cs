using System;
using System.Collections.Generic;
using System.Text;

namespace JxBackendService.Model.MessageQueue
{
    public class RoutingSetting
    {
        public string RoutingKey { get; set; }

        public string RequestId { get; set; }
    }

    public class TransferMessage : RoutingSetting
    {
        public string ProductCode { get; set; }

        public string Summary { get; set; }

        public bool IsReloadMiseLiveBalance { get; set; }
    }
}