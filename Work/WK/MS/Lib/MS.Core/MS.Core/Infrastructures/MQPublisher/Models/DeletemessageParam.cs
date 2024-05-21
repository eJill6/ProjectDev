using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.Core.Infrastructures.MQPublisher.Models
{
    public class DeletemessageParam
    {
        public string OwnerUserID { get; set; }

        public string RoomID { get; set; }

        public long SmallEqualThanTimestamp { get; set; }
    }
}
