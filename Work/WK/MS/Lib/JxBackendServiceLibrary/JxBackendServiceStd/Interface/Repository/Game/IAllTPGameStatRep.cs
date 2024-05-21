using JxBackendService.Model.Enums;
using JxBackendService.Model.Paging;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Repository.Game;
using System.Collections.Generic;

namespace JxBackendService.Interface.Repository.Game
{
    public interface IAllTPGameStatRep
    {
        PagedResultWithAdditionalData<AllGamePlayInfoRowModel, TPGamePlayInfoStatModel> GetPagedAllPlayInfo(SearchAllPagedPlayInfoParam searchParam);

        List<TotalUserScore> GetTotalUserScores(List<PlatformProduct> platformProducts, int userId);
    }
}