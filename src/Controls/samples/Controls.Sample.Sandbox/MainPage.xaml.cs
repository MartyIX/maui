namespace Maui.Controls.Sample;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}

	private async void Button_Clicked(object sender, EventArgs e)
	{
		IScreenshotResult? result = await myWebView.CaptureAsync();
		
		if (result != null)
		{
			Stream stream = await result.OpenReadAsync(ScreenshotFormat.Png, 100);
			using (MemoryStream memoryStream = new())
			{
				stream.CopyTo(memoryStream);
				File.WriteAllBytes(@"D:\test.png", memoryStream.ToArray());

				stream.Seek(0, SeekOrigin.Begin);

				screenshotResult.Add(new Label() { Text = $"Your screenshot ({result.Width}x{result.Height}):" });

				DisplayInfo displayInfo = DeviceDisplay.MainDisplayInfo;
				double width = result.Width / displayInfo.Density;
				double height = result.Height / displayInfo.Density;

				screenshotResult.Add(new Image() { Source = ImageSource.FromStream(() => stream), WidthRequest = width, HeightRequest = height });
				//screenshotResult.Add(new Image() { Source = ImageSource.FromFile(@"D:\test.png"), WidthRequest = width, HeightRequest = height });
			}
		}
	}
}
