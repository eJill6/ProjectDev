using MS.Core.MM.Models.Entities.Post;
using MS.Core.MM.Models.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.Core.MM.Models.Booking.Res
{
    public class ResBookingCompleted
    {
        public List<MMBooking> Bookings { get; set; }
        public List<MMPost> Posts { get; set; }

        public List<MMUserSummary> InsertUserSummary { get; set; }
        public List<MMUserSummary> UpdateUserSummary { get; set; }
    }
}
