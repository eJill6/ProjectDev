using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.DynamicProxy;
using IdGen;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Chat;
using JxBackendService.Interface.Service.GlobalSystem;
using JxBackendService.Interface.Service.MiseLive;
using JxBackendService.Interface.Service.Threading;
using JxBackendService.Interface.Service.Util;
using JxBackendService.Interface.Service.Web;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Service.MiseLive;
using JxBackendServiceN6.Common.Extensions;
using JxBackendServiceN6.DependencyInjection;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.OpenApi.Models;
using PolyDataBase.Services;
using SLPolyGame.Web.Common;
using SLPolyGame.Web.Core.Filters;
using System.Net;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo { Title = "SLPolyGame.Web", Version = "v1" });
        options.OperationFilter<SwaggerHeaderFilter>();

        string xmlFileName = "Web.xml";
        string xmlFilePath = Path.Combine(AppContext.BaseDirectory, xmlFileName);
        options.IncludeXmlComments(xmlFilePath);
    });

builder.Services.RegisterNetCore();

string assemblyPath = AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar;

// 先建立前置的Container，為了下面註冊正常使用
var preBuilder = DependencyUtil.GetJxBackendServiceContainerBuilder(assemblyPath, new ContainerBuilder());
preBuilder = DependencyUtilN6.GetJxBackendServiceContainerBuilder(assemblyPath, preBuilder);
preBuilder.RegisterInstance(builder.Configuration).As<IConfiguration>();
RegisterLocalService(preBuilder);
DependencyUtil.SetContainer(preBuilder.Build());

var environmentService = DependencyUtil.ResolveService<IEnvironmentService>().Value;

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>(containerBuilder =>
    {
        containerBuilder = DependencyUtil.GetJxBackendServiceContainerBuilder(assemblyPath, containerBuilder);
        containerBuilder = DependencyUtilN6.GetJxBackendServiceContainerBuilder(assemblyPath, containerBuilder);
        RegisterPolyDataBase(containerBuilder);
        RegisterLocalService(containerBuilder);
        RegisterJxLottery(containerBuilder);

        EnvironmentCode environmentCode = SharedAppSettings.GetEnvironmentCode();

        if (environmentCode == EnvironmentCode.Development)
        {
            //register mock services
            //containerBuilder.RegisterType(typeof(MiseLiveApiMockService)).AsImplementedInterfaces();
            //containerBuilder.RegisterType(typeof(HttpWebRequestUtilLogCurlMockService)).AsImplementedInterfaces();
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

        if (exceptionHandlerFeature == null)
        {
            return;
        }

        // 日誌記錄錯誤訊息
        var environmentService = DependencyUtil.ResolveService<IEnvironmentService>().Value;
        var httpContextUserService = DependencyUtil.ResolveService<IHttpContextUserService>().Value;

        var environmentUser = new EnvironmentUser()
        {
            Application = environmentService.Application,
            LoginUser = httpContextUserService.GetBasicUserInfo()
        };

        var errorMsgUtilService = DependencyUtil.ResolveService<IErrorMsgUtilService>().Value;
        var idGeneratorService = DependencyUtil.ResolveJxBackendService<IIdGeneratorService>(environmentUser, DbConnectionTypes.Master).Value;
        long? errorId = null;

        if (idGeneratorService.TryCreateId(out long id))
        {
            errorId = id;
        }

        errorMsgUtilService.ErrorHandle(exceptionHandlerFeature.Error, environmentUser, SendErrorMsgTypes.Queue, errorId);

        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        context.Response.ContentType = $"{HttpWebRequestContentType.Json}; charset=utf-8";
        string responseJson = new BaseReturnModel(exceptionHandlerFeature.Error.Message).ToJsonString(isCamelCaseNaming: true);

        await context.Response.WriteAsync(responseJson);
    });
#pragma warning restore CS1998 // Async 方法缺乏 'await' 運算子，將同步執行
});

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Use((context, next) =>
{
    context.Request.EnableBuffering();

    return next();
});

var logUtilService = DependencyUtil.ResolveService<ILogUtilService>().Value;
logUtilService.ForcedDebug($"{environmentService.Application}-WebSV 站台準備啟動");

var appSettingService = DependencyUtil.ResolveKeyed<IAppSettingService>(environmentService.Application, SharedAppSettings.PlatformMerchant).Value;
var threadPoolService = DependencyUtil.ResolveService<IThreadPoolService>().Value;
threadPoolService.SetMinThreads(appSettingService.MinWorkerThreads);

DependencyUtil.ResolveJxBackendService<ITPLiveStreamService>(
    new EnvironmentUser() { Application = environmentService.Application, LoginUser = new BasicUserInfo() },
    DbConnectionTypes.Slave).Value.StartNewSyncCachedAnchorsJob();

app.Run();

static void RegisterPolyDataBase(ContainerBuilder containerBuilder)
{
    containerBuilder.RegisterInstance(new NLog.Extensions.Logging.NLogLoggerFactory()).As<ILoggerFactory>().SingleInstance();
    containerBuilder.RegisterGeneric(typeof(Logger<>)).As(typeof(ILogger<>)).SingleInstance();
    containerBuilder.RegisterType<CacheBase>()
        .AsImplementedInterfaces()
        .SingleInstance();

    containerBuilder.RegisterType<ZipService>()
        .As<IZipService>()
        .InstancePerLifetimeScope();

    containerBuilder.RegisterType<SLPolyGame.Web.DAL.PalyInfo>()
        .As<SLPolyGame.Web.DAL.PalyInfo>()
        .InstancePerLifetimeScope();

    containerBuilder.RegisterType<SLPolyGame.Web.BLL.PalyInfo>()
        .As<SLPolyGame.Web.BLL.PalyInfo>()
        .InstancePerLifetimeScope();

    containerBuilder.RegisterType<SLPolyGame.Web.BLL.SysSettings>()
        .As<SLPolyGame.Web.BLL.SysSettings>()
        .InstancePerLifetimeScope();

    containerBuilder.RegisterType<SLPolyGame.Web.BLL.CurrentLotteryInfo>()
        .As<SLPolyGame.Web.BLL.CurrentLotteryInfo>()
        .InstancePerLifetimeScope();
}

static void RegisterLocalService(ContainerBuilder containerBuilder)
{
    string assemblyPath = AppDomain.CurrentDomain.BaseDirectory;
    string[] dllFileNames = new string[] { "WebApiImpl.dll", "SLPolyGame.Web.Core.dll" };

    foreach (string dllFileName in dllFileNames)
    {
        DependencyUtil.GetServiceContainerBuilderFromAssemblyTypes(assemblyPath, dllFileName, containerBuilder, processTypes: null);
    }
}

static void RegisterJxLottery(ContainerBuilder containerBuilder)
{
    string assemblyPath = AppDomain.CurrentDomain.BaseDirectory;

    // Register JxLottery Service
    var serviceAssmbly = Assembly.LoadFrom(Path.Combine(assemblyPath, "JxLottery.Services.dll"));

    containerBuilder.RegisterAssemblyTypes(serviceAssmbly)
        .Where(t => t.Name.EndsWith("Service"))
        .AsImplementedInterfaces();

    // Register JxLottery Adapter
    var adapterAssmbly = Assembly.LoadFrom(Path.Combine(assemblyPath, "JxLottery.Adapters.dll"));

    containerBuilder.RegisterAssemblyTypes(adapterAssmbly)
        .Where(t => t.Name.EndsWith("Service") || t.Name.EndsWith("Adapter"))
        .AsImplementedInterfaces();
}