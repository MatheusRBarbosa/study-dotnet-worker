using QueueSimulator.Consumer;
using QueueSimulator.Domain.Interfaces.Infra.Services;
using QueueSimulator.Infra.Services;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddSingleton<IRabbitService, RabbitService>();
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
