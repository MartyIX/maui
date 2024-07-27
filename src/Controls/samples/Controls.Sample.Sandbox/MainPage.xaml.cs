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

		debugInfo.Text += "#0 ";

		int windowNumber = int.Parse(windowToActivate.Text);
		int windowIndex = windowNumber - 1;

		debugInfo.Text += $"windows.count {windows.Count} ?? {windowNumber}";	

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

			debugInfo.Text += "+17_0_OR_GREATER ";

			if (platformView is UIKit.UIView platformWindow)
			{
				debugInfo.Text += "#1 ";

				if (platformWindow is UIKit.UIWindow window) {
					var windowScene = window.WindowScene;

					if (windowScene is not null) {
						UIKit.UISceneSessionActivationRequest activationRequest = UIKit.UISceneSessionActivationRequest.Create(windowScene.Session);

						debugInfo.Text += "#2 ";
						UIKit.UIApplication.SharedApplication.ActivateSceneSession(activationRequest, errorHandler: null);

						debugInfo.Text += "#3 ";
					} else {
						debugInfo.Text += "#window.scene.is.null ";
					}
				} else {
					debugInfo.Text += "#window.is.null ";	
				}
			}

			debugInfo.Text += "#4 ";

#elif MACCATALYST

			debugInfo.Text += "#MACCATALYT ";

			if (platformView is UIKit.UIView platformWindow)
			{
				debugInfo.Text += "#2 ";
//#pragma warning disable CA1422 // Validate platform compatibility
				UIKit.UIApplication.SharedApplication.RequestSceneSessionActivation(
					sceneSession: null,
					userActivity: platformWindow.Window.WindowScene!.UserActivity, // get the UserActivity
					options: null,
					errorHandler: null
				);
				debugInfo.Text += "#3 ";
//#pragma warning restore CA1422 // Validate platform compatibility
			}

			debugInfo.Text += "#4 ";

#endif
		}
	}
}
