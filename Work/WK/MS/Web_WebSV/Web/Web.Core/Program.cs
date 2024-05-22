using Autofac;
using Autofac.Extensions.DependencyInjection;
using ControllerShareLib.Helpers;
using ControllerShareLib.Helpers.Cache;
using ControllerShareLib.Infrastructure.HostingService;
using ControllerShareLib.Infrastructure.Jobs.Setting;
using ControllerShareLib.Infrastructure.Jobs.Util;
using ControllerShareLib.Infrastructure.Middlewares;
using ControllerShareLib.Interfaces.Service;
using ControllerShareLib.Interfaces.Service.Setting;
using ControllerShareLib.Services;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.GlobalSystem;
using JxBackendService.Interface.Service.Threading;
using JxBackendService.Interface.Service.Util;
using JxBackendService.Interface.Service.Web;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.Route;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Service.Config;
using JxBackendServiceN6.Common.Extensions;
using JxBackendServiceN6.DependencyInjection;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.OpenApi.Models;
using NLog.Extensions.Logging;
using Quartz;
using System.Net;
using System.Reflection;
using Web.Core.Infrastructure.Filters;
using Web.Core.Infrastructure.Middlewares;

var builder = WebApplication.CreateBuilder(args);

string assemblyPath = AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar;

// 先建立前置的Container，為了下面註冊正常使用
var preBuilder = DependencyUtil.GetJxBackendServiceContainerBuilder(assemblyPath, new ContainerBuilder());
preBuilder = DependencyUtilN6.GetJxBackendServiceContainerBuilder(assemblyPath, preBuilder);
preBuilder.RegisterInstance(builder.Configuration).As<IConfiguration>();
RegisterLocalService(assemblyPath, preBuilder);
DependencyUtil.SetContainer(preBuilder.Build());

var environmentService = DependencyUtil.ResolveService<IEnvironmentService>().Value;

// Add services to the container.
builder.Services.AddControllersWithViews().AddJsonOptions(options =>
{
    //options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddControllers();
builder.Services.AddDistributedMemoryCache();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Web.Core API", Version = "v1" });
    options.OperationFilter<SwaggerHeaderFilter>();
    string[] xmlFileNames = { "Web.xml", "JxBackendService.xml" };

    foreach (string xmlFileName in xmlFileNames)
    {
        string xmlFilePath = Path.Combine(AppContext.BaseDirectory, xmlFileName);
        options.IncludeXmlComments(xmlFilePath);
    }
});

builder.Services.RegisterNetCore();

builder.Services.AddQuartz(q =>
{
    var controllerJobSettingService = DependencyUtil.ResolveKeyed<IControllerJobSettingService>(
        environmentService.Application,
        SharedAppSettings.PlatformMerchant).Value;

    List<ControllerJobSetting> controllerJobSettings = controllerJobSettingService.GetAll();

    q.AddQuartzJobs(controllerJobSettings);
});

builder.Services.AddQuartzHostedService(options =>
{
    // when shutting down we want jobs to complete gracefully
    options.WaitForJobsToComplete = true;
});

builder.Services.AddHostedService<KJEventService>();

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>(containerBuilder =>
    {
        containerBuilder = DependencyUtil.GetJxBackendServiceContainerBuilder(assemblyPath, containerBuilder);
        containerBuilder = DependencyUtilN6.GetJxBackendServiceContainerBuilder(assemblyPath, containerBuilder);
        RegisterLocalService(assemblyPath, containerBuilder);
        RegisterProductTeamLibraries(assemblyPath, containerBuilder);

        EnvironmentCode environmentCode = SharedAppSettings.GetEnvironmentCode();

        if (environmentCode == EnvironmentCode.Development)
        {
            //register mock services
            containerBuilder.RegisterType(typeof(WebSVServiceSettingMockService)).AsImplementedInterfaces();
        }
    });

WebApplication app = builder.Build();
DependencyUtil.SetContainer(app.Services.GetAutofacRoot());

