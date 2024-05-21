using Autofac;
using MMService.Services;
using MS.Core.Infrastructure.Redis;
using MS.Core.Infrastructures.Providers;
using System.Runtime.Loader;

namespace MMService.Infrastructure
{
    public class AutofacModule : Autofac.Module
    {

        /// <summary>
        /// 資料庫後贅詞
        /// </summary>
        public static readonly string SuffixRepository = "Repo";

        /// <summary>
        /// 服務後贅詞
        /// </summary>
        public static readonly string SuffixService = "Service";

        /// <summary>
        /// 工具
        /// </summary>
        public static readonly string SuffixProvider = "Provider";

        /// <summary>
        /// 忽略的元件
        /// </summary>
        public readonly IDictionary<string, IList<string>> _ignoreClass = null;

        /// <summary>
        /// 設定檔
        /// </summary>
        private readonly Dictionary<string, List<string>> _config = new Dictionary<string, List<string>>()
        {
            { "MS.Core.dll", new List<string>() { SuffixRepository, SuffixService , SuffixProvider } },
            { "MS.Core.MM.dll", new List<string>() { SuffixRepository, SuffixService , SuffixProvider } },
            { "MMService.dll", new List<string>() { SuffixRepository, SuffixService , SuffixProvider } },
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="AutofacModule"/> class.
        /// </summary>
        public AutofacModule() : this(new Dictionary<string, IList<string>>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JxLotteryAutofacModule"/> class.
        /// </summary>
        public AutofacModule(IDictionary<string, IList<string>> ignoreClass)
        {
            _ignoreClass = ignoreClass;

            if (!_ignoreClass.ContainsKey(SuffixService))
            {
                _ignoreClass[SuffixService] = new List<string>();
            }
            if (!_ignoreClass.ContainsKey(SuffixProvider))
            {
                _ignoreClass[SuffixProvider] = new List<string>();
            }
            _ignoreClass[SuffixService].Add(nameof(RedisService));
            _ignoreClass[SuffixService].Add(nameof(CityService));
            _ignoreClass[SuffixProvider].Add(nameof(DateTimeProvider));
        }

        /// <summary>
        /// 實作注入的動作
        /// </summary>
        /// <param name="builder">autofac的builder</param>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(Logger<>))
                   .As(typeof(ILogger<>))
                   .SingleInstance();

            builder.Register(x =>
            {
                var factory = x.Resolve<ILoggerFactory>();
                return factory.CreateLogger(x.GetType());
            }).As<ILogger>();

            foreach (var item in _config)
            {
                var assmbly = AssemblyLoadContext.Default.LoadFromAssemblyPath(AppDomain.CurrentDomain.BaseDirectory + item.Key);
                foreach (var value in item.Value)
                {
                    var list = new List<string>();
                    if (_ignoreClass.ContainsKey(value))
                    {
                        list = _ignoreClass[value] as List<string>;
                    }

                    builder.RegisterAssemblyTypes(assmbly)
                        .Where(t => t.Name.EndsWith(value) && !list.Contains(t.Name))
                        .AsImplementedInterfaces()
                        .InstancePerDependency();
                }
            }

            // 註冊Singleton的服務
            builder.RegisterType<RedisService>()
                .AsImplementedInterfaces()
                .SingleInstance();
            builder.RegisterType<CityService>()
                .AsImplementedInterfaces()
                .SingleInstance();
            builder.RegisterType<DateTimeProvider>()
               .AsImplementedInterfaces()
               .SingleInstance();
        }
    }
}
