using MS.Core.MM.Models.Entities.Post;
using MS.Core.MM.Models.Filters;

namespace MS.Core.MM.Repos.interfaces
{
    public interface IPostReportRepo
    {
        Task<IEnumerable<MMPostReport>> QueryByFilter(PostReportFilter filter);
    }
}