using Microsoft.Maui.LifecycleEvents;
using Microsoft.Maui.Platform;

namespace Maui.Controls.Sample;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp() =>
		MauiApp
			.CreateBuilder()
#if __ANDROID__ || __IOS__
			.UseMauiMaps()
#endif
			.UseMauiApp<App>()
#if WINDOWS
				.ConfigureLifecycleEvents(events =>
				{
					// Make sure to add "using Microsoft.Maui.LifecycleEvents;" in the top of the file 
					events.AddWindows(windowsLifecycleBuilder =>
					{
						windowsLifecycleBuilder.OnWindowCreated(window =>
						{
							window.ExtendsContentIntoTitleBar = false;
							var handle = WinRT.Interop.WindowNative.GetWindowHandle(window);
							var id = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(handle);
							var appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(id);
							switch (appWindow.Presenter)
							{
								case Microsoft.UI.Windowing.OverlappedPresenter overlappedPresenter:
									overlappedPresenter.SetBorderAndTitleBar(false, false);
									overlappedPresenter.Maximize();
									break;
							}
						});
					});
				})
#endif

			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("Dokdo-Regular.ttf", "Dokdo");
				fonts.AddFont("LobsterTwo-Regular.ttf", "Lobster Two");
				fonts.AddFont("LobsterTwo-Bold.ttf", "Lobster Two Bold");
				fonts.AddFont("LobsterTwo-Italic.ttf", "Lobster Two Italic");
				fonts.AddFont("LobsterTwo-BoldItalic.ttf", "Lobster Two BoldItalic");
				fonts.AddFont("ionicons.ttf", "Ionicons");
				fonts.AddFont("SegoeUI.ttf", "Segoe UI");
				fonts.AddFont("SegoeUI-Bold.ttf", "Segoe UI Bold");
				fonts.AddFont("SegoeUI-Italic.ttf", "Segoe UI Italic");
				fonts.AddFont("SegoeUI-Bold-Italic.ttf", "Segoe UI Bold Italic");
			})
			.Build();
}
