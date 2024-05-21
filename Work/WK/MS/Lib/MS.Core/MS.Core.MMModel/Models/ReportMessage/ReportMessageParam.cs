using MS.Core.Models.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MS.Core.MMModel.Models.ReportMessage
{
    public class ReportMessageParam: PageParam
    {
        public string[] DeleteMessageIds { get; set; }
        public int UserId { get; set; }
    }
}
