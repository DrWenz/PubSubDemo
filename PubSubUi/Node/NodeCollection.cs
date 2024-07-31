using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using PubSubUi.Models;

namespace PubSubUi.Node;

public class NodeCollection : IEnumerable, INotifyCollectionChanged
{
    private readonly Dictionary<string, NodeItem> _nodes;

    public NodeCollection(IEnumerable<NodeItem> nodes)
    {
        _nodes = nodes.ToDictionary(node => node.Name);
    }

    public int Count => _nodes.Count;

    public IEnumerator GetEnumerator()
    {
        return _nodes.Values.GetEnumerator();
    }

    public event NotifyCollectionChangedEventHandler? CollectionChanged;

    public void Add(NodeItem node)
    {
        _nodes[node.Name] = node;
        CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, node));
    }

    public void Remove(string name)
    {
        if (_nodes.Remove(name, out var existNode))
            CollectionChanged?.Invoke(this,
                new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, existNode));
    }

    public void Update(NodeItem node)
    {
        if (!_nodes.TryGetValue(node.Name, out var existNode)) return;
        existNode.Value = node.Value;
        existNode.LastUpdated = node.LastUpdated;
    }
}