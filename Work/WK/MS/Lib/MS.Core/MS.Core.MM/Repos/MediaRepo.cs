using Dapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MS.Core.Extensions;
using MS.Core.Infrastructures.DBTools;
using MS.Core.Infrastructures.DBTools.Models;
using MS.Core.Infrastructures.Providers;
using MS.Core.MM.Model.Entities.Media;
using MS.Core.MM.Model.Media;
using MS.Core.MM.Repos.interfaces;
using MS.Core.MMModel.Models.Media.Enums;
using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.Models;
using MS.Core.Models.Models;
using MS.Core.Repos;
using System.Data;

namespace MS.Core.MM.Repos
{
    public class MediaRepo : BaseInlodbRepository, IMediaRepo
    {
        public MediaRepo(IOptionsMonitor<MsSqlConnections> setting, IRequestIdentifierProvider provider, ILogger logger) : base(setting, provider, logger)
        {
        }

        public async Task<bool> Create(MMMedia entity)
        {
            await Create(WriteDb, entity).SaveChangesAsync();
            return true;
        }

        public DapperComponent Create(DapperComponent component, MMMedia entity)
        {
            return component.Insert(entity);
        }

        public async Task<string> CreateNewObjectName(string id, string fileName)
        {
            var fileInfo = new FileInfo(fileName);

            return await Task.FromResult($"{id}-{DateTime.Now.ToUnixOfTime()}{fileInfo.Extension}");
        }

        public async Task<string> CreateNewSEQID()
        {
            return await WriteDb.GetSequenceIdentity("SEQ_MMMedia_Id");
        }

        public async Task<bool> Delete(string param)
        {
            await WriteDb.Delete(new MMMedia()
            {
                Id = param,
            }).SaveChangesAsync();
            return true;
        }

        public DapperComponent Delete(DapperComponent component, string[] refIds)
        {
            return component.Delete(refIds.Select(x => new MMMedia() { Id = x }));
        }

        public async Task<MMMedia> Get(string id)
        {
            return (await ReadDb.QueryTable<MMMedia>()
                .Where(x => x.Id == id)
                .QueryAsync()).FirstOrDefault();
        }

        public async Task<MMMedia> GetFromWriteDb(string id)
        {
            return (await WriteDb.QueryTable<MMMedia>()
                .Where(x => x.Id == id)
                .QueryAsync()).FirstOrDefault();
        }

        public async Task<MMMedia[]> Get(int mediaType, int sourceType, string[] refIds)
        {
            return (await ReadDb.QueryTable<MMMedia>()
                .Where(x => x.SourceType == sourceType
                    && x.MediaType == mediaType
                    && refIds.Contains(x.RefId))
                .QueryAsync())
                .ToArray();
        }
        public async Task<MMMedia[]> GetByIds(int mediaType, int sourceType, string[] ids)
        {
            return (await ReadDb.QueryTable<MMMedia>()
                .Where(x => x.SourceType == sourceType
                    && x.MediaType == mediaType
                    && ids.Contains(x.Id))
                .QueryAsync())
                .ToArray();
        }

        public async Task<PageResultModel<MMMedia>> GetUnencrypt(MediaType type, SourceType sourceType, DateTime begin, DateTime end, int pageNo, int size)
        {
            var source = (int)sourceType;
            var mediaType = (int)type;
            return (await ReadDb.QueryTable<MMMedia>()
                .Where(x => x.MediaType == mediaType &&
                        x.SourceType == source &&
                        x.CreateDate >= begin &&
                        x.CreateDate < end &&
                        x.RefId != string.Empty)
                .OrderByDescending(x => x.Id)
                .QueryPageResultAsync(new PaginationModel()
                {
                    PageNo = pageNo,
                    PageSize = size,
                }));
        }

        public DapperComponent Update(DapperComponent component, SaveMediaToOssParam entity)
        {
            return component.Update<MMMedia>(entity);
        }

        public async Task<bool> Update(SaveMediaToOssParam param)
        {
            await Update(WriteDb, param).SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// 取得media資料從id
        /// </summary>
        /// <param name="sourceType">來源</param>
        /// <param name="ids">MediaId</param>
        /// <returns></returns>
        public async Task<MMMedia[]> Get(string[] ids, bool isForce = false)
        {
            if (isForce)
            {
                return (await WriteDb.QueryTable<MMMedia>()
                    .Where(x => ids.Contains(x.Id))
                    .QueryAsync()).ToArray();
            }
            return (await ReadDb.QueryTable<MMMedia>()
                .Where(x => ids.Contains(x.Id))
                .QueryAsync()).ToArray();
        }

        /// <inheritdoc/>
        public async Task<DBResult> UpdateUrl(string id, string converted_path)
        {
            var data = (await WriteDb.QueryTable<MMMedia>()
                .Where(x => x.Id == id)
                .QueryAsync()).FirstOrDefault();

            if (data == null)
            {
                return new DBResult(ReturnCode.DataIsNotExist);
            }

            string sql = @"
                DECLARE @ErrorCode   VARCHAR(100)  = N'D99999';     -- 錯誤碼  
                DECLARE @ErrorMsg   NVARCHAR(100) = N'Run procedure fail'  -- 訊息  
                UPDATE [dbo].[MMMedia]
                SET FileUrl = @FileUrl,
                    ModifyDate = GETDATE()
                WHERE [Id] = @Id
                
                SELECT @ErrorCode = '000000',  
                        @ErrorMsg  = N'Ok';  

                SELECT @ErrorCode AS Code, @ErrorMsg AS Msg; ";
            var param = new DynamicParameters();
            param.Add("@Id", new DbString
            {
                Value = id,
                Length = 32,
                IsAnsi = true,
                IsFixedLength = false,
            });
            param.Add("@FileUrl", new DbString
            {
                Value = converted_path,
                Length = 250,
                IsAnsi = false,
                IsFixedLength = false,
            });

            var result = await WriteDb
                .QueryFirstAsync<DBResult>(sql, param);

            return result;
        }
    }
}