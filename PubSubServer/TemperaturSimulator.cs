using PubSubServer.Node;

namespace PubSubServer;

public class TemperaturSimulator : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Task.Run(async () =>
        {
            var random = new Random();
            while (!stoppingToken.IsCancellationRequested)
            {
                NodeManager.SetValue("Temperature", 21.0 + (random.Next(0, 100) / 100.0));
                await Task.Delay(1500, stoppingToken);
            }
        }, stoppingToken);
    }
}