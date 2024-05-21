using MS.Core.MMModel.Models.Post.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MS.Core.MMModel.Models.My
{
    public class MessageOperationParamForClient{
        public string[] MessageIds { get; set; }
        public int UserId { get; set; }
        public MessageType MessageType { get; set; }
        public MessageOperationType MessageOperationType { get; set; }
    }
}
