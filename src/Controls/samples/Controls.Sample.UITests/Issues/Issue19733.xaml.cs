using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Maui.Controls.Sample.Issues
{
	[Issue(IssueTracker.Github, 19733, "No unintended focus change occurs", PlatformAffected.iOS)]
	public partial class Issue19733 : ContentPage
	{
		public Issue19733()
		{
			InitializeComponent();
		}

		//private void OnTapped(object sender, TappedEventArgs e)
		//{
		//	// Report that the callback was called.
		//	layout.Children.Add(new Label { Text = "TapAccepted", AutomationId = "TapAccepted" });

		//	string result = "Failure";
		//	string automationId = "Failure";

		//		//result = "Error: position is null";
		//		//result = $"Success: relative position is: X={position.Value.X}, Y={position.Value.Y}";
		//		// automationId = "Success";

		//	// Report the result of the test.
		//	layout.Children.Add(new Label { Text = result, AutomationId = automationId });
		//}
	}
}
