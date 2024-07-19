using Microsoft.Maui.Graphics.Platform;

namespace Maui.Controls.Sample;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}

	private async void OnCounterClicked(object sender, EventArgs e)
	{
		try
		{
			var imageUrl = "https://cdn-dynmedia-1.microsoft.com/is/image/microsoftcorp/Highlight-Surface-Laptop-AI-7Ed-Sapphire-MC001-3000x1682:VP5-1920x600";
			var stream = await new HttpClient().GetStreamAsync(imageUrl);
			var img = PlatformImage.FromStream(stream);
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex);
		}

	}
}
