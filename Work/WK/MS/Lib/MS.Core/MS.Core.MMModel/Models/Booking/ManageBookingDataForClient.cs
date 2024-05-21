using MS.Core.MMModel.Attributes;
using MS.Core.MMModel.Models.Booking.Enums;
using MS.Core.Models.Models;
using System.ComponentModel;

namespace MS.Core.MMModel.Models.Booking
{
    public class ManageBookingDataForClient: PageParam
    {
        public ManageBookingStatusForClient Status { get; set; }

        public int PageNo { get; set; }
    }
}