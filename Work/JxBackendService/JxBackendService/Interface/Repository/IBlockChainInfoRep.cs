using JxBackendService.Model.Entity;

namespace JxBackendService.Interface.Repository
{
    public interface IBlockChainInfoRep : IBaseDbRepository<BlockChainInfo>
    {
        bool HasActiveUsdtAccount(int userId);
    }
}
