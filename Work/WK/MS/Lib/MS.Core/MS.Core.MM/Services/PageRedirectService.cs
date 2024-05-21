using Microsoft.Extensions.Logging;
using MS.Core.Extensions;
using MS.Core.Infrastructure.Redis;
using MS.Core.MM.Models.Entities.Post;
using MS.Core.MM.Models.Entities.User;
using MS.Core.MM.Repos.interfaces;
using MS.Core.MM.Services.interfaces;
using MS.Core.MMModel.Models.AdminUserManager;
using MS.Core.MMModel.Models.User.Enums;
using MS.Core.Models;
using MS.Core.Services;

namespace MS.Core.MM.Service
{
    /// <inheritdoc cref="IPageRedirectService"/>
    public class PageRedirectService : BaseService, IPageRedirectService
    {
        /// <inheritdoc cref="IPageRedirectRepo"/>
        private readonly IPageRedirectRepo _repo = null;

        private readonly IPostRepo _postRepo = null;

        private readonly IUserInfoRepo _userInfoRepo = null;

        /// <inheritdoc />
        public PageRedirectService(IPageRedirectRepo repo,
            IPostRepo postRepo,
            IUserInfoRepo userInfoRepo,
            ILogger<PageRedirectService> logger) : base(logger)
        {
            _repo = repo;
            _postRepo = postRepo;
            _userInfoRepo = userInfoRepo;
        }

        /// <inheritdoc />
        public async Task<BaseReturnModel> Update(MMPageRedirect param)
        {
            return await TryCatchProcedure(async (param) =>
            {
                //检查ApplyID是否存在
                if (param.Type == 4)
                {
                    var bossinfo = await _userInfoRepo.GetByApplyId(param.RefId);
                    if (bossinfo == null)
                    {
                        return new BaseReturnModel(ReturnCode.UserDoesNotBossApplyID);
                    }
                    var applyinfo = await _userInfoRepo.GetFavoriteApply(new string[] { param.RefId }).FirstOrDefaultAsync();
                    var userinfo = await _userInfoRepo.GetUserInfo(applyinfo.UserId);
                    if (userinfo.UserIdentity != (int)IdentityType.Boss && userinfo.UserIdentity != (int)IdentityType.SuperBoss)
                    {
                        return new BaseReturnModel(ReturnCode.UserDoesNotBossApplyID);
                    }
                }
                //检查官方帖子是否存在
                if (param.Type == 5)
                {
                    if (await _postRepo.GetOfficialPostById(param.RefId) == null)
                    {
                        return new BaseReturnModel(ReturnCode.PostIsNotExist);
                    };
                }
                //检查寻芳阁广场帖子是否存在
                if (param.Type == 6)
                {
                    if (await _postRepo.GetById(param.RefId) == null)
                    {
                        return new BaseReturnModel(ReturnCode.PostIsNotExist);
                    }
                }
                if (!(await _repo.Update(param)))
                {
                    return new BaseReturnModel(ReturnCode.UpdateFailed);
                }
                return new BaseReturnModel(ReturnCode.Success);
            }, param);
        }

        /// <inheritdoc />
        public async Task<BaseReturnDataModel<MMPageRedirect[]>> Get()
        {
            return await TryCatchProcedure<object, BaseReturnDataModel<MMPageRedirect[]>>(async (param) =>
            {
                var result = new BaseReturnDataModel<MMPageRedirect[]>();
                result.DataModel = await _repo.Get();
                result.SetCode(ReturnCode.Success);
                return result;
            }, null);
        }

        /// <inheritdoc />
        public async Task<BaseReturnDataModel<MMPageRedirect>> Get(int id)
        {
            return await TryCatchProcedure(async (param) =>
            {
                var result = new BaseReturnDataModel<MMPageRedirect>();
                result.DataModel = await _repo.Get(id);
                result.SetCode(ReturnCode.Success);
                return result;
            }, id);
        }
    }
}