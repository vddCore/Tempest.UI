namespace Tempest.UI.Controls;

using Avalonia;
using Avalonia.Controls;

public class PlasmaWindow : Window
{
    protected override Type StyleKeyOverride => typeof(PlasmaWindow);

    static PlasmaWindow()
    {
        WindowDecorationsProperty.Changed.AddClassHandler<PlasmaWindow, WindowDecorations>(
            (s, e) => s.OnWindowDecorationsChanged(e)
        );
    }
    
    private void OnWindowDecorationsChanged(AvaloniaPropertyChangedEventArgs<WindowDecorations> e)
    {
        if (e.NewValue == WindowDecorations.BorderOnly)
        {
            PseudoClasses.Add(":borderonly");
        }
        else
        {
            PseudoClasses.Remove(":borderonly");
        }
    }
}