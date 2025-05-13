#nullable enable
using System;
using System.Collections.Generic;
using Foundation;
using UIKit;

namespace Microsoft.Maui.Devices
{
	partial class DeviceDisplayImplementation : IDeviceDisplay
	{
		readonly List<NSObject> _observers = [];

		// NSObject? _orientationObserver;
		// NSObject? _screenModeChangeObserver;

		protected override bool GetKeepScreenOn() =>
			UIApplication.SharedApplication.IdleTimerDisabled;

		protected override void SetKeepScreenOn(bool keepScreenOn) =>
			UIApplication.SharedApplication.IdleTimerDisabled = keepScreenOn;

		protected override DisplayInfo GetMainDisplayInfo()
		{
			var bounds = UIScreen.MainScreen.Bounds;
			var scale = UIScreen.MainScreen.Scale;

			var rate = (OperatingSystem.IsIOSVersionAtLeast(10, 3) || OperatingSystem.IsMacCatalystVersionAtLeast(10, 3) || OperatingSystem.IsTvOSVersionAtLeast(10, 3))
				? UIScreen.MainScreen.MaximumFramesPerSecond
				: 0;

			return new DisplayInfo(
				width: bounds.Width * scale,
				height: bounds.Height * scale,
				density: scale,
				orientation: CalculateOrientation(),
				rotation: CalculateRotation(),
				rate: rate);
		}

		[System.Runtime.Versioning.UnsupportedOSPlatform("ios13.0")]
		protected override void StartScreenMetricsListeners()
		{
			/*
			var notification = UIScreen.Notifications.ObserveBrightnessDidChange((sender, args) => {
				Console.WriteLine("ObserveBrightnessDidChange: Notification: {0}", args.Notification);
			});

			var notification2 = UIScreen.Notifications.ObserveModeDidChange((sender, args) => {
				Console.WriteLine("ObserveModeDidChange: Notification: {0}", args.Notification);
			});
			*/

			/*
			var observer = NSNotificationCenter.DefaultCenter.AddObserver(UIDevice.OrientationDidChangeNotification,
				n =>
				{
					Console.WriteLine("Callback #1");
				});

			var observer2 = NSNotificationCenter.DefaultCenter.AddObserver(UIScreen.BrightnessDidChangeNotification,
				n =>
				{
					Console.WriteLine("Callback #2");
				});

			var observer3 = NSNotificationCenter.DefaultCenter.AddObserver(UIScreen.ReferenceDisplayModeStatusDidChangeNotification,
				n =>
				{
					Console.WriteLine("Callback #3");
				});

			var observer4 = NSNotificationCenter.DefaultCenter.AddObserver(UIDevice.OrientationDidChangeNotification,
				n =>
				{
					Console.WriteLine("Callback #4");
				});
			*/

			_observers.Add(NSNotificationCenter.DefaultCenter.AddObserver(UIDevice.OrientationDidChangeNotification, OnMainDisplayInfoChanged));
			_observers.Add(NSNotificationCenter.DefaultCenter.AddObserver(UIScreen.ModeDidChangeNotification, OnMainDisplayInfoChanged));
			_observers.Add(NSNotificationCenter.DefaultCenter.AddObserver(UIScreen.BrightnessDidChangeNotification, OnMainDisplayInfoChanged));
			_observers.Add(NSNotificationCenter.DefaultCenter.AddObserver(UIScreen.ReferenceDisplayModeStatusDidChangeNotification, OnMainDisplayInfoChanged));
		}

		protected override void StopScreenMetricsListeners()
		{
			foreach (var observer in _observers)
			{
				observer.Dispose();
			}
		}

		void OnMainDisplayInfoChanged(NSNotification obj)
		{
			Console.WriteLine("OnMainDisplayInfoChanged called");
			OnMainDisplayInfoChanged();
		}

#pragma warning disable CA1416 // UIApplication.StatusBarOrientation has [UnsupportedOSPlatform("ios9.0")]. (Deprecated but still works)
#pragma warning disable CA1422 // Validate platform compatibility
		static DisplayOrientation CalculateOrientation() =>
			UIApplication.SharedApplication.StatusBarOrientation.IsLandscape()
				? DisplayOrientation.Landscape
				: DisplayOrientation.Portrait;

		static DisplayRotation CalculateRotation() =>
			UIApplication.SharedApplication.StatusBarOrientation switch
			{
				UIInterfaceOrientation.Portrait => DisplayRotation.Rotation0,
				UIInterfaceOrientation.PortraitUpsideDown => DisplayRotation.Rotation180,
				UIInterfaceOrientation.LandscapeLeft => DisplayRotation.Rotation270,
				UIInterfaceOrientation.LandscapeRight => DisplayRotation.Rotation90,
				_ => DisplayRotation.Unknown,
			};

#pragma warning restore CA1422 // Validate platform compatibility
#pragma warning restore CA1416
	}
}
