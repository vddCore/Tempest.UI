namespace Tempest.UI.Infrastructure;

using System.Diagnostics;
using Avalonia;
using Avalonia.Media;

internal static class PlasmaValueWatchers
{
    public static void AccentColor(string value)
    {
        var colorValues = value.Split(',');
        var red = byte.Parse(colorValues[0]);
        var green = byte.Parse(colorValues[1]);
        var blue = byte.Parse(colorValues[2]);

        Application.Current!.Resources["Breeze_Shared_AccentColor"] = new SolidColorBrush(
            Color.FromArgb(255, red, green, blue)
        );
        
        Debug.WriteLine($"Accent color set to {value}");
    }
}