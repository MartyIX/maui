using Microsoft.Maui.Controls;

namespace Maui.Controls.Sample;

public partial class WorkspaceTabHeader : ContentView
{
	public string Title { get; }

    public WorkspaceTabHeader(string title)
	{
		Title = title;
		InitializeComponent();
	}
}