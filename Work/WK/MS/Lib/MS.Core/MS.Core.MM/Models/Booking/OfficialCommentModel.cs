using MS.Core.MM.Models.Entities.Post;

namespace MS.Core.MM.Models.Booking
{
    public class OfficialCommentModel
    {
        public MMOfficialPostComment Comment { get; set; }
        public MMBooking Booking { get; set; }
    }
}
