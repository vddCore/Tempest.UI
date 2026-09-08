namespace Tempest.UI.Infrastructure;

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

        Avalonia.Threading.Dispatcher.UIThread.Post(() =>
        {
            if (Application.Current is null) return;

            var res = Application.Current.Resources;

            res["Breeze::Shared.Accent"] = new SolidColorBrush(Color.FromArgb(255, red, green, blue));
            res["Breeze::Shared.Accent.100"] = new SolidColorBrush(Color.FromArgb(255, red, green, blue));
            res["Breeze::Shared.Accent.80"] = new SolidColorBrush(Color.FromArgb(204, red, green, blue));
            res["Breeze::Shared.Accent.60"] = new SolidColorBrush(Color.FromArgb(153, red, green, blue));
            res["Breeze::Shared.Accent.40"] = new SolidColorBrush(Color.FromArgb(102, red, green, blue));
            res["Breeze::Shared.Accent.20"] = new SolidColorBrush(Color.FromArgb(51, red, green, blue));
            res["Breeze::Shared.Accent.0"] = new SolidColorBrush(Color.FromArgb(0, red, green, blue));
            
            res["Breeze::Color.Shared.Accent"] = Color.FromArgb(255, red, green, blue);
            res["Breeze::Color.Shared.Accent.100"] = Color.FromArgb(255, red, green, blue);
            res["Breeze::Color.Shared.Accent.80"] = Color.FromArgb(204, red, green, blue);
            res["Breeze::Color.Shared.Accent.60"] = Color.FromArgb(153, red, green, blue);
            res["Breeze::Color.Shared.Accent.40"] = Color.FromArgb(102, red, green, blue);
            res["Breeze::Color.Shared.Accent.20"] = Color.FromArgb(51, red, green, blue);
            res["Breeze::Color.Shared.Accent.0"] = Color.FromArgb(0, red, green, blue);
        });
    }
}