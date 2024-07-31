using System.Collections.Concurrent;
using PubSubServer.Hub;

namespace PubSubServer.Node;

public static class NodeManager
{
    private static IAppHubService? _appHubService;
    private static readonly ConcurrentDictionary<string, NodeItem> Nodes = new();

    public delegate void NodeUpdatedEventHandler(NodeUpdatedEventArgs e);
    public static event NodeUpdatedEventHandler? NodeUpdated;
    public static void Init(IAppHubService appHubService)
    {
        _appHubService = appHubService;
    }

    public static IEnumerable<NodeItem> Get()
    {
        return Nodes.Values;
    }

    public static NodeItem? Get(string name)
    {
        Nodes.TryGetValue(name, out var node);
        return node;
    }

    public static void SetValue(string name, object value)
    {
        var updatedNode = Nodes.AddOrUpdate(name, 
            new NodeItem()
            {
                Name = name,
                Value = value,
                LastUpdated = DateTime.Now
            }, 
            (_, existingNode) => 
            {
                existingNode.Value = value;
                existingNode.LastUpdated = DateTime.Now;
                return existingNode;
            });
        NotifyNodeUpdated(updatedNode);
    }
    
    private static void NotifyNodeUpdated(NodeItem node)
    {
        NodeUpdated?.Invoke(new NodeUpdatedEventArgs(node));
        _appHubService?.SendMessage(new NodeUpdatedMessage
        {
            Value = node
        });
    }
}