using Microsoft.Maui.Controls;

namespace Maui.Controls.Sample.Sandbox;

public class ToolTab : BindableObject
{
	public string Title { get; }

	private bool selected;

	/// <inheritdoc cref="selected"/>
	public bool Selected
	{
		get => selected;
		set
		{
			selected = value;
			OnPropertyChanged();
		}
	}

	public ToolTab(string title, bool selected = false)
	{
		Title = title;
		this.selected = selected;
	}
}