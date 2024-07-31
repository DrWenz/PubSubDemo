using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace PubSubUi.Models;

public class NodeItem : ObservableObject
{
    private DateTime _lastUpdated;
    private object? _value;

    public string Name { get; set; }

    public object? Value
    {
        get => _value;
        set => SetProperty(ref _value, value);
    }

    public DateTime LastUpdated
    {
        get => _lastUpdated;
        set => SetProperty(ref _lastUpdated, value);
    }
}