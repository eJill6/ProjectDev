using MS.Core.MM.Models.Booking.Req;
using MS.Core.MM.Models.Booking.Res;
using MS.Core.MM.Models.Entities.Post;
using MS.Core.MM.Models.Entities.User;
using MS.Core.Models;
using MS.Core.Models.Models;

namespace MS.Core.MM.Services.interfaces
{
    public interface IBookingService
    {
        Task<BaseReturnDataModel<ResBookingAccept>> Accept(ReqBookingAccept req);
        Task<BaseReturnDataModel<ResBooking>> Booking(ReqBooking req);
        Task<BaseReturnDataModel<ResBookingCancel>> Cancel(ReqBookingCancel req);
        Task<BaseReturnDataModel<ResBookingCancel>> Refund(ReqBookingCancel req);
        Task<BaseReturnDataModel<ResBookingDelete>> Delete(ReqBookingDelete req);
        Task<BaseReturnDataModel<ResBookingDone>> Done(ReqBookingDone req);
        Task<BaseReturnDataModel<PageResultModel<ResManageBooking>>> GetManageBookings(ReqManageBooking req);
        Task<BaseReturnDataModel<PageResultModel<ResMyBooking>>> GetMyBooking(ReqMyBooking req);

        Task<BaseReturnDataModel<MMBooking>> GetBookingInfo(string bookingId);

        /// <summary>
        /// 预约中数量
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<int> GetInProgressBookingCount(int userId);

        Task<BaseReturnDataModel<ResMyBookingDetail>> GetMyBookingDetail(ReqMyBookingDetail req);

        Task<BaseReturnDataModel<ResBookingRefund>> Refund(ReqBookingRefund req);

        Task TimeoutNoAcceptance();

        /// <summary>
        /// 后台审核预约退款
        /// </summary>
        /// <param name="refundModel"></param>
        /// <returns></returns>
        Task Refund(MMApplyRefund refundModel);
        /// <summary>
        /// 派發
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        Task Distribute(int size);
        /// <summary>
        /// 48小时候将服务中的订单设为已完成
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        Task SetBookingCompleted(int size);
        /// <summary>
        /// 預約詳情
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<BaseReturnDataModel<ResBookingDetail>> GetBookingDetail(ReqBookingDetail req);
    }
}
