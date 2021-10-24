using JxBackendService.Interface.Repository;
using JxBackendService.Model.Entity;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;

namespace JxBackendService.Repository
{
    public class BlockChainInfoRep : BaseDbRepository<BlockChainInfo>, IBlockChainInfoRep
    {
        public BlockChainInfoRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType) { }

        public bool HasActiveUsdtAccount(int userId)
        {
            string sql = $@"
SELECT  COUNT(0)
FROM    {InlodbType.Inlodb.Value}.dbo.BlockChainInfo WITH(NOLOCK)
WHERE   IsActive = 1 AND UserID = @userId
";
            return DbHelper.QueryFirstOrDefault<int>(sql, new { userId }) > 0;
        }
    }
}
