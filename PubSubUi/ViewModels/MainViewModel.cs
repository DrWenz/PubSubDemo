using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using PubSubUi.Models;
using PubSubUi.Node;

namespace PubSubUi.ViewModels;

public class MainViewModel : ViewModelBase
{
    private readonly HttpClient _httpClient = new();
    private NodeCollection _nodes = new([]);

    public MainViewModel()
    {
        App.NodeManager.NodeUpdated += NodeManagerOnNodeUpdated;
        LoadInitialValues();
    }

    public NodeCollection Nodes
    {
        get => _nodes;
        set => SetProperty(ref _nodes, value);
    }

    private void NodeManagerOnNodeUpdated(object sender, NodeUpdatedEventArgs e)
    {
        if (Nodes.Count == 0)
            return;
        
        Nodes.Update(e.Node);
    }

    public async void LoadInitialValues()
    {
        var nodes = await _httpClient.GetFromJsonAsync<List<NodeItem>>("http://localhost:5201/api/v1/node");
        Nodes = new NodeCollection(nodes ?? []);
    }
}