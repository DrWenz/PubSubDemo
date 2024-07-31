using System;
using System.Linq;
using Avalonia;

namespace PubSubUi;

internal sealed class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static int Main(string[] args)
    {
        var app = BuildAvaloniaApp();
        app.AfterSetup(AfterSetupCallback);
        
        if(args.Contains("--drm"))
            return app.StartLinuxDrm(args, null, true);
        if(args.Contains("--fb"))
            return app.StartLinuxFbDev(args);
        
        return app.StartWithClassicDesktopLifetime(args);
    }

    private static async void AfterSetupCallback(AppBuilder builder)
    {
        await App.NodeManager.InitializeAsync();
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
    {
        return AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
    }
}