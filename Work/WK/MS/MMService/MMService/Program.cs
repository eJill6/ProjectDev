using Autofac;
using Autofac.Extensions.DependencyInjection;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using MMService.Attributes;
using MMService.Infrastructure;
using MMService.Models.Auth;
using MMService.Models.Settings;
using MMService.Services;
using MMService.Validators;
using MS.Core.Infrastructure.OSS.Model.Param.OSS;
using MS.Core.Infrastructure.Redis.Models.Settings.Redis;
using MS.Core.Infrastructures.DBTools.Models;
using MS.Core.Infrastructures.MQPublisher;
using MS.Core.Infrastructures.ZeroOne.Models;
using MS.Core.MM.Infrastructures.Extensions;
using MS.Core.Models;
using NLog;
using NLog.Web;
using System.Reflection;
using System.Text.Json;

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
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    // 加入設定檔
    builder.Configuration.AddJsonFile($"Settings/CitiesData.json", optional: false, reloadOnChange: false);
    builder.Configuration.AddJsonFile($"Settings/MsSqlConnections{env}.json", optional: false, reloadOnChange: true);
    builder.Configuration.AddJsonFile($"Settings/RedisConnections{env}.json", optional: false, reloadOnChange: true);
    builder.Configuration.AddJsonFile($"Settings/OssSettings{env}.json", optional: false, reloadOnChange: true);
    builder.Configuration.AddJsonFile($"Settings/ZeroOne{env}.json", optional: false, reloadOnChange: true);
    builder.Configuration.AddJsonFile($"Settings/MQConnections{env}.json", optional: false, reloadOnChange: true);

    builder.WebHost.ConfigureKestrel(serverOptions =>
    {
        serverOptions.Limits.MaxRequestBodySize = 50 * 1024 * 1024; // 50 MB
    });
    builder.Services.AddMemoryCache();
    builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressMapClientErrors = true;
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
    });

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo { Title = "MMService", Version = "v1" });
        options.AddJwtBearerTokenUI();
        options.AddIncludeXmlComments(new[] { $"{Assembly.GetExecutingAssembly().GetName().Name}.xml" });
    });

    builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
    builder.Services.AddValidatorsFromAssemblyContaining(typeof(IFluentValidation));
    builder.Services.Configure<ApiBehaviorOptions>(options =>
    {
        options.InvalidModelStateResponseFactory = (context) =>
        {
            var errors = context.ModelState
                .Values
                .SelectMany(x => x.Errors
                            .Select(p => p.ErrorMessage))
                .ToList();

            var result = new BaseReturnModel(ReturnCode.OperationFailed, string.Join("\n", errors));
            return new OkObjectResult(new BaseReturnModel(result));
        };
    });

    builder.Services.AddJwtBearerTokenAuthentication(
        builder.Configuration.GetValue<string>("JwtSettings:Issuer"),
        builder.Configuration.GetValue<string>("JwtSettings:SignKey"));

    builder.Services.Configure<ZeroOneSettings>(builder.Configuration.GetSection("ZeroOne"));

    builder.Services.AddAutoMapper(typeof(AutofacModule).Assembly);
    builder.Host.UseNLog();
    builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

    builder.Services.Configure<CitiesData>(builder.Configuration.GetSection("CitiesData"));
    builder.Services.Configure<RedisConnections>(builder.Configuration.GetSection("RedisConnections"));
    builder.Services.Configure<OssSettings>(builder.Configuration.GetSection("OssSettings"));
    builder.Services.Configure<MsSqlConnections>(builder.Configuration.GetSection("MsSqlConnections"));
    builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
    builder.Services.Configure<MQSettings>(builder.Configuration.GetSection("MQConnections"));

    builder.Services.AddAutoMapper(typeof(AutofacModule).Assembly);

    builder.Host.ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule(new AutofacModule()));

    var app = builder.Build();
    app.UseMiddleware<ErrorHandlingMiddleware>();
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseCors();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();
    app.UseHttpsRedirection();
    app.UseForwardedHeaders(new ForwardedHeadersOptions
    {
        ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
    });

    // 預載
    var service = app.Services.GetService<ICityService>();

    ThreadPool.GetMinThreads(out var workerThreads, out var completionPortThreads);
    logger.Error($"Get Origin MinThreads Result, workerThreads={workerThreads},completionPortThreads={completionPortThreads}");
    ThreadPool.SetMinThreads(200, completionPortThreads);
    ThreadPool.GetMinThreads(out workerThreads, out completionPortThreads);
    logger.Error($"Get Update MinThreads Result, workerThreads={workerThreads},completionPortThreads={completionPortThreads}");

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