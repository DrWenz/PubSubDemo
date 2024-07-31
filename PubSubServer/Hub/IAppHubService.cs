namespace PubSubServer.Hub;

public interface IAppHubService
{
    Task SendMessage(IHubMessage message);
}