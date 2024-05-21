using MS.Core.MM.Models.Entities.Post;

namespace MS.Core.MM.Repos.interfaces
{
    public interface IOfficialPostPriceRepo
    {
        Task<MMOfficialPostPrice> GetById(int id);
        Task<IEnumerable<MMOfficialPostPrice>> GetByPostId(string postId);
    }
}