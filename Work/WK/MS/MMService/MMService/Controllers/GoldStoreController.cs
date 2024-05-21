using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MMService.Models.My;
using MMService.Models.Post;
using MMService.Services;
using MS.Core.MM.Infrastructures.Extensions;
using MS.Core.MM.Model.Entities.Media;
using MS.Core.MM.Models.Entities.Post;
using MS.Core.MM.Models.Entities.User;
using MS.Core.MM.Models.Media;
using MS.Core.MM.Models.Post;
using MS.Core.MM.Models.Post.ServiceReq;
using MS.Core.MM.Repos;
using MS.Core.MM.Repos.interfaces;
using MS.Core.MM.Services.interfaces;
using MS.Core.MMModel.Models;
using MS.Core.MMModel.Models.AdminPost;
using MS.Core.MMModel.Models.AdminUserManager;
using MS.Core.MMModel.Models.Media.Enums;
using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.MMModel.Models.User;
using MS.Core.Models;
using MS.Core.Models.Models;

namespace MMService.Controllers
{
    /// <summary>
    /// 后台金牌店铺
    /// </summary>
    public class GoldStoreController : ApiControllerBase
    {
        /// <summary>
        /// 贴子相關服務
        /// </summary>
        private readonly IPostService _service;

        /// <summary>
        /// 身份认证相关
        /// </summary>
        private readonly IIdentityApplyRepo _identityApply;

        /// <summary>
        /// 會員相關
        /// </summary>
        private readonly IUserInfoRepo _userInfoRepo;

        private readonly IMapper _mapper = null;

        /// <summary>
        /// 媒體服務
        /// </summary>
        private readonly IEnumerable<IMediaService> _mediaServices;

        /// <summary>
        /// 設定取得贴子圖片
        /// </summary>
        private IMediaService GetPostMedia => _mediaServices.First(m => m.SourceType == SourceType.Post && m.Type == MediaType.Image);

        private IMediaService GetMedia => _mediaServices.FirstOrDefault(m => (m.SourceType == SourceType.BossApply || m.SourceType == SourceType.BusinessPhoto) && m.Type == MediaType.Image);

        public GoldStoreController(IPostService service, IMapper mapper, IIdentityApplyRepo identityApply, IUserInfoRepo userInfoRepo, IEnumerable<IMediaService> mediaServices,
    ILogger logger) : base(logger)
        {
            _service = service;
            _mapper = mapper;
            _identityApply = identityApply;
            _userInfoRepo = userInfoRepo;
            _mediaServices = mediaServices;
        }

