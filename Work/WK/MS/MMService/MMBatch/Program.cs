using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MMBatch.Infrastructure;
using MMBatch.Infrastructure.HostedService;
using MMService.Infrastructure;
using MS.Core.Infrastructure.OSS.Model.Param.OSS;
using MS.Core.Infrastructure.Redis.Models.Settings.Redis;
using MS.Core.Infrastructures.DBTools.Models;
using MS.Core.Infrastructures.MQPublisher;
using MS.Core.Infrastructures.ZeroOne.Models;
using NLog;
using NLog.Web;
using System.Reflection;

var logger = LogManager.GetCurrentClassLogger();
logger.Error($"程序啟動。Version：{Assembly.GetEntryAssembly()?.GetName().Version}");
var env = Environment.GetEnvironmentVariable("DEVELOPMENT_ENVIRONMENT") ?? string.Empty;
if (!string.IsNullOrWhiteSpace(env))
{
    env = string.Concat(".", env);
}
else
{
    env = string.Empty;
}

try
{
    var builder = Host.CreateDefaultBuilder(args);

    // Add services to the container.
    // 加入設定檔
    builder.ConfigureAppConfiguration((hostingContext, config) =>
    {
        config.AddJsonFile($"Settings/MsSqlConnections{env}.json", optional: false, reloadOnChange: true);
        config.AddJsonFile($"Settings/RedisConnections{env}.json", optional: false, reloadOnChange: true);
        config.AddJsonFile($"Settings/OssSettings{env}.json", optional: false, reloadOnChange: true);
        config.AddJsonFile($"Settings/ZeroOne{env}.json", optional: false, reloadOnChange: true);
        config.AddJsonFile($"Settings/MQConnections{env}.json", optional: false, reloadOnChange: true);
    });

    builder.ConfigureServices((hostContext, services) =>
    {
        services.AddAutofac();
        services.AddMemoryCache();
        services.Configure<RedisConnections>(hostContext.Configuration.GetSection("RedisConnections"));
        services.Configure<OssSettings>(hostContext.Configuration.GetSection("OssSettings"));
        services.Configure<MsSqlConnections>(hostContext.Configuration.GetSection("MsSqlConnections"));
        services.Configure<ZeroOneSettings>(hostContext.Configuration.GetSection("ZeroOne"));
        services.Configure<MQSettings>(hostContext.Configuration.GetSection("MQConnections"));
        services.AddHostedService<VideoEventService>();
        services.AddSchedule();

    });





    builder.UseServiceProviderFactory(new AutofacServiceProviderFactory());

    builder.ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule(new AutofacModule()));

    builder.UseNLog();
    var app = builder.Build();
    app.Run();
}
catch (Exception ex)
{
    // NLog: catch setup errors
    logger.Error(ex, "程序中止...");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    LogManager.Shutdown();
}