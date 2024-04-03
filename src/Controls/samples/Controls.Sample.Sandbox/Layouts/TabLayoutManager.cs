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

		double currentRowWidth = 0;
		double currentRowHeight = 0;

		int layoutCount = layout.Count;

		for (int n = 0; n < layoutCount; n++)
		{
			IView child = layout[n];
			if (child.Visibility == Visibility.Collapsed)
			{
				continue;
			}

			Size measure = child.Measure(double.PositiveInfinity, heightConstraint);

			Debug.WriteLine($"[!] [#{n}] currentRowWidth={currentRowWidth}, childMeasureWidth='{measure.Width}', sum={currentRowWidth + measure.Width}");

			// Will adding this IView put us past the edge?
			if (currentRowWidth + measure.Width > widthConstraint)
			{
				Debug.WriteLine($"[!] TOO LONG! Skipping child #{n}.");
				break;
			}

			currentRowWidth += measure.Width;
			currentRowHeight = Math.Max(currentRowHeight, measure.Height);

			if (n < layoutCount - 1)
			{
				currentRowWidth += layout.Spacing;
			}
		}

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

		for (int n = 0; n < layoutCount; n++)
		{
			IView child = layout[n];
			if (child.Visibility == Visibility.Collapsed)
			{
				continue;
			}

			Debug.WriteLine($"[!] [#{n}] currentX={currentX}, child.DesiredSize.Width='{child.DesiredSize.Width}', sum={currentX + child.DesiredSize.Width}");

			if (currentX + child.DesiredSize.Width > bounds.Width)
			{
				Debug.WriteLine($"[!] TOO LONG! Skipping child #{n}.");

				// Move it out of display.
				Rect destination = new(-1000, 0, child.DesiredSize.Width, child.DesiredSize.Height);
				_ = child.Arrange(destination);
				break;
			}
			else
			{
				Debug.WriteLine($"X Arrange child #{n} normally.");
				Rect destination = new(currentX, top, child.DesiredSize.Width, child.DesiredSize.Height);
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
}