namespace Tempest.UI;

using Avalonia;

public static partial class Extensions
{
    public static AppBuilder UseTempestUI(this AppBuilder builder)
    {
        builder.AfterSetup((_) => TempestUI.Initialize());
        return builder;
    }
}