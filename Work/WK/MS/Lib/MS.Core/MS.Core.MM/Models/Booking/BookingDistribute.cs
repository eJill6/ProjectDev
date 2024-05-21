using MS.Core.MM.Models.Booking.Enums;
using MS.Core.MM.Models.Entities.Post;
using MS.Core.MMModel.Models.User.Enums;

namespace MS.Core.MM.Models.Booking
{

    public class BookingDistribute
    {
        public string BookingId { get; set; } = null!;

        public string IncomeId { get; set; } = null!;

        public BookingStatus Status { get; set; }

        public IEnumerable<BookingStatus> Statuses { get; set; } = null!;
        /// <summary>
        /// 预定时发帖人身份
        /// </summary>
        public IdentityType? CurrentIdentity { get; set; }

        /// <summary>
        /// 超觅老板拆占比
        /// </summary>
        public decimal? PlatformSharing { get; set; }
    }
}
