namespace PubSubServer.Hub;

public interface IHubMessage
{
    string Action { get; }
    object? Value { get; set; }
}