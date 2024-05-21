using System;
using System.Collections.Generic;
using System.Text;

namespace MS.Core.MMModel.Models.AdminBooking
{
    public class AdminRefuseRefundUpdateModel
    {
        public string BookingId { get; set; }
        public int Status { get; set; }
        public DateTime ScheduledTime { get; set; }
    }
}
