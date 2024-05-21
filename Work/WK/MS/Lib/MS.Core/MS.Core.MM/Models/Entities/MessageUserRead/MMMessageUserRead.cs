using MS.Core.MMModel.Models.Post.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.Core.MM.Models.Entities.MessageUserRead
{
    public class MMMessageUserRead
    {
        public int Id { get; set;}
        public MessageType MessageType { get; set;}
        public int MessageId { get; set; }
        public int UserId { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
