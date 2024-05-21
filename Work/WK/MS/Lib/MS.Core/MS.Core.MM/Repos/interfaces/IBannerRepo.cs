using MS.Core.MM.Model.Banner;
using MS.Core.MM.Model.Entities.Banner;

namespace MS.Core.MM.Repos.interfaces
{
    /// <summary>
    /// 儲存Banner的Repo
    /// </summary>
    public interface IBannerRepo
    {
        Task<MMBanner[]> Get();

        Task<MMBanner> Get(string id);

        Task<string> CreateNewSEQID();

        Task<bool> Create(SaveBannerParam param);

        Task<bool> HasDuplicateSort(SaveBannerParam param);

        Task<bool> Update(SaveBannerParam param);
        Task Delete(string seqId, string[] strings);
    }
}