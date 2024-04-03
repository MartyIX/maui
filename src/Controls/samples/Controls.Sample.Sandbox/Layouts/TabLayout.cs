using Microsoft.Maui.Layouts;

namespace Maui.Controls.Sample.Layouts;

/// <summary>
/// Layout to display tool tabs based on available space.
/// </summary>
public class TabLayout : Microsoft.Maui.Controls.HorizontalStackLayout
{
    public double WindowWidth { get; internal set; }
    public double WindowHeight { get; internal set; }

    /// <inheritdoc/>
    protected override ILayoutManager CreateLayoutManager()
    {
        return new TabLayoutManager(this);
    }

    /// <inheritdoc/>
    public void Invalidate() => this.InvalidateMeasure();
}