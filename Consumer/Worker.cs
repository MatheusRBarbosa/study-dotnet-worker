namespace QueueSimulator.Consumer;
using QueueSimulator.Domain.Interfaces.Infra.Services;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IRabbitService rabbitService;
    private readonly IRedisService redisService;

    public Worker(
        ILogger<Worker> logger,
        IRabbitService rabbitService,
        IRedisService redisService
    )
    {
        _logger = logger;
        this.rabbitService = rabbitService;
        this.redisService = redisService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var message = rabbitService.GetMessage();

            if (message != null && message != "")
            {
                Console.WriteLine($"[X] Message received: {message}");
                await redisService.DecreaseAll();
            }

            await Task.Delay(20000, stoppingToken);
        }

    }
}
