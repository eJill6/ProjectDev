using JxBackendService.Model.Paging;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Repository.Game;
using System.Collections.Generic;

namespace JxBackendService.Interface.Service.Game
{
    public interface IAllGameStatService
    {
        PagedResultWithAdditionalData<AllGamePlayInfoRowModel, TPGamePlayInfoStatModel> GetPagedAllPlayInfo(SearchAllPagedPlayInfoParam param);

        List<TotalUserScore> GetTotalUserScores(int userId);
    }
}