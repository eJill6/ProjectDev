using MS.Core.Infrastructures.DBTools;
using MS.Core.MM.Model.Entities.Media;
using MS.Core.MM.Model.Media;
using MS.Core.MMModel.Models.Media.Enums;
using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.Models;
using MS.Core.Models.Models;

namespace MS.Core.MM.Repos.interfaces
{
    public interface IMediaRepo
    {
        Task<string> CreateNewSEQID();

        Task<bool> Create(MMMedia entity);

        DapperComponent Create(DapperComponent component, MMMedia entity);

        Task<MMMedia> Get(string id);

        Task<MMMedia> GetFromWriteDb(string id);

        Task<MMMedia[]> Get(string[] id, bool isForce = false);

        DapperComponent Update(DapperComponent component, SaveMediaToOssParam entity);

        Task<bool> Update(SaveMediaToOssParam param);

        Task<bool> Delete(string param);

        DapperComponent Delete(DapperComponent component, string[] refIds);

        Task<MMMedia[]> Get(int mediaType, int sourceType, string[] refIds);

        Task<MMMedia[]> GetByIds(int mediaType, int sourceType, string[] ids);

        Task<string> CreateNewObjectName(string id, string fileName);

        Task<PageResultModel<MMMedia>> GetUnencrypt(MediaType type, SourceType sourceType, DateTime begin, DateTime end, int pageNo, int size);

        Task<DBResult> UpdateUrl(string v, string converted_path);
    }
}