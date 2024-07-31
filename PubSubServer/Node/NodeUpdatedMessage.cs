using PubSubServer.Hub;

namespace PubSubServer.Node;

public class NodeUpdatedMessage : IHubMessage
{
    public string Action { get; } = "NoteUpdated";
    public object? Value { get; set; }
}