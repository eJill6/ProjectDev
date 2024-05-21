using MS.Core.Attributes;
using MS.Core.MM.Models.Entities.PostTransaction;

namespace MS.Core.MM.Repos.interfaces
{
    public interface IVipRepo
    {
        Task<MMVip[]> GetVipsAsync();

        Task<MMVip[]> GetListedVipsAsync();

        Task<MMVip?> GetVipAsync(int vipId);

        Task<string> GetSequenceIdentity();
    }
}
