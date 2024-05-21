using MS.Core.MM.Models.Entities.Post;
using MS.Core.MMModel.Models.Post.Enums;

namespace MS.Core.MM.Repos.interfaces
{
    public interface IPageRedirectRepo
    {
        Task<MMPageRedirect[]> Get();

        Task<MMPageRedirect> Get(int id);

        Task<bool> Update(MMPageRedirect param);
    }
}