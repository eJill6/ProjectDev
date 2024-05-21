using Autofac;
using Autofac.Extensions.DependencyInjection;
using BackSideWeb.Helpers;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Threading;
using JxBackendService.Interface.Service.Util;
using JxBackendService.Interface.Service.Web.BackSideWeb;
using JxBackendService.Model.Common;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendServiceN6.Common.Extensions;
using JxBackendServiceN6.DependencyInjection;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;

IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("Settings/province.json").AddJsonFile("Settings/city.json").Build();

var builder = WebApplication.CreateBuilder(args);

string assemblyPath = AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar;

// 先建立前置的Container，為了下面註冊正常使用
var preBuilder = DependencyUtil.GetJxBackendServiceContainerBuilder(assemblyPath, new ContainerBuilder());
preBuilder = DependencyUtilN6.GetJxBackendServiceContainerBuilder(assemblyPath, preBuilder);
RegisterLocalService(preBuilder);
DependencyUtil.SetContainer(preBuilder.Build());

var environmentService = DependencyUtil.ResolveService<IEnvironmentService>().Value;

//builder.Services.Configure<List<CityInfo>>(builder.Configuration.GetSection("CityData"));
//builder.Services.Configure<List<ProvinceInfo>>(builder.Configuration.GetSection("ProvinceData"));
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Limits.MaxRequestBodySize = 50 * 1024 * 1024; // 50 MB
});

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSingleton(new AppHelper(configuration));

string authenticationScheme = environmentService.Application.AuthenticationScheme;

builder.Services.AddAuthentication(authenticationScheme).AddCookie(authenticationScheme, o =>
{
    o.LoginPath = new PathString("/Authority/Login");
});

builder.Services.RegisterNetCore();

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>(containerBuilder =>
    {
        containerBuilder = DependencyUtil.GetJxBackendServiceContainerBuilder(assemblyPath, containerBuilder);
        containerBuilder = DependencyUtilN6.GetJxBackendServiceContainerBuilder(assemblyPath, containerBuilder);
        RegisterLocalService(containerBuilder);
    });

WebApplication app = builder.Build();

DependencyUtil.SetContainer(app.Services.GetAutofacRoot());

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    app.UseHttpsRedirection();
}
else
{
    //do something in Development
}

// 設定全域錯誤處理寻芳阁軟體
app.UseExceptionHandler(appBuilder =>
{
#pragma warning disable CS1998 // Async 方法缺乏 'await' 運算子，將同步執行
    appBuilder.Run(async context =>
    {
        var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();

        if (exceptionHandlerFeature != null)
        {
            // 日誌記錄錯誤訊息
            var backSideWebUserService = DependencyUtil.ResolveService<IBackSideWebUserService>().Value;
            var environmentService = DependencyUtil.ResolveService<IEnvironmentService>().Value;

            var environmentUser = new EnvironmentUser()
            {
                Application = environmentService.Application,
                LoginUser = new BasicUserInfo()
                {
                    UserId = backSideWebUserService.GetUserId(),
                }
            };

            ErrorMsgUtil.ErrorHandle(exceptionHandlerFeature.Error, environmentUser);

            if (context.Request.IsAjaxRequest())
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = $"{HttpWebRequestContentType.Json}; charset=utf-8";
                string responseJson = new BaseReturnModel(exceptionHandlerFeature.Error.Message).ToJsonString(isCamelCaseNaming: true);

                await context.Response.WriteAsync(responseJson);

                return;
            }
        }

        context.Response.Redirect("/Public/Error");
    });
#pragma warning restore CS1998 // Async 方法缺乏 'await' 運算子，將同步執行
});

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}");

var logUtilService = DependencyUtil.ResolveService<ILogUtilService>().Value;
logUtilService.ForcedDebug($"{environmentService.Application} 站台準備啟動");

var appSettingService = DependencyUtil.ResolveKeyed<IAppSettingService>(environmentService.Application, SharedAppSettings.PlatformMerchant).Value;
var threadPoolService = DependencyUtil.ResolveService<IThreadPoolService>().Value;
threadPoolService.SetMinThreads(appSettingService.MinWorkerThreads);

app.Run();

static void RegisterLocalService(ContainerBuilder containerBuilder)
{
    string assemblyPath = AppDomain.CurrentDomain.BaseDirectory;
    string[] dllFileNames = new string[] { "BackSideWeb.dll" };

    foreach (string dllFileName in dllFileNames)
    {
        DependencyUtil.GetServiceContainerBuilderFromAssemblyTypes(assemblyPath, dllFileName, containerBuilder, processTypes: null);
    }
}