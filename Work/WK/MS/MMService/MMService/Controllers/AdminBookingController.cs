using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MS.Core.MM.Repos.interfaces;
using MS.Core.MM.Services.interfaces;
using MS.Core.MMModel.Models;
using MS.Core.MMModel.Models.AdminBooking;
using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.Models;
using MS.Core.Models.Models;

namespace MMService.Controllers
{
    public class AdminBookingController : ApiControllerBase
    {
        /// <summary>
        /// 预约单相關Repo
        /// </summary>
        private readonly IBookingRepo _bookingRepo;

        /// <summary>
        /// 预约单相關Service
        /// </summary>
        private readonly IBookingService _bookingService;

        /// <summary>
        /// 会员相關Repo
        /// </summary>
        private readonly IUserInfoRepo _userInfoRepo;

        /// <summary>
        /// 媒体相关服務
        /// </summary>
        private readonly IEnumerable<IMediaService> _mediaServices;

        /// <summary>
        /// 型別轉換
        /// </summary>
        private readonly IMapper _mapper;

        public AdminBookingController(
            IBookingRepo bookingRepo,
            IBookingService bookingService,
            IUserInfoRepo userInfoRepo,
            IEnumerable<IMediaService> mediaServices,
            IMapper mapper,
            ILogger logger) : base(logger)
        {
            _bookingRepo = bookingRepo;
            _bookingService = bookingService;
            _userInfoRepo = userInfoRepo;
            _mediaServices = mediaServices;
            _mapper = mapper;
        }

        /// <summary>
        /// 预约单记录
        /// </summary>
        /// <param name="model">查詢條件</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<PageResultModel<AdminBookingList>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> List(AdminBookingListParam model)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                var pagedBookingList = await _bookingRepo.List(model);
                var result = new BaseReturnDataModel<PageResultModel<AdminBookingList>>
                {
                    DataModel = _mapper.Map<PageResultModel<AdminBookingList>>(pagedBookingList)
                };
                result.SetCode(ReturnCode.Success);
                return result;
            }, model));
        }

        /// <summary>
        /// 预约退款申请记录
        /// </summary>
        /// <param name="model">查詢條件</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<PageResultModel<AdminBookingRefundList>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> RefundApplyList(AdminBookingRefundListParam model)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                var bookingRefundList = new List<AdminBookingRefundList>();
                var pagedBookingList = await _bookingRepo.RefundApplyList(model);
                foreach (var item in pagedBookingList.Data)
                {
                    var booking = await _bookingRepo.GetById(item.BookingId);
                    bookingRefundList.Add(new AdminBookingRefundList
                    {
                        RefundId = item.RefundId,
                        BookingId = item.BookingId,
                        PostId = booking.PostId,
                        UserId = item.UserId,
                        PaymentType = (int)booking.PaymentType,
                        PaymentMoney = booking.PaymentMoney,
                        Discount = booking.Discount,
                        ReasonType = item.ReasonType,
                        ApplyTime = item.ApplyTime,
                        ExamineTime = item.ExamineTime,
                        Status = item.Status,
                        Memo = item.Memo
                    });
                }
                var result = new BaseReturnDataModel<PageResultModel<AdminBookingRefundList>>
                {
                    DataModel = new PageResultModel<AdminBookingRefundList>
                    {
                        Data = bookingRefundList.ToArray(),
                        TotalCount = pagedBookingList.TotalCount,
                        PageNo = pagedBookingList.PageNo,
                        PageSize = pagedBookingList.PageSize,
                        TotalPage = pagedBookingList.TotalPage
                    }
                };
                result.SetCode(ReturnCode.Success);
                return result;
            }, model));
        }

        /// <summary>
        /// 预约退款审核详情
        /// </summary>
        /// <param name="refundId">退款Id</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{refundId}")]
        [ProducesResponseType(typeof(ApiResponse<AdminBookingRefundDetail>), StatusCodes.Status200OK)]
        public async Task<IActionResult> RefundDetail(string refundId)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                var result = new BaseReturnDataModel<AdminBookingRefundDetail>();
                var bookingRefund = await _bookingRepo.GetRefundById(refundId);
                var service = _mediaServices.FirstOrDefault(x => x.SourceType == SourceType.Refund);
                if (bookingRefund != null && service != null)
                {
                    var booking = await _bookingRepo.GetById(bookingRefund.BookingId);
                    var media = await service.Get(SourceType.Refund, new string[] { bookingRefund.RefundId });
                    return new BaseReturnDataModel<AdminBookingRefundDetail>(ReturnCode.Success)
                    {
                        DataModel = new AdminBookingRefundDetail
                        {
                            RefundId = refundId,
                            BookingId = bookingRefund.BookingId,
                            UserId = bookingRefund.UserId,
                            Contact = booking.Contact,
                            PostId = booking.PostId,
                            PaymentType = (int)booking.PaymentType,
                            PaymentMoney = booking.PaymentMoney,
                            Discount = booking.Discount,
                            ReasonType = bookingRefund.ReasonType,
                            Reason = bookingRefund.Reason,
                            ApplyTime = bookingRefund.ApplyTime,
                            ProofPics = media.DataModel.Select(x => x.FullMediaUrl).ToArray(),
                            Status = bookingRefund.Status,
                            Memo = bookingRefund.Memo
                        }
                    };
                }
                result.SetCode(ReturnCode.DataIsNotExist);
                return result;
            }, refundId));
        }

        /// <summary>
        /// 预约退款审核
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> RefundAudit(AdminBookingRefundParam param)
        {
            return ApiSuccessResult(await TryCatchProcedure(async (param) =>
            {
                var refund = await _bookingRepo.GetRefundById(param.RefundId);
                if (refund != null)
                {
                    refund.Memo = param.Memo;
                    refund.ExamineMan = param.ExamineMan;
                    refund.Status = param.Status;
                    await _bookingService.Refund(refund);
                    return new BaseReturnModel(ReturnCode.Success);
                }
                return new BaseReturnModel(ReturnCode.DataIsNotExist);
            }, param));
        }
    }
}