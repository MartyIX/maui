using System;
using Maui.Controls.Sample.Sandbox;
using System.Collections.ObjectModel;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace Maui.Controls.Sample
{
	public partial class MainPage : ContentPage
	{
		/// <summary>List of tool tabs.</summary>
		public ObservableCollection<ToolTab> ToolTabs { get; }

		public MainPage()
		{
			ToolTabs = new();
			ToolTabs.Add(new ToolTab("Zero", selected: true));
			ToolTabs.Add(new ToolTab("First"));
			ToolTabs.Add(new ToolTab("Second"));
			ToolTabs.Add(new ToolTab("Third"));
			ToolTabs.Add(new ToolTab("Fourth"));
			ToolTabs.Add(new ToolTab("Fifth"));
			ToolTabs.Add(new ToolTab("Sixth"));
			ToolTabs.Add(new ToolTab("Seventh"));
			ToolTabs.Add(new ToolTab("Eighth"));
			ToolTabs.Add(new ToolTab("Ninth"));
			ToolTabs.Add(new ToolTab("Tenth"));

			BindingContext = this;

			InitializeComponent();
		}

		private void tabsLayout_Loaded(object sender, EventArgs e)
		{
			Window.SizeChanged += Window_SizeChanged;
		}

		private void Window_SizeChanged(object? sender, EventArgs e)
		{
			tabsLayout.Invalidate();
		}

		private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
		{
			if (sender is not View view)
				throw new Exception($"Sender is not a view. Got '{sender?.GetType().FullName}'.");

			if (view.BindingContext is not ToolTab toolTab)
				throw new Exception($"Sender is not a view. Got '{view.BindingContext?.GetType().FullName}'.");

			tabsLayout.SetSelected(toolTab);
		}
	}
}