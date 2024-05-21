using JxBackendService.Interface.Repository;
using JxBackendService.Interface.Repository.Game;
using JxBackendService.Interface.Service.Enums;
using JxBackendService.Interface.Service.Game;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Paging;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Repository.Game;
using JxBackendService.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JxBackendService.Service.Game
{
    public class AllTPGameStatService : BaseService, IAllGameStatService
    {
        private readonly Lazy<IAllTPGameStatRep> _allTPGameStatRep;

        private readonly Lazy<IPlatformProductService> _platformProductService;

        private readonly Lazy<IUserInfoRep> _userInfoReadRep;

        public AllTPGameStatService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _platformProductService = ResolveKeyed<IPlatformProductService>(envLoginUser.Application);
            _allTPGameStatRep = ResolveJxBackendService<IAllTPGameStatRep>();
            _userInfoReadRep = ResolveJxBackendService<IUserInfoRep>(DbConnectionTypes.Slave);
        }

        public PagedResultWithAdditionalData<AllGamePlayInfoRowModel, TPGamePlayInfoStatModel> GetPagedAllPlayInfo(SearchAllPagedPlayInfoParam param)
        {
            PagedResultWithAdditionalData<AllGamePlayInfoRowModel, TPGamePlayInfoStatModel> pagedResult = _allTPGameStatRep
                .Value
                .GetPagedAllPlayInfo(param);

            List<int> userIds = pagedResult.ResultList.Select(s => s.UserId).Distinct().ToList();
            Dictionary<int, string> userIdMap = null;

            if (pagedResult.ResultList.Any())
            {
                userIdMap = _userInfoReadRep.Value.GetBaseBasicUserInfos(userIds).ToDictionary(d => d.UserID, d => d.UserName);
            }

            pagedResult.ResultList.ForEach(f =>
            {
                f.ProductName = _platformProductService.Value.GetName(f.ProductCode);

                if (userIds.Any() && userIdMap.TryGetValue(f.UserId, out string userName))
                {
                    f.UserName = userName;
                }
            });

            return pagedResult;
        }

        public List<TotalUserScore> GetTotalUserScores(int userId)
        {
            List<PlatformProduct> nonSelfPlatformProducts = _platformProductService.Value.GetNonSelfProduct();

            return _allTPGameStatRep.Value.GetTotalUserScores(nonSelfPlatformProducts, userId);
        }
    }
}