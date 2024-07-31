using System;
using PubSubUi.Models;

namespace PubSubUi.Node;

public class NodeUpdatedEventArgs : EventArgs
{
    public NodeItem Node { get; }

    public NodeUpdatedEventArgs(NodeItem node)
    {
        Node = node;
    }
}