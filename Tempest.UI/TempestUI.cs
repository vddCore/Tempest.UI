namespace Tempest.UI;

using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Tempest.UI.Infrastructure;
using Tempest.UI.Infrastructure.Plasma;

public class TempestUI : ResourceDictionary
{
    private PlasmaGlobalConfigWatcher? _globalConfigWatcher;
    
    public TempestUI()
    {        
        AvaloniaXamlLoader.Load(this);
        
        InitializeConfigWatcher();
    }

    private void InitializeConfigWatcher()
    {
        _globalConfigWatcher = new PlasmaGlobalConfigWatcher(
            Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                ".config",
                "kdeglobals"
            )
        );
        
        _globalConfigWatcher.WatchValue("General", "AccentColor", PlasmaValueWatchers.AccentColor);
    }
}