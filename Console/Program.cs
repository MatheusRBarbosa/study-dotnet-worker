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
            Name = RandomName(),
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

        var position = (await redisService.GetPosition(guid)) + 1;
        Console.WriteLine($"[-] Sua posicao eh: {position.ToString()}");
    }

    static string RandomName()
    {
        var names = new List<string>
        {
            "Matheus",
            "Lucas",
            "Pedro",
            "Joao",
            "Paulo",
            "Marcos",
            "Joaquim",
            "Jorge",
            "Ana",
            "Maria",
            "Lucia",
            "Alice",
            "Fernanda",
            "Beatriz",
            "Paula",
            "Paulo"
        };

        var lastNames = new List<string>
        {
            "Barbosa",
            "Silva",
            "Santos",
            "Souza",
            "Oliveira",
            "Pereira",
            "Rodrigues",
            "Almeida",
            "Costa",
            "Lima"
        };

        var random = new Random();
        var name = random.Next(names.Count);
        var lastName = random.Next(lastNames.Count);

        return $"{names[name]} {lastNames[lastName]}";
    }
}