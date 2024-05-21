using MS.Core.MM.Models.Entities.PostTransaction;

namespace MS.Core.MM.Repos.interfaces
{
    public interface IVipTypeRepo
    {
        Task<MMVipType[]> GetAll();
    }
}
