using QueueSimulator.Domain.Models;
using System.Text.Json;

using QueueSimulator.Infra.Services;

namespace TeleprompterConsole;

internal class Program
{
    static async Task Main(string[] args)
    {
        if (args.Length == 0 || args[0].ToUpper().Equals("PUBLISH"))
        {
            var quantity = args.Length > 1 ? int.Parse(args[1]) : 5;
            for (int i = 0; i < quantity; i++)
            {
                await Publish();
            }
        }
        else if (args[0].ToUpper().Equals("GET"))
        {
            var id = args[1];
            await Get(id);
        }
    }

    static async Task Publish()
    {
        var rabbitService = new RabbitService();
        var redisService = new RedisService();

        var position = await redisService.GetTotal();
        var guid = System.Guid.NewGuid();
        await redisService.IncreaseCounter();
        await redisService.SetPosition(guid, position);

        var message = new Message
        {
            Name = "Matheus barbosa",
            Position = position,
            Id = guid
        };

        var stringMessage = JsonSerializer.Serialize(message);
        rabbitService.Send(stringMessage);
        Console.WriteLine("[X] Message sent!");
    }

    static async Task Get(string id)
    {
        var redisService = new RedisService();
        var guid = Guid.Parse(id);

        var position = await redisService.GetPosition(guid);
        Console.WriteLine($"[-] Sua posicao eh: {position.ToString()}");
    }
}