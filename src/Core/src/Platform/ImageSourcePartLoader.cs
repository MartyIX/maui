﻿using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.Maui.Handlers;

#if IOS || MACCATALYST
using PlatformImage = UIKit.UIImage;
using PlatformView = UIKit.UIView;
#elif ANDROID
using PlatformImage = Android.Graphics.Drawables.Drawable;
using PlatformView = Android.Views.View;
#elif WINDOWS
using PlatformImage = Microsoft.UI.Xaml.Media.ImageSource;
using PlatformView = Microsoft.UI.Xaml.FrameworkElement;
#elif TIZEN
using PlatformImage = Microsoft.Maui.Platform.MauiImageSource;
using PlatformView = Tizen.NUI.BaseComponents.View;
#elif (NETSTANDARD || !PLATFORM) || (NET6_0_OR_GREATER && !IOS && !ANDROID && !TIZEN)
using PlatformImage = System.Object;
using PlatformView = System.Object;
#endif

namespace Microsoft.Maui.Platform
{
	public partial class ImageSourcePartLoader
	{
#if IOS || ANDROID || WINDOWS || TIZEN
		IImageSourceServiceProvider? _imageSourceServiceProvider;
#endif

		readonly IImageSourcePartSetter _setter;

		internal ImageSourceServiceResultManager SourceManager { get; } = new ImageSourceServiceResultManager();

		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("Use ImageSourcePartLoader(IImageSourcePartSetter setter) instead.", true)]
		public ImageSourcePartLoader(IElementHandler handler, Func<IImageSourcePart?> imageSourcePart, Action<PlatformImage?> setImage)
			: this((IImageSourcePartSetter)handler)
		{
		}

		public ImageSourcePartLoader(IImageSourcePartSetter setter) =>
			_setter = setter;

		internal IImageSourcePartSetter Setter => _setter;

		public void Reset()
		{
			SourceManager.Reset();
		}

		public async Task UpdateImageSourceAsync()
		{
			System.Diagnostics.Debug.WriteLine($"XXX {DateTime.UtcNow:HH:mm:ss.fff} ImageSourcePartLoader.UpdateImageSourceAsync [S]");

			if (Setter.Handler is not IElementHandler handler || handler.PlatformView is not PlatformView platformView)
			{
				System.Diagnostics.Debug.WriteLine($"XXX {DateTime.UtcNow:HH:mm:ss.fff} ImageSourcePartLoader.UpdateImageSourceAsync [E]<#1>");
				return;
			}

			var token = SourceManager.BeginLoad();
			var imageSource = Setter.ImageSourcePart;

			if (imageSource?.Source is not null)
			{
#if IOS || ANDROID || WINDOWS || TIZEN
				_imageSourceServiceProvider ??= handler.GetRequiredService<IImageSourceServiceProvider>();
#endif

#if IOS || WINDOWS
				System.Diagnostics.Debug.WriteLine($"XXX {DateTime.UtcNow:HH:mm:ss.fff} ImageSourcePartLoader.UpdateImageSourceAsync: [1]");
				var scale = handler.MauiContext?.GetOptionalPlatformWindow()?.GetDisplayDensity() ?? 1.0f;

				System.Diagnostics.Debug.WriteLine($"XXX {DateTime.UtcNow:HH:mm:ss.fff} ImageSourcePartLoader.UpdateImageSourceAsync: [2]");
				var result = await imageSource.UpdateSourceAsync(platformView, _imageSourceServiceProvider, Setter.SetImageSource, scale, token)
					.ConfigureAwait(false);

				System.Diagnostics.Debug.WriteLine($"XXX {DateTime.UtcNow:HH:mm:ss.fff} ImageSourcePartLoader.UpdateImageSourceAsync: [3]");
				SourceManager.CompleteLoad(result);

				System.Diagnostics.Debug.WriteLine($"XXX {DateTime.UtcNow:HH:mm:ss.fff} ImageSourcePartLoader.UpdateImageSourceAsync: [4]");
#elif ANDROID || TIZEN
				var result = await imageSource.UpdateSourceAsync(platformView, _imageSourceServiceProvider, Setter.SetImageSource, token)
					.ConfigureAwait(false);
#else
				await Task.CompletedTask;
#endif

				System.Diagnostics.Debug.WriteLine($"XXX {DateTime.UtcNow:HH:mm:ss.fff} ImageSourcePartLoader.UpdateImageSourceAsync: [5]");
			}
			else
			{
				Setter.SetImageSource(null);
			}

			System.Diagnostics.Debug.WriteLine($"XXX {DateTime.UtcNow:HH:mm:ss.fff} ImageSourcePartLoader.UpdateImageSourceAsync [E]");
		}
	}
}
