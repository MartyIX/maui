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
				//.UseMauiMaps()
				.UseMauiApp<App>()
				.ConfigureFonts
				(
					fonts =>
					{
						// Ubuntu fonts (https://fonts.google.com/specimen/Ubuntu/).
						_ = fonts.AddFont(filename: "Ubuntu-Light.ttf", alias: "UbuntuLight");
						_ = fonts.AddFont(filename: "Ubuntu-LightItalic.ttf", alias: "UbuntuLightItalic");
						_ = fonts.AddFont(filename: "Ubuntu-Regular.ttf", alias: "UbuntuRegular");
						_ = fonts.AddFont(filename: "Ubuntu-Italic.ttf", alias: "UbuntuItalic");
						_ = fonts.AddFont(filename: "Ubuntu-Medium.ttf", alias: "UbuntuMedium");
						_ = fonts.AddFont(filename: "Ubuntu-MediumItalic.ttf", alias: "UbuntuMediumItalic");
						_ = fonts.AddFont(filename: "Ubuntu-Bold.ttf", alias: "UbuntuBold");
						_ = fonts.AddFont(filename: "Ubuntu-BoldItalic.ttf", alias: "UbuntuBoldItalic");
					}
				)
				.Build();
	}

	class App : Application
	{
		protected override Window CreateWindow(IActivationState? activationState)
		{
			// To test shell scenarios, change this to true
			bool useShell = false;

			if (!useShell)
			{
				return new Window(new NavigationPage(new MainPage()));
			}
			else
			{
				return new Window(new SandboxShell());
			}
		}
	}
}