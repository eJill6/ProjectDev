using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MMService.Attributes;
using MMService.Services;
using MS.Core.MM.Infrastructures.Extensions;
using MS.Core.MM.Models.Booking;
using MS.Core.MM.Models.Booking.Req;
using MS.Core.MM.Models.Post;
using MS.Core.MM.Models.Post.ServiceReq;
using MS.Core.MM.Services.interfaces;
using MS.Core.MMModel.Models;
using MS.Core.Models.Models;

namespace MMService.Controllers
{
    /// <summary>
    /// 官方贴相關
    /// </summary>
    public class OfficialPostController : ApiControllerBase
    {
        /// <summary>
        /// 贴子相關服務
        /// </summary>
        private readonly IPostService _post;

        /// <summary>
        /// 適配器
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// 預約相關服務
        /// </summary>
        private IBookingService BookingService { get; }

        /// <summary>
        /// 官方相關
        /// </summary>
        /// <param name="logger">log</param>
        /// <param name="bookingService">預約相關</param>
        /// <param name="post">贴子服務</param>
        /// <param name="mapper">適配器</param>
        public OfficialPostController(ILogger<OfficialPostController> logger,
            IBookingService bookingService,
            IPostService post,
            IMapper mapper) : base(logger)
        {
            BookingService = bookingService;
            _post = post;
            _mapper = mapper;
        }

        /// <summary>
        /// 贴子預約
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [UserFrequency(seconds: 3, times: 1)]
        public async Task<IActionResult> Booking(BookingOfficialData model)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                ReqBooking req = new()
                {
                    PaymentType = param.PaymentType,
                    PostId = param.PostId!,
                    PostPriceId = param.PostPriceId,
                    UserId = User.GetUserId(),
                };

