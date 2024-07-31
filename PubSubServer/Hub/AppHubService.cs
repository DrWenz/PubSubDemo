using Microsoft.AspNetCore.SignalR;

namespace PubSubServer.Hub;

public sealed class AppHubService : IAppHubService
{
    private readonly IHubContext<AppHub> _hubContext;

    public AppHubService(IHubContext<AppHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public Task SendMessage(IHubMessage message)
    {
        return _hubContext.Clients.All.SendCoreAsync( message.Action,  new object?[] { new { message.Value} });
    }
}