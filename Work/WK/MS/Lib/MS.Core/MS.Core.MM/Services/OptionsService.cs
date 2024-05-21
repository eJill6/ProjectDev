using Microsoft.Extensions.Logging;
using MS.Core.MM.Models.Entities.Post;
using MS.Core.MM.Models.Post;
using MS.Core.MM.Repos.interfaces;
using MS.Core.MM.Services.Bases;
using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.Models;
using MS.Core.Services;

namespace MS.Core.MM.Services
{
    public class OptionsService : BaseService, IOptionsService
    {
        private readonly IOptionItemRepo _repo = null;
        public OptionsService(IOptionItemRepo repo, ILogger logger) : base(logger)
        {
            _repo = repo;
        }

        public async Task<BaseReturnModel> Create(CreateOptionsParam param)
        {
            return await TryCatchProcedure(async (param) =>
            {
                var insertResult = await _repo.Create(param);
                BaseReturnModel result = new BaseReturnModel();
                result.SetCode(new ReturnCode(insertResult.Code, insertResult.IsSuccess, insertResult.Msg));
                return result;
            }, param);
        }

        public async Task<BaseReturnModel> Delete(int OptionId)
        {
            return await TryCatchProcedure(async (param) =>
            {
                await _repo.Delete(OptionId);
                return new BaseReturnModel(ReturnCode.Success);
            }, OptionId);
        }

        public async Task<BaseReturnDataModel<MMOptions[]>> GetOptionByPostType(int postType)
        {
            return await TryCatchProcedure(async (param) =>
            {
                var result = new BaseReturnDataModel<MMOptions[]>();
                result.DataModel = await _repo.GetOptionByPostType(postType);
                result.SetCode(ReturnCode.Success);
                return result;
            }, postType);
        }

        public async Task<BaseReturnDataModel<MMOptions[]>> GetOptionsByPostTypeAndOptionType(PostType postType, OptionType optionType, int? OptionId)
        {
            return await TryCatchProcedure(async (param) =>
            {
                var result = new BaseReturnDataModel<MMOptions[]>();
                result.DataModel = await _repo.GetOptionsByPostTypeAndOptionType(postType, optionType, OptionId);
                result.SetCode(ReturnCode.Success);
                return result;
            }, postType);
        }

        public async Task<BaseReturnModel> Update(UpdateOptionsParam param)
        {
            return await TryCatchProcedure(async (param) =>
            {
                var insertResult = await _repo.Update(param);
                BaseReturnModel result = new BaseReturnModel();
                result.SetCode(new ReturnCode(insertResult.Code, insertResult.IsSuccess, insertResult.Msg));
                return result;
            }, param);
        }
    }
}
