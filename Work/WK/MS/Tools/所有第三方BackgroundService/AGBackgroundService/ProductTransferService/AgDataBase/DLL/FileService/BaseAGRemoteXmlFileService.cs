using ProductTransferService.AgDataBase.Model;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Enums.Product;
using JxBackendService.Interface.Service.ThirdPartyTransfer;
using JxBackendService.Interface.Service.Util;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.ThirdParty;
using JxBackendService.Model.ViewModel;
using ProductTransferService.AgDataBase.Common;
using System.Collections.Concurrent;

namespace ProductTransferService.AgDataBase.DLL.FileService
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

        private readonly Lazy<IBetLogFileService> _betLogFileService;

        private readonly Lazy<IPlatformProductAGSettingService> _platformProductAGSettingService;

        private readonly Lazy<IAgApi> _agApi;

        private readonly Lazy<ILogUtilService> _logUtilService;

        protected IBetLogFileService BetLogFileService => _betLogFileService.Value;

        public IPlatformProductAGSettingService PlatformProductAGSettingService => _platformProductAGSettingService.Value;

        public IAgApi AGApi => _agApi.Value;

        protected ILogUtilService LogUtilService => _logUtilService.Value;

        public BaseAGRemoteXmlFileService()
        {
            _betLogFileService = DependencyUtil.ResolveJxBackendService<IBetLogFileService>(s_envLoginUser, DbConnectionTypes.Slave);
            _platformProductAGSettingService = DependencyUtil.ResolveKeyed<IPlatformProductAGSettingService>(SharedAppSettings.PlatformMerchant);
            _agApi = DependencyUtil.ResolveService<IAgApi>();
            _logUtilService = DependencyUtil.ResolveService<ILogUtilService>();
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