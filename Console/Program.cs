using System.Reflection;

using QueueSimulator.Domain.Interfaces.Infra.Services;
using QueueSimulator.Domain.Models;

using QueueSimulator.Infra.Services;

namespace TeleprompterConsole;

internal class Program
{
    static void Main(string[] args)
    {
        var rabbitService = new RabbitService();

        var message = new Message
        {
            FromId = 1,
            ToId = 2,
            Content = "Hello World!"
        };

        rabbitService.Send(message);
        Console.WriteLine("[X] Message sent!");
    }
}