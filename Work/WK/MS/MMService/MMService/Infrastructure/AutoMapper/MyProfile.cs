using AutoMapper;
using MMService.Models;
using MMService.Models.My;
using MS.Core.MM.Models.Post.ServiceReq;
using MS.Core.MM.Models.Post;
using MS.Core.MM.Models.Vip;
using MS.Core.MMModel.Models.My;
using MS.Core.Models.Models;
using MS.Core.MM.Models.Booking;
using MS.Core.MM.Models.Booking.Req;
using MS.Core.MM.Models.Entities.MessageUserRead;
using MS.Core.MM.Models.Entities.User;

namespace MMService.Infrastructure.AutoMapper
{
    public class MyProfile : Profile
    {
        public MyProfile()
        {
            CreateMap<UserInfoRes, UserInfoViewModel>();
            CreateMap<CenterInfo, CenterViewModel>();
            CreateMap<UserFavoriteStatistics, UserFavoriteStatisticsViewModel>();
            CreateMap<UserUnreadMessage, UserUnreadMessageViewModel>();
            CreateMap<UserSummaryInfo, UserSummaryViewModel>();
            CreateMap<ResUserVip, ResUserEfficientVipViewModel>();

            // 申請退費
            CreateMap<ApplyRefundData, ReqBookingRefund>();
        }

        private static string DateTimeToString(DateTime? time)
        {
            return time?.ToString(GlobalSettings.DateTimeFormat) ?? string.Empty;
        }
    }
}