using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MS.Core.Infrastructures.DBTools.Models;
using MS.Core.Infrastructures.Providers;
using MS.Core.MM.Models.Entities.Post;
using MS.Core.MM.Repos.interfaces;
using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.Repos;

namespace MS.Core.MM.Repos
{
    public class PageRedirectRepo : BaseInlodbRepository, IPageRedirectRepo
    {
        public PageRedirectRepo(IOptionsMonitor<MsSqlConnections> setting, IRequestIdentifierProvider provider, ILogger logger) : base(setting, provider, logger)
        {
        }

        public async Task<MMPageRedirect[]> Get()
        {
            return (await ReadDb.QueryTable<MMPageRedirect>()
                .QueryAsync()).ToArray();
        }

        public async Task<MMPageRedirect> Get(int id)
        {
            return (await ReadDb.QueryTable<MMPageRedirect>()
                .Where(x => x.Id == id).
                QueryAsync()).FirstOrDefault();
        }

        public async Task<bool> Update(MMPageRedirect param)
        {
            string sql = @"UPDATE [Inlodb].[dbo].[MMPageRedirect]
                            SET
                            [Title] = @Title,
                            [Type]  = @Type,
                            [RefId] = @RefId,
                            [UpdateDate] = GETDATE(),
                            [CreateUser] = @CreateUser
                            WHERE [Id] = @Id;";
            await WriteDb
               .AddExecuteSQL(sql, param).SaveChangesAsync();
            return true;
        }
    }
}