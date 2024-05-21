using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MMService.Helpers;
using MS.Core.Attributes;
using MS.Core.Infrastructures.DBTools;
using MS.Core.Infrastructures.DBTools.Models;
using MS.Core.Infrastructures.Providers;
using MS.Core.Models;
using System.Reflection;

namespace MS.Core.Repos
{
    public abstract class BaseRepository
    {
        protected DapperComponent ReadDb { get; }
        protected DapperComponent WriteDb { get; }

        protected IRequestIdentifierProvider Provider { get; }
        protected ILogger Logger { get; }

        public BaseRepository(IOptionsMonitor<MsSqlConnections> setting, string readConn, string writeConn
            , IRequestIdentifierProvider provider, ILogger logger)
        {
            ReadDb = new DapperComponent(logger, provider, DecryptDesHelper.DecryptDES(setting.CurrentValue.Connections[readConn]));
            WriteDb = new DapperComponent(logger, provider, DecryptDesHelper.DecryptDES(setting.CurrentValue.Connections[writeConn]));
            Logger = logger;
            Provider = provider;
        }

        public async Task<string> GetSequenceIdentity(string sequenceName)
        {
            return await WriteDb.GetSequenceIdentity(sequenceName);
        }

        public virtual async Task<string> GetSequenceIdentity<T>() where T : BaseDBModel
        {
            PropertyInfo? pkProperty = typeof(T).GetProperties().SingleOrDefault(p => Attribute.IsDefined(p, typeof(PrimaryKeyAttribute)));

            if(pkProperty == null) 
            {
                throw new NotImplementedException("Not Impl PrimaryKey Attribute.");
            }

            string typeName = typeof(T).Name.Replace("Model", string.Empty).Replace("Entity", string.Empty);
            string sequenceName = $"SEQ_{typeName}_{pkProperty.Name}";
            return await WriteDb.GetSequenceIdentity(sequenceName);
        }
    }
}
