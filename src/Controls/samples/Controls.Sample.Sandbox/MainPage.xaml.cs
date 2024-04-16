using System.Collections.ObjectModel;
using Maui.Controls.Sample.Sandbox;
using Microsoft.Maui.Controls;

namespace Maui.Controls.Sample;

public partial class MainPage : ContentPage
{
	/// <summary>Full list of workspace tabs on this dashboard.</summary>
	public ObservableCollection<WorkspaceTab> WorkspaceTabs { get; }


	public MainPage()
	{
		BindingContext = this;
		WorkspaceTabs = new();

		WorkspaceTabHeader header1 = new();
		header1.Title = "#1";
		WorkspaceTabs.Add(new WorkspaceTab(header1));

		WorkspaceTabHeader header2 = new();
		header2.Title = "#2";

		WorkspaceTabs.Add(new WorkspaceTab(header2));

		InitializeComponent();
	}

	private void Button_Clicked(object sender, System.EventArgs e)
	{
		WorkspaceTabs.Move(0, 1);
	}
}