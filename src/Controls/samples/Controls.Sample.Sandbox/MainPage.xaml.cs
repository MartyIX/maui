using System.IO;
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
			using HttpClient httpClient = new();

			int count = 9;
			Stream[] streams = new Stream[count];
			Task<Microsoft.Maui.Graphics.IImage>[] imageTasks = new Task<Microsoft.Maui.Graphics.IImage>[count];

			// Download sample images.
			for (int i = 0; i < count; i++)
			{
				streams[i] = await httpClient.GetStreamAsync("https://picsum.photos/100");
			}

			// Attempt to load them in parallel to make it more likely to hit any possible error.
			for (int i = 0; i < count; i++)
			{
				Stream stream = streams[i];
				imageTasks[i] = Task.Run(() => PlatformImage.FromStream(stream));
			}

			Microsoft.Maui.Graphics.IImage[] images = imageTasks.Select(task => task.Result).ToArray();

			graphicsVIew.Drawable = new GraphicsDrawable(images);
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex);
		}
	}
}
