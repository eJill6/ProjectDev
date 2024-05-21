using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Castle.Core.Internal;
using JxBackendService.DependencyInjection;
using JxBackendServiceNF.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Web.App_Start;
using Web.Helpers;
using Web.Helpers.Cache;
using Web.Helpers.Security;
using Web.Infrastructure;
using Web.PublicApiService;
using Web.SerTabService;
using Web.Services;
using Web.Services.WebSV.WCF;
using Web.Services.WebSV.WebApi;
using Web.SlotApiService;
using Web.SLPolyGameService;
using Web.ThirdPartyApiService;

namespace Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private bool useMainService = true;

        private static readonly object s_obj = new object();

        private string serviceUrl = System.Configuration.ConfigurationManager.AppSettings["ServiceUrl"];

        private string bakupServiceUrl = System.Configuration.ConfigurationManager.AppSettings["BackupServiceUrl"];

        private string appName = ConfigurationManager.AppSettings["AppName"].Trim();

        private System.Threading.Timer timerforCheckService;

        private ILogger<MvcApplication> _logger = null;

        /// <summary>
        /// 設定檔
        /// </summary>
        private readonly Dictionary<string, List<string>> _config = new Dictionary<string, List<string>>()
        {
            { "MS.Core.MMClient.dll", new List<string>() { "Service" } },
            { "JxLottery.Services.dll", new List<string>() { "Service" } },
            { "JxLottery.Adapters.dll", new List<string>() { "Adapter", "Service" } },
        };

        private ContainerBuilder builder = null;

        private IContainer container = null;

        private LatestWinningService _latestWinningService = null;

        private DistributeMemcached _distributeMemcached = null;

        protected void Application_Start()
        {
            string assemblyPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin\\");

            builder = new ContainerBuilder();

            var webAssembly = Assembly.GetExecutingAssembly();

            builder.RegisterInstance(new NLog.Extensions.Logging.NLogLoggerFactory()).As<ILoggerFactory>().SingleInstance();
            builder.RegisterGeneric(typeof(Logger<>)).As(typeof(ILogger<>)).SingleInstance();

            builder.RegisterInstance(ServiceProxy.PublicApiServiceProxy)
                .As<IPublicApiService>();

            builder.RegisterInstance(ServiceProxy.SLPolyGameServiceProxy)
                .As<ISLPolyGameService>();

            builder.RegisterInstance(ServiceProxy.ThirdPartyApiServiceProxy)
                .As<IThirdPartyApiWCFService>();

            builder.RegisterInstance(ServiceProxy.SerTabServiceProxy)
                .As<ISerTabService>();

            builder.RegisterInstance(ServiceProxy.SlotApiServiceProxy)
                .As<ISlotApiService>();

            builder.RegisterType<LocalMemoryCached>()
                .As<ICache>()
                .SingleInstance();

            builder.RegisterType<DistributeMemcached>()
                .As<ICacheRemote>()
                .SingleInstance();

            builder.RegisterType<CacheService>()
                .As<ICacheService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<UserService>()
                .As<IUserService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<PlayInfoService>()
                .As<IPlayInfoService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<LotteryService>()
                .As<ILotteryService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<LatestWinningService>();

            builder.RegisterAssemblyTypes(webAssembly)
                .Where(t => t.Name.EndsWith("Adapter")
                    || t.Name.EndsWith("Service"))
                .AsImplementedInterfaces()
                .InstancePerDependency();

            builder.RegisterAssemblyTypes(webAssembly)
                .Where(t => t.Name.EndsWith("PlayConfig") || t.Name.EndsWith("SpaTrendHelper"))
                .AsImplementedInterfaces()
                .SingleInstance();

            foreach (var item in _config)
            {
                var fileName = System.IO.Path.Combine(assemblyPath, item.Key);
                var assmbly = Assembly.LoadFrom(fileName);

                builder.RegisterAssemblyTypes(assmbly)
                    .Where(t => item.Value.Any(value => t.Name.EndsWith(value)))
                    .AsImplementedInterfaces()
                    .InstancePerDependency();
            }

            // Register your Web API controllers.
            builder.RegisterApiControllers(webAssembly);
            builder.RegisterWebApiFilterProvider(GlobalConfiguration.Configuration);

            builder.RegisterControllers(webAssembly);
            builder = DependencyUtil.GetJxBackendServiceContainerBuilder(assemblyPath, builder);
            builder = DependencyUtilNF.GetJxBackendServiceContainerBuilder(assemblyPath, builder);

            //打WCF的服務(WebSV成功轉移.Net Core後可移除)
            builder.RegisterType<SLPolyGameWCFService>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<SerTabWCFService>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<ThirdPartyApiWCFService>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<SlotApiWCFService>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<PublicApiWCFService>().AsImplementedInterfaces().SingleInstance();

            //打WebApi的服務(WebSV轉移失敗時先註解可拿到上面打WCF的服務)
            if (!ConfigurationManager.AppSettings["CoreServiceUrl"].IsNullOrEmpty())
            {
                builder.RegisterType<SLPolyGameWebApiService>().AsImplementedInterfaces().SingleInstance();
                builder.RegisterType<SerTabWebApiService>().AsImplementedInterfaces().SingleInstance();
                builder.RegisterType<ThirdPartyApiWebApiService>().AsImplementedInterfaces().SingleInstance();
                builder.RegisterType<SlotApiWebApiService>().AsImplementedInterfaces().SingleInstance();
                builder.RegisterType<PublicApiWebApiService>().AsImplementedInterfaces().SingleInstance();
            }

            container = builder.Build();

            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            DependencyUtil.SetContainer(container);

            _logger = container.Resolve<ILogger<MvcApplication>>();

            _latestWinningService = container.Resolve<LatestWinningService>();
            _latestWinningService.Init();

            _distributeMemcached = container.Resolve<ICacheRemote>() as DistributeMemcached;

            // 調整MaxJsonLength 為 int.MaxValue;
            ValueProviderFactories.Factories.Remove(ValueProviderFactories.Factories.OfType<JsonValueProviderFactory>().FirstOrDefault());
            ValueProviderFactories.Factories.Add(new CustomJsonValueProviderFactory());

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            RegisterGlobalConfig();
        }

        private static void RegisterGlobalConfig()
        {
            GlobalCacheHelper.StompServiceUrl = ConfigurationManager.AppSettings["StompServiceUrl"];
        }
    }
}