EnvironmentCode environmentCode = SharedAppSettings.GetEnvironmentCode();
// Configure the HTTP request pipeline.
if (environmentCode.IsTestingEnvironment)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 設定全域錯誤處理中介軟體
app.UseExceptionHandler(appBuilder =>
{
#pragma warning disable CS1998 // Async 方法缺乏 'await' 運算子，將同步執行
    appBuilder.Run(async context =>
    {
        var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();

        if (exceptionHandlerFeature != null)
        {
            // 日誌記錄錯誤訊息
            var httpContextUserService = DependencyUtil.ResolveService<IHttpContextUserService>().Value;

            var environmentUser = new EnvironmentUser()
            {
                Application = environmentService.Application,
                LoginUser = httpContextUserService.GetBasicUserInfo()
            };

            var errorMsgUtilService = DependencyUtil.ResolveService<IErrorMsgUtilService>().Value;
            errorMsgUtilService.ErrorHandle(exceptionHandlerFeature.Error, environmentUser, SendErrorMsgTypes.Queue);

            Endpoint? endpoint = exceptionHandlerFeature?.Endpoint;
            bool hasApiControllerAttribute = endpoint.HasApiController();

            if (hasApiControllerAttribute || context.Request.IsAjaxRequest())
            {
                string responseJson = new BaseReturnModel(exceptionHandlerFeature.Error.Message).ToJsonString(isCamelCaseNaming: true);
                var byteArrayApiService = DependencyUtil.ResolveService<IByteArrayApiService>().Value;

                if (!byteArrayApiService.IsEncodingRequired(context.Request))
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = $"{HttpWebRequestContentType.Json}; charset=utf-8";

                    await context.Response.WriteAsync(responseJson);
                }
                else
                {
                    await byteArrayApiService.EncodeResponseAsync(context.Response, responseJson, context.Request.Headers);
                }

                return;
            }
        }

        context.Response.Redirect("/Public/Error");
    });
#pragma warning restore CS1998 // Async 方法缺乏 'await' 運算子，將同步執行
});

//app.UseHttpsRedirection();
var provider = new FileExtensionContentTypeProvider();
provider.Mappings.Add(".aes", "application/octet-stream");

app.UseStaticFiles(new StaticFileOptions()
{
    ContentTypeProvider = provider
});

app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<ByteArrayApiMiddleware>();
app.UseRouting();
app.UseMiddleware<ResponseHandlingMiddleware>();

foreach (RouteName routeName in RouteName.GetAll())
{
    app.MapControllerRoute(
        name: routeName.Value,
        pattern: routeName.Url,
        defaults: routeName.Defaults);
}

var logUtilService = DependencyUtil.ResolveService<ILogUtilService>().Value;
logUtilService.ForcedDebug($"{environmentService.Application}-Web 站台準備啟動");

var appSettingService = DependencyUtil.ResolveKeyed<IAppSettingService>(environmentService.Application, SharedAppSettings.PlatformMerchant).Value;
var threadPoolService = DependencyUtil.ResolveService<IThreadPoolService>().Value;
threadPoolService.SetMinThreads(appSettingService.MinWorkerThreads);

app.Run();

static void RegisterLocalService(string assemblyPath, ContainerBuilder containerBuilder)
{
    containerBuilder.RegisterInstance(new NLogLoggerFactory()).As<ILoggerFactory>().SingleInstance();
    containerBuilder.RegisterGeneric(typeof(Logger<>)).As(typeof(ILogger<>)).SingleInstance();

    containerBuilder.RegisterType<LocalMemoryCached>()
        .As<ICache>()
        .SingleInstance();

    containerBuilder.RegisterType<CacheBase>()
        .As<ICacheBase>()
        .SingleInstance();

    containerBuilder.RegisterType<DistributeMemcachedV2>()
        .As<ICacheRemote>()
        .SingleInstance();

    containerBuilder.RegisterType<CacheService>()
        .As<ICacheService>()
        .InstancePerLifetimeScope();

    containerBuilder.RegisterType<UserService>()
        .As<IUserService>()
        .InstancePerLifetimeScope();

    containerBuilder.RegisterType<PlayInfoService>()
        .As<IPlayInfoService>()
        .InstancePerLifetimeScope();

    containerBuilder.RegisterType<LotteryService>()
        .As<ILotteryService>()
        .InstancePerLifetimeScope();

    containerBuilder.RegisterType<LatestWinningService>();

    var webAssembly = Assembly.GetExecutingAssembly();
    RegisterContainerHelper.RegisterPlayConfig(webAssembly, containerBuilder);

    string[] dllFileNames = new string[] { "ControllerShareLib.dll", "Web.Core.dll" };
    RegisterContainerHelper.RegisterDllFiles(assemblyPath, dllFileNames, containerBuilder);
}

static void RegisterProductTeamLibraries(string assemblyPath, ContainerBuilder containerBuilder)
{
    Dictionary<string, List<string>> configMap = new Dictionary<string, List<string>>()
    {
        { "MS.Core.MMClient.dll", new List<string>() { "Service" } },
        { "JxLottery.Services.dll", new List<string>() { "Service" } },
        { "JxLottery.Adapters.dll", new List<string>() { "Adapter", "Service" } },
    };

    foreach (KeyValuePair<string, List<string>> item in configMap)
    {
        var fileName = Path.Combine(assemblyPath, item.Key);
        var assmbly = Assembly.LoadFrom(fileName);

        containerBuilder.RegisterAssemblyTypes(assmbly)
            .Where(t => item.Value.Any(value => t.Name.EndsWith(value)))
            .AsImplementedInterfaces()
            .InstancePerDependency();
    }
}