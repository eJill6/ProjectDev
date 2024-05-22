using ProductTransferService;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<ProductTransferScheduleService>();
    })
    .Build();

await host.RunAsync();