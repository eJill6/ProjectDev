using MS.Core.MM.Model.Entities.Media;
using MS.Core.MM.Models.Booking;
using MS.Core.MM.Models.Booking.Res;
using MS.Core.MM.Models.Entities.Post;
using MS.Core.MM.Models.Entities.User;
using MS.Core.MM.Models.Filters;
using MS.Core.MMModel.Models.AdminBooking;
using MS.Core.MMModel.Models.Refund;
using MS.Core.Models;
using MS.Core.Models.Models;

namespace MS.Core.MM.Repos.interfaces
{
    public interface IBookingRepo
    {
        Task Booking(PostBookingModel postBooking);

        Task SetBookingCompleted(ResBookingCompleted resBookingCompleted);

        Task<MMBooking?> GetById(string bookingId, bool isWriteDb = false);

        Task<PageResultModel<MMBooking>> GetPageByFilter(PageBookingFilter filter);

        Task<PageResultModel<MMBooking>> GetBookingPageByFilter(MyOrderPageBookingFilter filter);

        Task<string> GetSequenceIdentity();

        Task<string> GetSequenceIdentity<T>() where T : BaseDBModel;

        Task Update(MMBooking booking);

        /// <summary>
        /// 取得預約的贴子
        /// </summary>
        /// <param name="filter">篩選條件</param>
        /// <returns></returns>
        Task<MMBooking[]> GetBookingPost(BookingFilter filter);

        /// <summary>
        /// 获取预约Count;
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<int> GetBookingCount(BookingFilter filter);

        /// <summary>
        /// 退款
        /// </summary>
        /// <param name="refund"></param>
        /// <returns></returns>
        Task Refund(RefundModel refund);

        /// <summary>
        /// 接受
        /// </summary>
        /// <param name="booking"></param>
        /// <returns></returns>
        Task Accept(BookingAccept booking);

        /// <summary>
        /// 從預約單號查詢退費申請單
        /// </summary>
        /// <param name="bookingIds">預約單id</param>
        /// <returns></returns>
        Task<MMApplyRefund[]> GetApplyRefundWithBookingId(string[] bookingIds);

        /// <summary>
        /// 申請退費
        /// </summary>
        /// <param name="bookingEntity">預約資料</param>
        /// <param name="applyRefundEntity">申請退費資料</param>
        /// <param name="mediaEntity">上傳圖檔</param>
        /// <returns></returns>
        Task<bool> ApplyRefund(MMBooking bookingEntity, MMApplyRefund applyRefundEntity, MMMedia[] mediaEntity);

        Task<MMBooking[]> GetUserBookingPost(BookingFilter filter);

        /// <summary>
        /// 获取预约数，被预约数，预约中数
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<int> GetInProgressBookingCount(int userId);

        /// <summary>
        /// 根据预约单Id获取贴子Id
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        Task<Dictionary<string, string>> List(string[] bookingIds);

        /// <summary>
        /// 查询预定单列表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<PageResultModel<MMBooking>> List(AdminBookingListParam param);

        /// <summary>
        /// 根据预约单Id获取贴子身份
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        Task<Dictionary<string, int?>> GetBookingIdentityByIds(string[] bookingIds);

        /// <summary>
        /// 查询预定单退款申请列表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<PageResultModel<MMApplyRefundAndBookingModel>> RefundApplyList(AdminBookingRefundListParam param);

        /// <summary>
        /// 根据退款申请ID查询退款申请详情
        /// </summary>
        /// <param name="refundId"></param>
        /// <returns></returns>
        Task<MMApplyRefund?> GetRefundById(string refundId);

        /// <summary>
        /// 后台拒绝退款
        /// </summary>
        /// <param name="refundModel"></param>
        /// <returns></returns>
        Task RefuseRefund(MMApplyRefund refundModel, AdminRefuseRefundUpdateModel refuseRefundModel);

        Task Done(BookingDone bookDone);

        Task Distribute(BookingDistributeModel bookingDistribute);

        /// <summary>
        /// 根据用户ID和发帖人ID获取状态不为完成的预约订单数
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="postUserId"></param>
        /// <returns></returns>
        Task<int> GetUserAndPostUserNotCompletedCount(int userId, int postUserId);

        Task<int> GetBookingPostCount(BookingFilter filter);
    }
}