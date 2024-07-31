namespace PubSubServer.Node;

public class NodeUpdatedEventArgs : EventArgs
{
    public NodeItem Value { get; set; }

    public NodeUpdatedEventArgs(NodeItem value)
    {
        Value = value;
    }
}