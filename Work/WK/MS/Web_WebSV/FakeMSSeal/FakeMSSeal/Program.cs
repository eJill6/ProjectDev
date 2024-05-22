using FakeMSSeal.Middleware;
using FakeMSSeal.Mock;
using FakeMSSeal.Models;
using MS.Core.Infrastructures.ZeroOne.Models.Responses;
using MS.Core.Infrastructures.ZoneOne;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureAppConfiguration((hostingContext, config) =>
{
    config.AddJsonFile(
        $"Settings/Anchor.json", optional: false, reloadOnChange: true);
    config.AddJsonFile(
        $"Settings/Bot.json", optional: false, reloadOnChange: true);
    config.AddJsonFile(
        $"Settings/UserInfos.json", optional: false, reloadOnChange: true);
    config.AddJsonFile(
        $"Settings/LuoznzUrl.json", optional: false, reloadOnChange: true);
});

builder.Services.AddOptions();
builder.Services.Configure<AnchorResult>(builder.Configuration.GetSection("AnchorResult"));
builder.Services.Configure<RobotResult>(builder.Configuration.GetSection("RobotResult"));
builder.Services.Configure<List<ZOUserInfoRes>>(builder.Configuration.GetSection("UserInfos"));
builder.Services.Configure<LuoznzUrlModel>(builder.Configuration.GetSection("LuoznzUrl"));

builder.Logging.ClearProviders();
builder.Host.UseNLog();
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IBalanceMockServcie, MockBalanceServcie>();

var app = builder.Build();

app.UseCustomLogger();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();