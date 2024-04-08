using Maui.Controls.Sample.Sandbox;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Layouts;

namespace Maui.Controls.Sample.Layouts;

/// <summary>
/// Layout to display tool tabs based on available space.
/// </summary>
public class TabLayout : HorizontalStackLayout
{
	/// <summary>Tab that is currently selected, or <c>null</c> if none is selected at the moment.</summary>
	public ToolTab? SelectedTab { get; internal set; }

    /// <inheritdoc/>
    protected override ILayoutManager CreateLayoutManager()
    {
        return new TabLayoutManager(this);
    }

    /// <inheritdoc/>
    public void Invalidate() => this.InvalidateMeasure();

	/// <summary>
	/// Sets selected tool tab.
	/// </summary>
	/// <param name="toolTab">Tool tab that is currently selected.</param>
	public void SetSelected(ToolTab toolTab)
	{
		if (SelectedTab is not null)
			SelectedTab.Selected = false;

		toolTab.Selected = true;
		SelectedTab = toolTab;
	}
}