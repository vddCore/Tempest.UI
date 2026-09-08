namespace Tempest.UI.Example;

using System;
using Avalonia;
using Avalonia.OpenGL.Egl;
using Avalonia.Rendering.Composition;
using AvaloniaUI.DiagnosticsSupport;

internal class Program
{
    [STAThread]
    public static void Main(string[] args) => BuildAvaloniaApp()
        .StartWithClassicDesktopLifetime(args);

    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .UseSkia()
            .UseHarfBuzz()
#if DEBUG
            .WithDeveloperTools()
#endif
            .WithInterFont()
            .UseTempestUI()
            .LogToTrace();
}