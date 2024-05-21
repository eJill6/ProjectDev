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
    public class AdvertisingContentRepo : BaseInlodbRepository, IAdvertisingContentRepo
    {
        public AdvertisingContentRepo(IOptionsMonitor<MsSqlConnections> setting, IRequestIdentifierProvider provider, ILogger logger) : base(setting, provider, logger)
        {
        }

        public async Task<MMAdvertisingContent[]> Get()
        {
            return (await ReadDb.QueryTable<MMAdvertisingContent>()
                .QueryAsync()).ToArray();
        }

        public async Task<MMAdvertisingContent> Get(int id)
        {
            return (await ReadDb.QueryTable<MMAdvertisingContent>()
                .Where(x => x.Id == id).
                QueryAsync()).FirstOrDefault();
        }

        public async Task<bool> Update(MMAdvertisingContent param)
        {
            string sql = @"UPDATE [Inlodb].[dbo].[MMAdvertisingContent]
SET [ModifyDate] = GETDATE(),
[IsActive] = @IsActive,
[AdvertisingContent]=@AdvertisingContent
WHERE [Id] = @Id;";
            await WriteDb
               .AddExecuteSQL(sql, param).SaveChangesAsync();
            return true;
        }

        public async Task Delete(int id)
        {
            //string sql = @"DELETE FROM [Inlodb].[dbo].[MMAdvertisingContent] WHERE Id = @Id";
            //await WriteDb.AddExecuteSQL(sql, id).SaveChangesAsync();
            await WriteDb.Delete(new MMAdvertisingContent()
            {
                Id = id,
            }).SaveChangesAsync();
        }

        /// <summary>
        /// 取得贴子類型的宣傳文字
        /// </summary>
        /// <param name="contentType">宣傳類型</param>
        /// <returns></returns>
        public async Task<MMAdvertisingContent[]> GetByPostType(AdvertisingContentType contentType)
        {
            return (await ReadDb
                .QueryTable<MMAdvertisingContent>()
                .Where(x => x.ContentType == (int)contentType && x.IsActive == true).QueryAsync())?
                .ToArray() ?? Array.Empty<MMAdvertisingContent>();
        }

        /// <summary>
        /// 取得管理員聯繫方式
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetAdminContact()
        {
            return (await ReadDb
                .QueryTable<MMAdvertisingContent>()
                .Where(x => x.AdvertisingType == (int)AdvertisingType.PT_Account).QueryAsync())?
                .FirstOrDefault()?.AdvertisingContent ?? string.Empty;
        }

        /// <summary>
        /// 取得提示訊息
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetTip(PostType postType)
        {
            return (await ReadDb
                .QueryTable<MMAdvertisingContent>()
                .Where(x => x.AdvertisingType == (int)AdvertisingType.Tip && x.PostType == (int)postType).QueryAsync())?
                .FirstOrDefault()?.AdvertisingContent ?? string.Empty;
        }
    }
}