                return await BookingService.Booking(req);
            }, model));
        }

        /// <summary>
        /// 預約詳情
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{postId}")]
        public async Task<IActionResult> BookingDetail(string postId)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                ReqBookingDetail req = new()
                {
                    PostId = param.PostId,
                    UserId = User.GetUserId(),
                };

                return await BookingService.GetBookingDetail(req);
            }, new { PostId = postId }));
        }
        /// <summary>
        /// 預約管理
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ManageBooking(ManageBookingData model)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                ReqManageBooking req = new()
                {
                    PageNo = model.PageNo,
                    Status = model.Status,
                    UserId = User.GetUserId(),
                };

                return await BookingService.GetManageBookings(req);
            }, model));
        }



        /// <summary>
        /// 我的預約
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> MyBooking(MyBookingData model)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                ReqMyBooking req = new()
                {
                    Status = model.Status,
                    UserId = User.GetUserId(),
                    PageNo = model.PageNo,
                };

                return await BookingService.GetMyBooking(req);
            }, model));
        }
        /// <summary>
        /// 訂單詳情
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{bookingId}")]
        public async Task<IActionResult> MyBookingDetail(string bookingId)
        {
            return ApiResult(await TryCatchProcedure(async (id) =>
            {
                ReqMyBookingDetail req = new()
                {
                    UserId = User.GetUserId(),
                    BookingId = id,
                };

                return await BookingService.GetMyBookingDetail(req);
            }, bookingId));
        }

        /// <summary>
        /// 取消預約
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        [HttpGet]
        [UserFrequency(seconds: 3, times: 1)]
        [Route("{bookingId}")]
        public async Task<IActionResult> Cancel(string bookingId)
        {
            return ApiResult(await TryCatchProcedure(async (id) =>
            {
                ReqBookingCancel req = new()
                {
                    UserId = User.GetUserId(),
                    BookingId = id,
                };

                return await BookingService.Cancel(req);
            }, bookingId));
        }

        /// <summary>
        /// 待接單退款
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        [HttpGet]
        [UserFrequency(seconds: 3, times: 1)]
        [Route("{bookingId}")]
        public async Task<IActionResult> Refund(string bookingId)
        {
            return ApiResult(await TryCatchProcedure(async (id) =>
            {
                ReqBookingCancel req = new()
                {
                    UserId = User.GetUserId(),
                    BookingId = id,
                };

                return await BookingService.Refund(req);
            }, bookingId));
        }

        /// <summary>
        /// 接受
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        [HttpGet]
        [UserFrequency(seconds: 3, times: 1)]
        [Route("{bookingId}")]
        public async Task<IActionResult> Accept(string bookingId)
        {
            return ApiResult(await TryCatchProcedure(async (id) =>
            {
                ReqBookingAccept req = new()
                {
                    UserId = User.GetUserId(),
                    BookingId = id,
                };

                return await BookingService.Accept(req);
            }, bookingId));
        }

        /// <summary>
        /// 確認完成
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        [HttpGet]
        [UserFrequency(seconds: 3, times: 1)]
        [Route("{bookingId}")]
        public async Task<IActionResult> Done(string bookingId)
        {
            return ApiResult(await TryCatchProcedure(async (id) =>
            {
                ReqBookingDone req = new()
                {
                    UserId = User.GetUserId(),
                    BookingId = id,
                };

                return await BookingService.Done(req);
            }, bookingId));
        }

        /// <summary>
        /// 刪除
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        [HttpGet]
        [UserFrequency(seconds: 3, times: 1)]
        [Route("{bookingId}")]
        public async Task<IActionResult> Delete(string bookingId)
        {
            return ApiResult(await TryCatchProcedure(async (id) =>
            {
                ReqBookingDelete req = new()
                {
                    UserId = User.GetUserId(),
                    BookingId = id,
                };

                return await BookingService.Delete(req);
            }, bookingId));
        }

        /// <summary>
        /// 申請退費
        /// </summary>
        /// <param name="model">刪除資料</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> ApplyRefund(ApplyRefundData model)
        {
            return ApiResult(await TryCatchProcedure(async () =>
            {
                var dm = _mapper.Map<ReqBookingRefund>(model);
                dm.UserId = User.GetUserId();

                return await BookingService.Refund(dm);
            }));
        }

        /// <summary>
        /// 新增發佈官方贴
        /// </summary>
        /// <param name="model">發佈資訊</param>
        /// <returns></returns>
        [HttpPost]
        [UserFrequency(seconds: 3, times: 1)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> AddPost(OfficialPostData model)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                var dm = _mapper.Map<ReqOfficialPostData>(model);
                dm.UserId = param.UserId ?? User?.UserId();
                dm.Nickname = User?.Nickname();

                return await _post.AddOfficialPost(dm);
            }, model));
        }

        /// <summary>
        /// 取得編輯官方贴子資料
        /// </summary>
        /// <param name="postId">贴 Id</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{postId}")]
        [ProducesResponseType(typeof(ApiResponse<OfficialPostEditData>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPostEditData(string postId)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                return await _post.GetOfficialPostEditData(new ReqPostIdUserId()
                {
                    UserId = (int)User?.UserId(),
                    PostId = postId
                });
            }, postId));
        }

        /// <summary>
        /// 編輯發佈的官方贴
        /// </summary>
        /// <param name="postId">贴 Id</param>
        /// <param name="model">發佈資訊</param>
        /// <returns></returns>
        [HttpPost]
        [Route("{postId}")]
        [UserFrequency(seconds: 3, times: 1)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> EditPost(string postId, OfficialPostData model)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                var dm = _mapper.Map<ReqOfficialPostData>(model);
                dm.UserId = param.Model.UserId ?? User?.UserId();
                dm.Nickname = User?.Nickname();
                dm.PostId = param.PostId;

                return await _post.EditOfficialPost(dm);
            }, (PostId: postId, Model: model)));
        }

        /// <summary>
        /// 查找官方贴子
        /// </summary>
        /// <param name="model">查詢條件</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<OfficialPostListViewModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> PostSearch(OfficialPostSearchParam model)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                var dm = _mapper.Map<ReqOfficialPostSearchParam>(model);
                dm.UserId = (int)User?.UserId();

                return await _post.OfficialPostSearch(dm);
            }, model));
        }

        /// <summary>
        /// 官方覓贴詳情
        /// </summary>
        /// <param name="postId">官方贴Id</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{postId}")]
        [ProducesResponseType(typeof(ApiResponse<OfficialPostDetail>), StatusCodes.Status200OK)]
        public async Task<IActionResult> PostDetail(string postId)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                return await _post.OfficialPostDetail(new ReqPostIdUserId()
                {
                    UserId = (int)User?.UserId(),
                    PostId = postId
                });
            }, postId));
        }

        /// <summary>
        /// 取得官方私信
        /// </summary>
        /// <param name="postId">官方贴Id</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<OfficialDM>), StatusCodes.Status200OK)]
        public async Task<IActionResult> DM(string postId)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                return await _post.GetOfficialDM(new ReqPostIdUserId()
                {
                    UserId = (int)User?.UserId(),
                    PostId = postId
                });
            }, postId));
        }

        /// <summary>
        /// 評論贴子
        /// </summary>
        /// <param name="model">評論資訊</param>
        /// <returns></returns>
        [HttpPost]
        [UserFrequency(seconds: 3, times: 1)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> AddComment(OfficialCommentData model)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                var dm = _mapper.Map<ReqOfficialCommentData>(model);
                dm.UserId = (int)User?.UserId();
                dm.Nickname = User?.Nickname();

                return await _post.AddOfficialComment(dm);
            }, model));
        }

        /// <summary>
        /// 取得編輯評論資訊
        /// </summary>
        /// <param name="commentId">評論Id</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{commentId}")]
        [ProducesResponseType(typeof(ApiResponse<OfficialCommentEditData>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCommentEditData(string commentId)
        {
            return ApiResult(await TryCatchProcedure(async (commentId) =>
            {
                return await _post.GetOfficialCommentEditData(new ReqCommentIdUserId()
                {
                    CommentId = commentId,
                    UserId = (int)User?.UserId()
                });
            }, commentId));
        }

        /// <summary>
        /// 編輯評論
        /// </summary>
        /// <param name="commentId">評論Id</param>
        /// <param name="model">評論資訊</param>
        /// <returns></returns>
        [HttpPost]
        [Route("{commentId}")]
        [UserFrequency(seconds: 3, times: 1)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> EditComment(string commentId, OfficialCommentData model)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                var dm = _mapper.Map<ReqOfficialCommentData>(model);
                dm.CommentId = param.CommentId;
                dm.UserId = (int)User?.UserId();
                dm.Nickname = User?.Nickname();

                return await _post.EditOfficialComment(dm);
            }, (CommentId: commentId, Model: model)));
        }

        /// <summary>
        /// 贴子評論清單
        /// </summary>
        /// <param name="postId">贴子id</param>
        /// <param name="pageNo">目前頁數</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{postId}")]
        [ProducesResponseType(typeof(ApiResponse<PageResultModel<OfficialCommentList>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CommentList(string postId, int pageNo)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                return await _post.OfficialCommentList(new ReqPostIdPageNo()
                {
                    PostId = param.PostId,
                    PageNo = pageNo
                });
            }, (PostId: postId, PageNo: pageNo)));
        }

        /// <summary>
        /// 官方店铺帖子列表
        /// </summary>
        /// <param name="model">查詢條件</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<OfficialPostListViewModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> OfficialPostList(ReqOfficialStorePost model)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                return await _post.OfficialPostList(param);
            }, model));
        }
        /// <summary>
        /// 获取我的官方店铺帖子列表
        /// </summary>
        /// <param name="model">查詢條件</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<OfficialPostListViewModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMyOfficialPostList(ReqMyOfficialStorePost model)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                param.UserId  = (int)User?.UserId();
                return await _post.GetMyOfficialPostList(param);
            }, model));
        }
    }
}