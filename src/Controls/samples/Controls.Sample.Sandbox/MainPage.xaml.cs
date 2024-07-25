using Microsoft.Maui.Graphics.Platform;

namespace Maui.Controls.Sample;

public partial class MainPage : ContentPage
{
	private int counter;

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

	private void NewWindowButton_Clicked(object sender, EventArgs e)
	{
		counter++;
		TestPage page = new()
		{
			Description = $"#{counter}"
		};

		Window secondWindow = new Window(page);
		Application.Current!.OpenWindow(secondWindow);
	}

	private void ActivateWindow2_Clicked(object sender, EventArgs e)
	{
		IReadOnlyList<Window> windows = Application.Current!.Windows;

		int windowNumber = int.Parse(windowToActivate.Text);
		int windowIndex = windowNumber - 1;

		if (windows.Count >= windowNumber)
		{
			Window windowToActivate = windows[windowIndex];
			var platformView = windowToActivate.Handler.PlatformView;

#if WINDOWS

			if (platformView is Microsoft.UI.Xaml.Window platformWindow)
			{
				platformWindow.Activate();
			}

#elif MACCATALYST17_0_OR_GREATER

			if (platformView is UIKit.UIView platformWindow)
			{
				UIKit.UISceneSessionActivationRequest activationRequest = UIKit.UISceneSessionActivationRequest.Create(platformWindow.Window.WindowScene!.Session);
				UIKit.UIApplication.SharedApplication.ActivateSceneSession(activationRequest, errorHandler: null);
			}

#elif MACCATALYST

			if (platformView is UIKit.UIView platformWindow)
			{
//#pragma warning disable CA1422 // Validate platform compatibility
				UIKit.UIApplication.SharedApplication.RequestSceneSessionActivation(
					sceneSession: null,
					userActivity: platformWindow.Window.WindowScene!.UserActivity, // get the UserActivity
					options: null,
					errorHandler: null
				);
//#pragma warning restore CA1422 // Validate platform compatibility
			}

#endif
		}
	}
}
