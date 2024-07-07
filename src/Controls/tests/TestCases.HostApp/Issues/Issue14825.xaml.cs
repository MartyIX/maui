﻿#nullable enable
namespace Maui.Controls.Sample.Issues;

[XamlCompilation(XamlCompilationOptions.Compile)]
[Issue(IssueTracker.Github, 14825, "Capture WebView screenshot", PlatformAffected.UWP)]
public partial class Issue14825 : ContentPage
{
	public Issue14825()
	{
		InitializeComponent();
	}

	private async void CaptureButton_Clicked(object sender, EventArgs e)
	{
		IScreenshotResult? result = await myWebView.CaptureAsync();

		if (result != null)
		{
			using Stream stream = await result.OpenReadAsync(ScreenshotFormat.Png, 100);

			screenshotResult.Add(new Label() { Text = $"Your screenshot ({result.Width}x{result.Height}):" });

			DisplayInfo displayInfo = DeviceDisplay.MainDisplayInfo;
			double width = result.Width / displayInfo.Density;
			double height = result.Height / displayInfo.Density;

			screenshotResult.Add(new Image() { Source = ImageSource.FromStream(() => stream), WidthRequest = width, HeightRequest = height });
		}
	}
}