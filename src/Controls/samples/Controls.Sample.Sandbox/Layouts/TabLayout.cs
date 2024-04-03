using System;
using Maui.Controls.Sample.Sandbox;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Layouts;

namespace Maui.Controls.Sample.Layouts;

/// <summary>
/// Layout to display tool tabs based on available space.
/// </summary>
public class TabLayout : HorizontalStackLayout
{
    public ToolTab? SelectedTab { get; internal set; }

    /// <inheritdoc/>
    protected override ILayoutManager CreateLayoutManager()
    {
        return new TabLayoutManager(this);
    }

    /// <inheritdoc/>
    public void Invalidate() => this.InvalidateMeasure();

	public void SetSelectedView(ToolTab toolTab)
	{
		if (SelectedTab is not null)
			SelectedTab.Selected = false;

		toolTab.Selected = true;
		SelectedTab = toolTab;
	}
}