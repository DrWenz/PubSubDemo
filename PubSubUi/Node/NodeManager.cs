using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Avalonia.Threading;
using Microsoft.AspNetCore.SignalR.Client;
using PubSubUi.Models;

namespace PubSubUi.Node;

public class NodeManager : IAsyncDisposable
{
    private const string HubUrl = "http://localhost:5201/api/v1/hub";
    private HubConnection? _connection;

    public delegate void NodeUpdateEventHandler(object sender, NodeUpdatedEventArgs e);

    public event NodeUpdateEventHandler? NodeUpdated;
    
    public async ValueTask DisposeAsync()
    {
        if (_connection != null) await _connection.DisposeAsync();
    }

    public async Task InitializeAsync()
    {
        _connection = new HubConnectionBuilder()
            .WithUrl(HubUrl)
            .WithAutomaticReconnect()
            .Build();

        _connection.On<object>("NoteUpdated",
            nodeObject =>
            {
                if (nodeObject is JsonElement jsonElement)
                {
                    var valueObject= jsonElement.GetProperty("value");
                    valueObject.TryGetProperty("name", out var name);
                    valueObject.TryGetProperty("value", out var value);
                    valueObject.TryGetProperty("lastUpdated", out var lastUpdated);
                    var nodeItem = new NodeItem
                    {
                        Name = name.GetString(),
                        Value = GetValue(value),
                        LastUpdated = lastUpdated.GetDateTime()
                    };
                    Dispatcher.UIThread.Invoke(() =>
                    {
                        NodeUpdated?.Invoke(this, new NodeUpdatedEventArgs(nodeItem));
                    });
                }
            });

        _connection.Closed += exception =>
        {
            Console.WriteLine($"Connection closed: {exception}");
            return Task.CompletedTask;
        };

        await _connection.StartAsync();
    }

    private object? GetValue(JsonElement element)
    {
        return element.ValueKind switch
        {
            JsonValueKind.String => element.GetString(),
            JsonValueKind.Undefined => null,
            JsonValueKind.Object => GetObject(element),
            JsonValueKind.Array => GetArray(element),
            JsonValueKind.Number => element.TryGetInt32(out int intValue) ? intValue : element.GetDouble(),
            JsonValueKind.True => element.GetBoolean(),
            JsonValueKind.False => element.GetBoolean(),
            JsonValueKind.Null => null,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private Dictionary<string, object?> GetObject(JsonElement element)
    {
        var dict = new Dictionary<string, object?>();
        foreach (var property in element.EnumerateObject())
        {
            dict[property.Name] = GetValue(property.Value);
        }
        return dict;
    }

    private List<object?> GetArray(JsonElement element)
    {
        var list = new List<object?>();
        foreach (var item in element.EnumerateArray())
        {
            list.Add(GetValue(item));
        }
        return list;
    }
}