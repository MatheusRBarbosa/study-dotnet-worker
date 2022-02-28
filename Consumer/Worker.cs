namespace QueueSimulator.Consumer;
using QueueSimulator.Domain.Interfaces.Infra.Services;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IRabbitService rabbitService;

    public Worker(ILogger<Worker> logger, IRabbitService rabbitService)
    {
        _logger = logger;
        this.rabbitService = rabbitService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var message = rabbitService.GetMessage();

            if (message != null && message != "")
            {
                Console.WriteLine($"[X] Message received: {message}");
            }

            await Task.Delay(20000, stoppingToken);
        }

    }
}
