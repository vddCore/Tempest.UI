namespace Tempest.UI.Example;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Tempest.UI.Controls;

public partial class MainWindow : PlasmaWindow
{
    public static readonly StyledProperty<Dock> CurrentStripPlacementProperty 
        = AvaloniaProperty.Register<MainWindow, Dock>(nameof(CurrentStripPlacement));

    public Dock CurrentStripPlacement
    {
        get => GetValue(CurrentStripPlacementProperty);
        set => SetValue(CurrentStripPlacementProperty, value);
    }
    
    public MainWindow()
    {
        InitializeComponent();
    }

    public void SetTabStripPlacement(object? placement)
    {
        Dispatcher.Invoke(() =>
        {
            var oldValue = CurrentStripPlacement;
            CurrentStripPlacement = (Dock)placement;
            
            OnPropertyChanged(new AvaloniaPropertyChangedEventArgs<Dock>(
                this, 
                CurrentStripPlacementProperty,
                oldValue,
                CurrentStripPlacement,
                BindingPriority.LocalValue
            ));
        });



    }
}