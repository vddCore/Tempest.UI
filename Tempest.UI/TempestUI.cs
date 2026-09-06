namespace Tempest.UI;

using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.Styling;
using Avalonia.Styling;
using Tempest.UI.Infrastructure;
using Tempest.UI.Infrastructure.Plasma;

public class TempestUI : ResourceDictionary
{
    private static PlasmaConfigWatcher? _globalConfigWatcher;

    public TempestVariant Variant
    {
        get;
        set
        {
            field = value;
            SelectTheme(value);
        }
    }

    public TempestUI()
    {        
        AvaloniaXamlLoader.Load(this);
        SelectTheme(Variant);
    }

    internal static void Initialize()
    {
        InitializeConfigWatcher();
    }

    private void SelectTheme(TempestVariant variant)
    {
        var themePrefix = variant switch
        {
            TempestVariant.Classic => "Colors.Classic",
            TempestVariant.Modern => "Colors.Modern",
            _ => throw new ArgumentOutOfRangeException(nameof(variant), variant, "Unrecognized theme.")
        };

        
        var darkUri = new Uri($"avares://Tempest.UI/Assets/{themePrefix}.Dark.axaml");
        var lightUri = new Uri($"avares://Tempest.UI/Assets/{themePrefix}.Light.axaml");
        
        var darkInclude = new ResourceInclude(darkUri) { Source = darkUri };
        var lightInclude = new ResourceInclude(lightUri) { Source = lightUri };

        ThemeDictionaries.Remove(ThemeVariant.Dark);
        ThemeDictionaries.Remove(ThemeVariant.Light);
        
        ThemeDictionaries.Add(ThemeVariant.Dark, darkInclude);
        ThemeDictionaries.Add(ThemeVariant.Light, lightInclude);
    }

    private static void InitializeConfigWatcher()
    {
        _globalConfigWatcher = new PlasmaConfigWatcher(
            Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                ".config",
                "kdeglobals"
            )
        );
        
        _globalConfigWatcher.WatchValue(
            "General", 
            "AccentColor",
            PlasmaValueWatchers.AccentColor
        );
    }
}