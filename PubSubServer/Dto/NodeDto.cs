namespace PubSubServer.Dto;

public class NodeDto
{
    public string Name { get; set; }
    public object? Value { get; set; }
    public DateTime LastUpdated { get; set; }
}