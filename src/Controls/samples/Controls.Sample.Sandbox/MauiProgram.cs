using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;

namespace Maui.Controls.Sample
{
	public static class MauiProgram
	{
		public static MauiApp CreateMauiApp() =>
			MauiApp
				.CreateBuilder()
				// .UseMauiMaps()
				.UseMauiApp<App>()
				.Build();
	}

	class App : Application
	{
		protected override Window CreateWindow(IActivationState? activationState)
		{
			// To test shell scenarios, change this to true
			bool useShell = false;

			Window window;

			if (!useShell)
			{
				window = new Window(new NavigationPage(new MainPage()));
			}
			else
			{
				window = new Window(new SandboxShell());
			}

			window.MaximumWidth = 400;
			window.MaximumHeight = 400;
			return window;

		}
	}
}