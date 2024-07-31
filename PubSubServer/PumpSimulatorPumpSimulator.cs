using PubSubServer.Node;

namespace PubSubServer;

public class PumpSimulator : BackgroundService
{
    private double _currentFlow;
    private double _currentHeat;
    private readonly Random _random = new Random();

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Task.Run(async () =>
        {
            _currentFlow = _random.Next(0, 100);
            _currentHeat = _random.Next(0, 50);
            
            while (!stoppingToken.IsCancellationRequested)
            {
                _currentFlow = SimulateValueChange(_currentFlow, 0, 100, 1, 5);
                _currentHeat = SimulateValueChange(_currentHeat, 0, 50, 0.5, 2);

                NodeManager.SetValue("Pump-Flow", _currentFlow);
                NodeManager.SetValue("Pump-Heat", _currentHeat);

                await Task.Delay(250, stoppingToken);
            }
        }, stoppingToken);
    }

    private double SimulateValueChange(double currentValue, double minValue, double maxValue, double minChange, double maxChange)
    {
        var change = _random.NextDouble() * (maxChange - minChange) + minChange;
        if (_random.Next(2) == 0)
        {
            change = -change;
        }

        var newValue = currentValue + change;

        if (newValue < minValue)
        {
            newValue = minValue;
        }
        else if (newValue > maxValue)
        {
            newValue = maxValue;
        }

        return newValue;
    }
}