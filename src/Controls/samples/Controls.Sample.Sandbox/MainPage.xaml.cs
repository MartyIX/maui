using System.Reflection;
using Microsoft.Maui.Platform;
using Microsoft.UI.Windowing;

namespace Maui.Controls.Sample;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}

	private async void OnCounterClicked(object sender, EventArgs e)
	{
		System.Diagnostics.Debug.WriteLine("OnCounterClicked");

		var modalPage = new ModalTestPage();

		System.Diagnostics.Debug.WriteLine("OnCounterClicked: Pushing modal");
		await Navigation.PushModalAsync(modalPage);
		System.Diagnostics.Debug.WriteLine("OnCounterClicked: Modal pushed");

		/*
		var platformWindow = (MauiWinUIWindow)(modalPage.Window.Handler!.PlatformView)!;
		platformWindow.ExtendsContentIntoTitleBar = false;
		platformWindow.AppWindow.TitleBar.ExtendsContentIntoTitleBar = false;
		var presenter = platformWindow.AppWindow.Presenter as OverlappedPresenter;

		// Disable titlebar, maximize the window
		presenter!.SetBorderAndTitleBar(false, false);
		presenter.Maximize();

		var handler = modalPage.Window.Handler!;
		var navigationRootManager = handler.MauiContext!.Services.GetRequiredService<NavigationRootManager>()!;

		//System.Diagnostics.Debug.WriteLine("OnCounterClicked: Hide titlebar");
		//var method = typeof(NavigationRootManager).GetMethod("SetTitleBarVisibility", BindingFlags.Instance | BindingFlags.NonPublic)!;
		//method.Invoke(navigationRootManager, new object[] { handler.PlatformView!, false });

		// ((WindowRootView)navigationRootManager.RootView)!.AppTitleBarTemplate = null;
		// ((WindowRootView)navigationRootManager.RootView)!.AppTitleBarTemplate = null;

		System.Diagnostics.Debug.WriteLine("OnCounterClicked: Hide titlebar");
		var method = typeof(WindowRootView).GetMethod("UpdateAppTitleBar", BindingFlags.Instance | BindingFlags.NonPublic)!;
		method.Invoke(navigationRootManager.RootView, new object[] { 0, false });
		*/

		System.Diagnostics.Debug.WriteLine("OnCounterClicked finished");
	}
}
