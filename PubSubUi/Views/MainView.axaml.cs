using Avalonia.Controls;
using PubSubUi.ViewModels;

namespace PubSubUi.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        this.DataContext = new MainViewModel();
        InitializeComponent();
    }
}