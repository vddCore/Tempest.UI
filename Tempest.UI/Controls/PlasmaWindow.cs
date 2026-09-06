namespace Tempest.UI.Controls;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;

[TemplatePart("PART_ResizeLeft", typeof(Border))]
[TemplatePart("PART_ResizeTopLeft", typeof(Border))]
[TemplatePart("PART_ResizeTop", typeof(Border))]
[TemplatePart("PART_ResizeTopRight", typeof(Border))]
[TemplatePart("PART_ResizeRight", typeof(Border))]
[TemplatePart("PART_ResizeBottomRight", typeof(Border))]
[TemplatePart("PART_ResizeBottom", typeof(Border))]
[TemplatePart("PART_ResizeBottomLeft", typeof(Border))]
public class PlasmaWindow : Window
{
    protected override Type StyleKeyOverride => typeof(PlasmaWindow);

    static PlasmaWindow()
    {
        WindowDecorationsProperty.Changed.AddClassHandler<PlasmaWindow, WindowDecorations>(
            (s, e) => s.OnWindowDecorationsChanged(e)
        );
        
        CanResizeProperty.Changed.AddClassHandler<PlasmaWindow, bool>(
            (s, e) => s.OnCanResizeChanged(e)
        );
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        
        BindResizeGrip(e, "PART_ResizeLeft", WindowEdge.West);
        BindResizeGrip(e, "PART_ResizeTopLeft", WindowEdge.NorthWest);
        BindResizeGrip(e, "PART_ResizeTop", WindowEdge.North);
        BindResizeGrip(e, "PART_ResizeTopRight", WindowEdge.NorthEast);
        BindResizeGrip(e, "PART_ResizeRight", WindowEdge.East);
        BindResizeGrip(e, "PART_ResizeBottomRight", WindowEdge.SouthEast);
        BindResizeGrip(e, "PART_ResizeBottom", WindowEdge.South);
        BindResizeGrip(e, "PART_ResizeBottomLeft", WindowEdge.SouthWest);
    }

    private void BindResizeGrip(TemplateAppliedEventArgs e, string partName, WindowEdge edge)
    {
        var grip = e.NameScope.Find<Border>(partName);

        if (grip == null)
        {
            return;
        }

        grip.PointerPressed += (_, args) =>
        {
            if (args.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
            {
                if (CanResize && WindowState == WindowState.Normal)
                {
                    BeginResizeDrag(edge, args);
                }
            }
        };
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
    
    private void OnCanResizeChanged(AvaloniaPropertyChangedEventArgs<bool> e)
    {
        if (e.NewValue == false)
        {
            PseudoClasses.Add(":noresize");
        }
        else
        {
            PseudoClasses.Remove(":noresize");
        }
    }
}