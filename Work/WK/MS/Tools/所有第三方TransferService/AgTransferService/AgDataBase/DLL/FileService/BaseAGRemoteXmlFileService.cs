using AgDataBase.Common;
using AgDataBase.Model;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Enums.Product;
using JxBackendService.Interface.Service.ThirdPartyTransfer;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.ThirdParty;
using JxBackendService.Model.ViewModel;
using System;
using System.Collections.Concurrent;

namespace AgDataBase.DLL.FileService
{
    public class BaseAGRemoteXmlFileService
    {
        private readonly ConcurrentDictionary<RuntimeTypeHandle, ConcurrentDictionary<string, XmlContent>> _xmlFileMaps =
           new ConcurrentDictionary<RuntimeTypeHandle, ConcurrentDictionary<string, XmlContent>>();

        private readonly ConcurrentDictionary<RuntimeTypeHandle, string> _currentCrawlDateMap = new ConcurrentDictionary<RuntimeTypeHandle, string>();

        private static readonly EnvironmentUser s_envLoginUser = new EnvironmentUser
        {
            Application = JxApplication.AGTransferService,
            LoginUser = new BasicUserInfo { }
        };

        protected IBetLogFileService BetLogFileService { get; private set; }

        public IPlatformProductAGSettingService PlatformProductAGSettingService { get; }

        public IAgApi AGApi { get; }

        public BaseAGRemoteXmlFileService()
        {
            BetLogFileService = DependencyUtil.ResolveJxBackendService<IBetLogFileService>(s_envLoginUser, DbConnectionTypes.Slave);
            PlatformProductAGSettingService = DependencyUtil.ResolveKeyed<IPlatformProductAGSettingService>(SharedAppSettings.PlatformMerchant);
            AGApi = DependencyUtil.ResolveService<IAgApi>();
        }

        protected string CurrentCrawlDate
        {
            get
            {
                RuntimeTypeHandle typeHandle = GetType().TypeHandle;

                if (_currentCrawlDateMap.TryGetValue(typeHandle, out string currentCrawlDate))
                {
                    return currentCrawlDate;
                }

                return null;
            }
        }

        protected void UpdateCurrentCrawlDate(string newCrawlDate)
        {
            RuntimeTypeHandle typeHandle = GetType().TypeHandle;
            _currentCrawlDateMap[typeHandle] = newCrawlDate;
        }

        protected ConcurrentDictionary<string, XmlContent> XmlFileMap
        {
            get
            {
                RuntimeTypeHandle typeHandle = GetType().TypeHandle;

                if (_xmlFileMaps.TryGetValue(typeHandle, out ConcurrentDictionary<string, XmlContent> xmlFileMap))
                {
                    return xmlFileMap;
                }

                _xmlFileMaps[typeHandle] = new ConcurrentDictionary<string, XmlContent>();

                return _xmlFileMaps[typeHandle];
            }
        }

        protected string GetXmlMapKey(AGGameType agGameType, string xmlFileName)
        {
            return $"{agGameType.Value}_{xmlFileName}";
        }
    }
}