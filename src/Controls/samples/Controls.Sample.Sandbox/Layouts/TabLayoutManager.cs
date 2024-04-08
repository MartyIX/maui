using System;
using System.Diagnostics;
using Microsoft.Maui;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Layouts;

namespace Maui.Controls.Sample.Layouts;

/// <summary>
/// Layout manager computing layout for tool tabs based on available space.
/// </summary>
/// <remarks>
/// The rules of the layout are:
/// <list type="bullet">
/// <item>If there is enough space, we display all tabs.</item>
/// <item>If there is not enough space, we display the active tab and neighbor tabs for which there is enough space.</item>
/// </list>
/// </remarks>
/// <seealso href="https://github.com/dotnet/maui/blob/main/docs/design/layout.md"/>
/// <seealso href="https://learn.microsoft.com/en-gb/dotnet/maui/user-interface/layouts/custom?view=net-maui-8.0"/>
public class TabLayoutManager : HorizontalStackLayoutManager
{
	/// <summary>Layout to display tool tabs based on available space.</summary>
	private readonly TabLayout layout;

	/// <summary>
	/// Creates a new instance of the object.
	/// </summary>
	/// <param name="tabLayout">Layout to display tool tabs based on available space.</param>
	public TabLayoutManager(TabLayout tabLayout) : base(tabLayout)
	{
		layout = tabLayout;
	}

	/// <inheritdoc/>
	public override Size Measure(double widthConstraint, double heightConstraint)
	{
		Debug.WriteLine($"[Measure][Start] {nameof(widthConstraint)}='{widthConstraint}',{nameof(heightConstraint)}='{heightConstraint}'");

		Thickness padding = layout.Padding;

		widthConstraint -= padding.HorizontalThickness;

		double currentRowHeight = 0;
		double toolTabsBarWidth = widthConstraint;
		double sumWidths = 0;

		for (int i = 0; i < layout.Count; i++)
		{
			var child = layout[i];

			if (child.Visibility == Visibility.Collapsed)
				continue;

			Size measure = child.Measure(double.PositiveInfinity, heightConstraint);
			sumWidths += measure.Width;
			currentRowHeight = Math.Max(currentRowHeight, measure.Height);
		}

		// Find start and end indices by starting with full range and moving the start index up or the end index down.
		int startIdx = 0;
		int endIdx = layout.Count - 1;

		if (sumWidths > toolTabsBarWidth)
		{
			int index = GetSelectedTabIndexOrZero();
			currentRowHeight = 0;

			while (sumWidths > toolTabsBarWidth)
			{
				if (index > startIdx)
				{
					IView child = layout[startIdx];
					startIdx++;

					if (child.Visibility == Visibility.Collapsed)
						continue;

					Size measure = child.Measure(double.PositiveInfinity, heightConstraint);
					sumWidths -= measure.Width;
					currentRowHeight = Math.Max(currentRowHeight, measure.Height);
					continue;
				}

				if (index < endIdx)
				{
					IView child = layout[endIdx];
					endIdx--;

					if (child.Visibility == Visibility.Collapsed)
						continue;

					Size measure = child.Measure(double.PositiveInfinity, heightConstraint);
					sumWidths -= measure.Width;
					currentRowHeight = Math.Max(currentRowHeight, measure.Height);
					continue;
				}

				// Only one tab - the one that is selected - remains, nothing to do.
				break;
			}
		}

		double currentRowWidth = sumWidths;
		
		// Account for padding.
		currentRowWidth += padding.HorizontalThickness;

		// Ensure that the total size of the layout fits within its constraints
		double finalWidth = ResolveConstraints(externalConstraint: widthConstraint, explicitLength: Stack.Width, measuredLength: currentRowWidth,
			min: Stack.MinimumWidth, max: Stack.MaximumWidth);
		double finalHeight = ResolveConstraints(externalConstraint: heightConstraint, explicitLength: Stack.Height, measuredLength: currentRowHeight,
			min: Stack.MinimumHeight, max: Stack.MaximumHeight);

		Size result = new(finalWidth, finalHeight);

		Debug.WriteLine($"[Measure][End] '{result}' [Stack.Width={Stack.Width},Stack.Height={Stack.Height}]");

		return result;
	}

	/// <inheritdoc/>
	public override Size ArrangeChildren(Rect bounds)
	{
		Debug.WriteLine($"[ArrangeChildren][Start] {nameof(bounds)}='{bounds}'");

		Thickness padding = Stack.Padding;
		double top = padding.Top + bounds.Top;
		double left = padding.Left + bounds.Left;

		double currentX = left;
		double currentRowHeight = 0;

		int layoutCount = layout.Count;

		int index = GetSelectedTabIndexOrZero();

		int startIdx = 0;
		int endIdx = layoutCount - 1;

		double toolTabsBarWidth = bounds.Width;
		double sumWidths = 0;

		for (int i = 0; i < layoutCount; i++)
			sumWidths += layout[i].DesiredSize.Width;

		while (sumWidths > toolTabsBarWidth)
		{
			if (index > startIdx)
			{
				IView child = layout[startIdx];
				startIdx++;

				if (child.Visibility == Visibility.Collapsed)
					continue;

				sumWidths -= child.DesiredSize.Width;
				continue;
			}

			if (index < endIdx)
			{
				IView child = layout[startIdx];
				endIdx--;

				if (child.Visibility == Visibility.Collapsed)
					continue;

				sumWidths -= child.DesiredSize.Width;
				continue;
			}

			// Only one tab - the one that is selected - remains, nothing to do.
			break;
		}

		for (int n = 0; n < layoutCount; n++)
		{
			IView child = layout[n];
			if (child.Visibility == Visibility.Collapsed)
				continue;

			if (n < startIdx || n > endIdx)
			{
				Debug.WriteLine($"[!] TOO LONG! Skipping child #{n}.");

				// Move it out of display.
				Rect destination = new(-1000, 0, child.DesiredSize.Width, child.DesiredSize.Height);
				_ = child.Arrange(destination);
			}
			else
			{
				Rect destination = new(currentX, top, child.DesiredSize.Width, child.DesiredSize.Height);
				Debug.WriteLine($"X Arrange child #{n} normally. Destination: {destination}");
				_ = child.Arrange(destination);

				currentX += destination.Width + layout.Spacing;
				currentRowHeight = Math.Max(currentRowHeight, destination.Height);
			}
		}

		Size actual = new(currentX, currentRowHeight);

		// Adjust the size if the layout is set to fill its container.
		Size result = actual.AdjustForFill(bounds, Stack);

		Debug.WriteLine($"[ArrangeChildren][End] '{result}'");
		return result;
	}

	/// <summary>
	/// Gets index of the selected tab in the layout's children list.
	/// </summary>
	/// <returns>Index of the selected tool tab, or <c>0</c> if no tab is selected.</returns>
	private int GetSelectedTabIndexOrZero()
	{
		int index = 0;

		if (layout.SelectedTab is not null)
		{
			int i = 0;
			foreach (IView view in layout.Children)
			{
				if (view is Microsoft.Maui.Controls.View viewWithContext && viewWithContext.BindingContext == layout.SelectedTab)
				{
					index = i;
					break;
				}

				i++;
			}
		}

		return index;
	}
}