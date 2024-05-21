using Microsoft.AspNetCore.Mvc;
using MMService.Attributes;
using MMService.Models.Post;
using MMService.Services;
using MS.Core.MM.Infrastructures.Extensions;
using MS.Core.MMModel.Models;
using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.Models.Models;

namespace MMService.Controllers
{
    /// <summary>
    /// 廣場相關
    /// </summary>
    public class SquareController : ApiControllerBase
    {
        /// <summary>
        /// 贴子相關服務
        /// </summary>
        private readonly IPostService _post;

        /// <summary>
        /// 廣場相關
        /// </summary>
        /// <param name="logger">log</param>
        /// <param name="post">贴子相關服務</param>
        public SquareController(ILogger<SquareController> logger,
            IPostService post) : base(logger)
        {
            _post = post;
        }

        /// <summary>
        /// 解鎖贴子
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [UserFrequency(seconds: 3, times: 1)]
        public async Task<IActionResult> UnlockPost(UnlockPostData model)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                return await _post.UnlockPost(new UnlockPostReqModel
                {
                    Nickname = User.GetNickname(),
                    UserId = User.GetUserId(),
                    PostId = param.PostId,
                });
            }, model));
        }

        /// <summary>
        /// 發贴說明
        /// </summary>
        /// <param name="postType">贴子類型(二期因調整使用宣傳類型，因此先做相容處理，三期再拿掉)</param>
        /// <param name="contentType">宣傳類型</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<WhatIsData>), StatusCodes.Status200OK)]
        public async Task<IActionResult> WhatIs(PostType postType, AdvertisingContentType contentType)
        {
            if (Enum.IsDefined(typeof(PostType), postType) &&
                !Enum.IsDefined(typeof(AdvertisingContentType), contentType))
            {
                contentType = (AdvertisingContentType)((int)postType);
            }

            return ApiResult(await TryCatchProcedure(async (contentType) =>
            {
                return await _post.WhatIs((AdvertisingContentType)contentType);
            }, contentType));
        }

        /// <summary>
        /// 新增發佈贴
        /// </summary>
        /// <param name="model">發佈資訊</param>
        /// <returns></returns>
        [HttpPost]
        [UserFrequency(seconds: 3, times: 1)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> AddPost(PostData model)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                var userId = param.UserId == null || param.UserId == default(int) ? User?.UserId() : null;

                return await _post.AddPost(userId, User?.Nickname(), param);
            }, model));
        }

        /// <summary>
        /// 取得編輯贴子資料
        /// </summary>
        /// <param name="postId">贴 Id</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{postId}")]
        [ProducesResponseType(typeof(ApiResponse<PostEditData>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPostEditData(string postId)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                return await _post.GetPostEditData(param, User?.UserId());
            }, postId));
        }

        /// <summary>
        /// 編輯發佈贴子
        /// </summary>
        /// <param name="postId">贴 Id</param>
        /// <param name="model">發佈資訊</param>
        /// <returns></returns>
        [HttpPost]
        [Route("{postId}")]
        [UserFrequency(seconds: 3, times: 1)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> EditPost(string postId, PostData model)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                return await _post.EditPost(param.PostId, User?.UserId(), User?.Nickname(), param.Model);
            }, (PostId: postId, Model: model)));
        }

        /// <summary>
        /// 舉報
        /// </summary>
        /// <param name="model">投訴資訊</param>
        /// <returns></returns>
        [HttpPost]
        [UserFrequency(seconds: 3, times: 1)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> Report(ReportData model)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                return await _post.Report(User?.UserId(), param);
            }, model));
        }

        /// <summary>
        /// 評論贴子
        /// </summary>
        /// <param name="model">評論資訊</param>
        /// <returns></returns>
        [HttpPost]
        [UserFrequency(seconds: 3, times: 1)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> AddComment(CommentData model)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                return await _post.AddComment(User?.UserId(), User?.Nickname(), param);
            }, model));
        }

        /// <summary>
        /// 取得編輯評論資訊
        /// </summary>
        /// <param name="commentId">評論Id</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{commentId}")]
        [ProducesResponseType(typeof(ApiResponse<CommentEditData>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCommentEditData(string commentId)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                return await _post.GetCommentEditData(param, User?.UserId());
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
        public async Task<IActionResult> EditComment(string commentId, CommentData model)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                return await _post.EditComment(param.CommentId, User?.UserId(), User?.Nickname(), param.Model);
            }, (CommentId: commentId, Model: model)));
        }

        /// <summary>
        /// 贴子評論清單
        /// </summary>
        /// <param name="postId">贴子id</param>
        /// <param name="page">目前頁數</param>
        /// <param name="pageNo">目前頁數</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{postId}")]
        [ProducesResponseType(typeof(ApiResponse<PageResultModel<CommentList>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CommentList(string postId, int page, int pageNo)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                //原page參數因為了與後台統一，因此先暫時先保留等四期刪除
                var tmpPageNo = param.Page;
                if (param.Page == default(int))
                {
                    tmpPageNo = param.PageNo == default(int) ? 1 : param.PageNo;
                }

                return await _post.CommentList(param.PostId, tmpPageNo);
            }, (PostId: postId, Page: page, PageNo: pageNo)));
        }

        /// <summary>
        /// 查找贴子。適用首頁、廣場
        /// </summary>
        /// <param name="model">查詢條件</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<PostListViewModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> PostSearch(PostSearchParam model)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                return await _post.PostSearch(User?.UserId(), param);
            }, model));
        }

        /// <summary>
        /// 覓贴詳情
        /// </summary>
        /// <param name="postId">贴子Id</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{postId}")]
        [ProducesResponseType(typeof(ApiResponse<PostDetail>), StatusCodes.Status200OK)]
        public async Task<IActionResult> PostDetail(string postId)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                return await _post.PostDetail(User?.UserId(), param);
            }, postId));
        }

        /// <summary>
        /// 覓贴收藏
        /// </summary>
        /// <param name="postId">贴子Id</param>
        /// <returns></returns>
        [HttpPost]
        [Route("{postId}")]
        [ProducesResponseType(typeof(ApiResponse<PostDetail>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Favorite(string postId)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                return await _post.PostFavoriteUpsert(User?.UserId(), param, 1);
            }, postId));
        }

        /// <summary>
        /// 店铺收藏
        /// </summary>
        /// <param name="applyId"></param>
        /// <returns></returns>
        [HttpPost("{applyId}")]
        [ProducesResponseType(typeof(ApiResponse<PostDetail>), StatusCodes.Status200OK)]
        public async Task<IActionResult> OfficialShopFollow(string applyId)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                return await _post.PostFavoriteUpsert(User?.UserId(), param, 2);
            }, applyId));
        }

        /// <summary>
        /// 查找贴子。適用首頁、廣場
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<PostList>), StatusCodes.Status200OK)]
        public async Task<IActionResult> RecommendPostList()
        {
            return ApiResult(await TryCatchProcedure(async () =>
            {
                return await _post.RecommendPostList(User?.UserId());
            }));
        }
    }
}