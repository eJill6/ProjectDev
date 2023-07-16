using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JxBackendService.Model.ErrorHandle
{
    public class SendTelegramErrorReportContentParam
    {
        public string errorCode { get; set; }

        public string httpStatusCode { get; set; }

        public string message { get; set; }

        public string url { get; set; }
    }
}