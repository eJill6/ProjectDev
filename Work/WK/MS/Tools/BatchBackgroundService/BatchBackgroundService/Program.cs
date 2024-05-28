using BatchService;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<BatchMainService>();
    })
    .Build();

await host.RunAsync();