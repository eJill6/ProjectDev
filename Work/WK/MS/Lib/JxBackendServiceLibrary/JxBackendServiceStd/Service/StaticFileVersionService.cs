using System;
using System.Collections.Generic;
using System.IO;
using JxBackendService.Common.Extensions;
using System.Linq;
using JxBackendService.Common.Util.Cache;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Common.Util;
using JxBackendService.Model.Common;
using JxBackendService.Interface.Service.Util;
using JxBackendService.Model.ViewModel.Web;

namespace JxBackendService.Service
{
    public class StaticFileVersionService : IStaticFileVersionService
    {
        private readonly Lazy<IEnvironmentService> _environmentService;

        private readonly Lazy<IJxCacheService> _jxCacheService;

        private readonly Lazy<ILogUtilService> _logUtilService;

        public StaticFileVersionService()
        {
            _environmentService = DependencyUtil.ResolveService<IEnvironmentService>();
            _jxCacheService = DependencyUtil.ResolveService<IJxCacheService>();
            _logUtilService = DependencyUtil.ResolveService<ILogUtilService>();
        }

        public string GetStaticFileVersion()
        {
            string staticContentVersion = SharedAppSettings.StaticContentVersion;

            if (!staticContentVersion.IsNullOrEmpty())
            {
                return staticContentVersion;
            }

            StaticFileVersionInfo staticFileVersionInfo = GetStaticFileVersionInfo();

            if (staticFileVersionInfo != null)
            {
                return staticFileVersionInfo.Version;
            }

            return Guid.NewGuid().ToString();
        }

        public void InitStaticFileVersionInfo(params StaticDirectoryInfo[] staticDirectoryInfos)
        {
            List<KeyValuePair<string, long>> allPairs = new List<KeyValuePair<string, long>>();

            foreach (StaticDirectoryInfo staticDirectoryInfo in staticDirectoryInfos)
            {
                if (!Directory.Exists(staticDirectoryInfo.FullName))
                {
                    return;
                }

                List<FileInfo> fileInfos = Directory.GetFiles(staticDirectoryInfo.FullName, "*.*", SearchOption.AllDirectories)
                    .Select(s => new FileInfo(s))
                    .ToList();

                allPairs.AddRange(fileInfos.ToDictionary(
                    d => staticDirectoryInfo.PublishPrefix + d.FullName.Replace(staticDirectoryInfo.FullName, string.Empty),
                    d => d.Length));
            }

            _logUtilService.Value.ForcedDebug($"allFileInfoMap count={allPairs.Count()}");

            string fileInfosJson = allPairs.OrderBy(o => o.Key).ToJsonString(isFormattingNone: true);
            _logUtilService.Value.ForcedDebug($"fileInfosJson={fileInfosJson}");

            string fileInfosHash = HashExtension.SHA256(fileInfosJson);
            _logUtilService.Value.ForcedDebug($"fileInfosHash={fileInfosHash}");

            JxApplication application = _environmentService.Value.Application;

            CacheKey lockKey = CacheKey.StaticFileVersionLock(application);

            _jxCacheService.Value.DoWorkWithRemoteLock(lockKey, () =>
            {
                CacheKey versionKey = CacheKey.StaticFileVersion(application);
                StaticFileVersionInfo staticFileVersionInfo = GetStaticFileVersionInfo();

                if (fileInfosHash != staticFileVersionInfo?.Hash)
                {
                    if (staticFileVersionInfo == null)
                    {
                        staticFileVersionInfo = new StaticFileVersionInfo();
                    }

                    staticFileVersionInfo.Hash = fileInfosHash;
                    staticFileVersionInfo.Version = Guid.NewGuid().ToString();

                    _jxCacheService.Value.SetCache(new SetCacheParam()
                    {
                        Key = versionKey,
                        CacheSeconds = int.MaxValue,
                    }, staticFileVersionInfo);

                    _logUtilService.Value.ForcedDebug($"{GetType().Name}, VersionResult:{staticFileVersionInfo.ToJsonString()}");
                }
            });
        }

        private StaticFileVersionInfo GetStaticFileVersionInfo()
        {
            CacheKey versionKey = CacheKey.StaticFileVersion(_environmentService.Value.Application);

            var searchCacheParam = new SearchCacheParam
            {
                Key = versionKey,
                CacheSeconds = int.MaxValue,
                IsSlidingExpiration = true
            };

            return _jxCacheService.Value.GetCache<StaticFileVersionInfo>(versionKey);
        }
    }
}