        /// <summary>
        /// 後台金牌店铺表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> List()
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                var result = new BaseReturnDataModel<IEnumerable<GoldStoreResult>>();
                var query = await _service.GetMMGoldStore();
                if (query.IsSuccess)
                {
                    result.DataModel = _mapper.Map<IEnumerable<GoldStoreResult>>(query.DataModel);
                    result.SetCode(ReturnCode.Success);
                }
                else
                {
                    result.SetModel(query);
                }
                return result;
            }, String.Empty));
        }

        /// <summary>
        /// 後台金牌店铺表修改
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Update(List<UpdateMMGoldStoreParam> param)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                param.ForEach(a => a.Operator = User?.Nickname());
                return await _service.UpdateMMGoldStore(param);
            }, param));
        }

        /// <summary>
        /// 官方店铺推荐列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> OfficialRecommendShopList()
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                var result = new BaseReturnDataModel<IEnumerable<GoldStoreViewModel>>();
                var query = await _service.GetMMGoldStore();

                List<GoldStoreViewModel> data = new List<GoldStoreViewModel>();
                foreach (var item in query.DataModel.Where(a => a.Type == 1))
                {
                    if (item.UserId == null)
                    {
                        continue;
                    }

                    var userInfo = await _userInfoRepo.GetUserInfo((int)item.UserId);
                    if (!userInfo.IsOpen || userInfo.UserIdentity != 3) 
                    {
                        continue;
                    }

                    var applyInfo = await _identityApply.DetailByUserId((int)item.UserId);
                    if (applyInfo == null)
                    {
                        continue;
                    }

                    // 根据审核id获取信息
                    var detailByUser = _mapper.Map<IdentityApplyForClient>(applyInfo);
                    if (detailByUser == null)
                    {
                        continue;
                    }
                    var boss = await _userInfoRepo.GetByApplyId(detailByUser.ApplyId);

                    if (query.IsSuccess && boss != null && !string.IsNullOrEmpty(boss.ShopName))
                    {
                        var shopAvatar = (await GetMedia.Get(SourceType.BossApply, new string[] { boss.BossId }))?.DataModel?.OrderBy(p => p.CreateDate);
                        //var businessPhotos = (await GetMedia.Get(SourceType.BusinessPhoto, new string[] { boss.ApplyId }))?.DataModel?.OrderBy(p => p.CreateDate);

                        int girls = 0;
                        int.TryParse(boss.Girls, out girls);
                        data.Add(new GoldStoreViewModel()
                        {
                            Top = item.Top,
                            ApplyId = boss.ApplyId,
                            ShopName = boss.ShopName,
                            Girls = girls,
                            ShopAvatarSource = shopAvatar.Any() ? shopAvatar.FirstOrDefault()?.FullMediaUrl : ""
                        });
                    }
                }
                result.DataModel = data;
                result.SetCode(ReturnCode.Success);
                return result;
            }, String.Empty));
        }

        /// <summary>
        /// 金牌店铺（官方首页）
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> OfficialGoldenShopList()
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                var result = new BaseReturnDataModel<IEnumerable<GoldStoreViewModel>>();
                var query = await _service.GetMMGoldStore();

                List<GoldStoreViewModel> data = new List<GoldStoreViewModel>();
                foreach (var item in query.DataModel.Where(a => a.Type == 2))
                {
                    if (item.UserId == null)
                    {
                        continue;
                    }

                    var userInfo = await _userInfoRepo.GetUserInfo((int)item.UserId);
                    if (!userInfo.IsOpen || userInfo.UserIdentity != 3)
                    {
                        continue;
                    }

                    var applyInfo = await _identityApply.DetailByUserId((int)item.UserId);
                    if (applyInfo == null || applyInfo.Status != 1)
                    {
                        continue;
                    }

                    // 根据审核id获取信息
                    var detailByUser = _mapper.Map<IdentityApplyForClient>(await _identityApply.DetailByUserId((int)item.UserId,1));
                    if (detailByUser == null)
                    {
                        continue;
                    }
                    var boss = await _userInfoRepo.GetByApplyId(detailByUser.ApplyId);

                    if (query.IsSuccess && boss != null && !string.IsNullOrEmpty(boss.ShopName))
                    {
                        var shopAvatar = (await GetMedia.Get(SourceType.BossApply, new string[] { boss.BossId }))?.DataModel?.OrderBy(p => p.CreateDate);
                        //var businessPhotos = (await GetMedia.Get(SourceType.BusinessPhoto, new string[] { boss.ApplyId }))?.DataModel?.OrderBy(p => p.CreateDate);

                        int girls = 0;
                        int.TryParse(boss.Girls, out girls);
                        data.Add(new GoldStoreViewModel()
                        {
                            Top = item.Top,
                            ApplyId = boss.ApplyId,
                            ShopName = boss.ShopName,
                            Girls = girls,
                            ShopAvatarSource = shopAvatar.Any() ? shopAvatar.FirstOrDefault()?.FullMediaUrl : ""
                        });
                    }
                }
                result.DataModel = data;
                result.SetCode(ReturnCode.Success);
                return result;
            }, String.Empty));
        }

        /// <summary>
        /// 官方店铺与帖子列表（搜索）
        /// </summary>
        /// <param name="reqOfficialShop"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> OfficialShopList(ReqOfficialShop reqOfficialShop)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                var result = new BaseReturnDataModel<PageResultModel<OfficialShopViewModel>>();
                List<OfficialShopViewModel> data = new List<OfficialShopViewModel>();
                var (officialShopList, rowCount) = await _userInfoRepo.OfficialShopList(reqOfficialShop);

                foreach (var shop in officialShopList)
                {
                    var postList = new List<OfficialShopPostList>();
                    var postData = await _service.GetOfficialPostByUserId(shop.UserId);
                    int count = 0;
                    foreach (var post in postData)
                    {
                        if ((string.IsNullOrWhiteSpace(param.Keyword) || (post.Title.Contains(param.Keyword))) && count < 3)
                        {
                            count++;
                            postList.Add(new OfficialShopPostList()
                            {
                                PostId = post.PostId,
                                Title = post.Title,
                                LowPrice = post.LowPrice,
                                CoverUrl = await GetPostMedia.GetFullMediaUrl(

                                                            new MMMedia()
                                                            {
                                                                FileUrl = post.CoverUrl
                                                            },
                                                            postType: PostType.Official,
                                                            isThumbnail: true)
                            });
                        }
                    }
                    var boss = await _userInfoRepo.GetByApplyId(shop.ApplyId);
                    var shopAvatar = (await GetMedia.Get(SourceType.BossApply, new string[] { boss.BossId }))?.DataModel?.OrderBy(p => p.CreateDate);

                    int girlsCount = 0;
                    int.TryParse(shop.Girls, out girlsCount);
                    data.Add(new OfficialShopViewModel()
                    {
                        ApplyId = shop.ApplyId,
                        ShopAvatarSource = shopAvatar.Any() ? shopAvatar.FirstOrDefault()?.FullMediaUrl : "",
                        ShopName = shop.ShopName,
                        Girls = girlsCount,
                        ViewBaseCount = shop.ViewBaseCount,
                        Views = shop.Views,
                        ShopYears = shop.ShopYears,
                        DealOrder = shop.DealOrder,
                        SelfPopularity = shop.SelfPopularity,
                        PostList = postList
                    });
                }
                result.DataModel = new PageResultModel<OfficialShopViewModel>
                {
                    PageNo = param.PageNo,
                    PageSize = param.PageSize,
                    TotalCount = rowCount,
                    Data = data.ToArray(),
                    TotalPage = rowCount % param.PageSize == 0 ? rowCount / param.PageSize : rowCount / param.PageSize + 1,
                };
                result.SetCode(ReturnCode.Success);
                return result;
            }, reqOfficialShop));
        }
        /// <summary>
        /// 获取自己的官方店铺信息
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        [ProducesResponseType(typeof(ApiResponse<IdentityApplyForClient>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMyOfficialShopDetail()
        {
            return ApiResult(await TryCatchProcedure(async () =>
            {
                if (!User.UserId().HasValue){
                    return new BaseReturnDataModel<IdentityApplyForClient>(ReturnCode.DataIsNotExist);
                }

                int UserId = User.GetUserId();

                var applyInfo= await _identityApply.DetailByUserId(UserId,1);

                if(applyInfo==null || string.IsNullOrEmpty(applyInfo.ApplyId))
                    return new BaseReturnDataModel<IdentityApplyForClient>(ReturnCode.DataIsNotExist);

                var boss = await _userInfoRepo.GetByApplyId(applyInfo.ApplyId);

                if(boss==null)
                {
                    return new BaseReturnDataModel<IdentityApplyForClient>(ReturnCode.DataIsNotExist);
                }
                var postList = await _service.GetOfficialPostByUserId(UserId);

                IdentityApplyForClient result = null;
                // 根据审核id获取信息
               
                result = new IdentityApplyForClient();
                result.ApplyId = boss.ApplyId;
                result.ShopName = boss.ShopName;
                result.Girls = string.IsNullOrWhiteSpace(boss.Girls) ? 0 : int.Parse(boss.Girls);
                result.ContactApp = applyInfo.ContactApp;
                result.ContactInfo = applyInfo.ContactInfo;
                result.DealOrder = boss.DealOrder;
                result.ShopYears = boss.ShopYears;
                result.SelfPopularity = boss.SelfPopularity;
                result.ViewBaseCount = boss.ViewBaseCount;
                result.Introduction = boss.Introduction;
                result.BusinessDate = boss.BusinessDate;
                result.BusinessHour = boss.BusinessHour;
                result.BossId = boss.BossId;
                result.AreaCodes = postList.Select(x => x.AreaCode.Trim()).Distinct().ToArray();

                var shopAvatar = (await GetMedia.Get(SourceType.BossApply, new string[] { boss.BossId }))?.DataModel?.OrderBy(p => p.CreateDate);
                var businessPhotos = (await GetMedia.Get(SourceType.BusinessPhoto, new string[] { boss.BossId }))?.DataModel?.OrderBy(p => p.CreateDate);

                result.ShopAvatarSource = shopAvatar?.FirstOrDefault(p => p.MediaType == (int)MediaType.Image)?.FullMediaUrl ?? "";
                result.BusinessPhotoSource = businessPhotos?.Where(p => p.MediaType == (int)MediaType.Image)?.Select(p => p.FullMediaUrl).ToArray();
                
                result.BusinessPhotoSourceViewModel= businessPhotos?.Where(p => p.MediaType == (int)MediaType.Image)?.Select(p => new MediaViewModel() { FullMediaUrl=p.FullMediaUrl,Id=p.Id}).ToArray();

                var favoriteData = await _service.GetMMPostFavorite(boss.ApplyId);
                result.IsFollow = favoriteData?.DataModel.Any() ?? false;


                return new BaseReturnDataModel<IdentityApplyForClient>(ReturnCode.Success)
                {
                    DataModel = result
                };

            }));
        }
        //public async Task<IActionResult> 

        /// <summary>
        /// 官方店铺详情
        /// </summary>
        /// <param name="applyId">审核id</param>
        /// <returns></returns>
        [HttpGet("{applyId}")]
        [ProducesResponseType(typeof(ApiResponse<IdentityApplyForClient>), StatusCodes.Status200OK)]
        public async Task<IActionResult> OfficialShopDetail(string applyId)
        {
            return ApiResult(await TryCatchProcedure(async () =>
            {
                var boss = await _userInfoRepo.GetByApplyId(applyId);

                IdentityApplyForClient result = null;
                // 根据审核id获取信息
                var detailById = await _identityApply.Detail(applyId);

                if (boss != null)
                {
                    await _userInfoRepo.UpdateShopViewsCount(applyId);

                    var postList = await _service.GetOfficialPostByUserId(detailById.UserId);

                    int girls = 0;
                    int.TryParse(boss.Girls, out girls);
                    result = new IdentityApplyForClient();
                    result.ApplyId = boss.ApplyId;
                    result.ShopName = boss.ShopName;
                    result.Girls = girls;
                    result.ContactApp = detailById.ContactApp;
                    result.ContactInfo = detailById.ContactInfo;
                    result.DealOrder = boss.DealOrder;
                    result.ShopYears = boss.ShopYears;
                    result.SelfPopularity = boss.SelfPopularity;
                    result.ViewBaseCount = boss.ViewBaseCount;
                    result.Views = boss.Views;
                    result.Introduction = boss.Introduction;
                    result.BusinessDate = boss.BusinessDate;
                    result.BusinessHour = boss.BusinessHour;
                    result.AreaCodes = postList.Select(x => x.AreaCode.Trim()).ToArray();

                    var shopAvatar = (await GetMedia.Get(SourceType.BossApply, new string[] { boss.BossId }))?.DataModel?.OrderBy(p => p.CreateDate);
                    var businessPhotos = (await GetMedia.Get(SourceType.BusinessPhoto, new string[] { boss.BossId }))?.DataModel?.OrderBy(p => p.CreateDate);

                    result.ShopAvatarSource = shopAvatar?.FirstOrDefault(p => p.MediaType == (int)MediaType.Image)?.FullMediaUrl ?? "";
                    result.BusinessPhotoSource = businessPhotos?.Where(p => p.MediaType == (int)MediaType.Image)?.Select(p => p.FullMediaUrl).ToArray();

                    var favoriteData = await _service.GetMMPostFavorite(boss.ApplyId);
                    result.IsFollow = favoriteData?.DataModel.Any(x => x.UserId == User.GetUserId()) ?? false;
                }
                if (result == null)
                {
                    return new BaseReturnDataModel<IdentityApplyForClient>(ReturnCode.DataIsNotExist)
                    {
                        DataModel = null,
                    };
                }
                return new BaseReturnDataModel<IdentityApplyForClient>(ReturnCode.Success)
                {
                    DataModel = result
                };
            }, applyId));
        }
    }
}