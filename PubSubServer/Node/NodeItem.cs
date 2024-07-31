namespace PubSubServer.Node;

public class NodeItem
{
    public required string Name { get; set; }
    public object? Value { get; set; }
    public DateTime LastUpdated { get; set; }
}