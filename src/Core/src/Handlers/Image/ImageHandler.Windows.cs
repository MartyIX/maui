using System;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using WImage = Microsoft.UI.Xaml.Controls.Image;

namespace Microsoft.Maui.Handlers
{
	public partial class ImageHandler : ViewHandler<IImage, WImage>
	{
		protected override WImage CreatePlatformView() => new WImage();

		protected override void ConnectHandler(WImage platformView)
		{
			System.Diagnostics.Debug.WriteLine($"XXX {DateTime.UtcNow:HH:mm:ss.fff} ImageHandler.ConnectHandler [S]");

			platformView.ImageOpened += OnImageOpened;

			base.ConnectHandler(platformView);

			System.Diagnostics.Debug.WriteLine($"XXX {DateTime.UtcNow:HH:mm:ss.fff} ImageHandler.ConnectHandler [E]");
		}

		protected override void DisconnectHandler(WImage platformView)
		{
			System.Diagnostics.Debug.WriteLine($"XXX {DateTime.UtcNow:HH:mm:ss.fff} ImageHandler.DisconnectHandler [S]");

			platformView.ImageOpened -= OnImageOpened;

			base.DisconnectHandler(platformView);
			SourceLoader.Reset();

			System.Diagnostics.Debug.WriteLine($"XXX {DateTime.UtcNow:HH:mm:ss.fff} ImageHandler.DisconnectHandler [E]");
		}

		public override bool NeedsContainer =>
			VirtualView?.Background != null ||
			base.NeedsContainer;

		public static void MapBackground(IImageHandler handler, IImage image)
		{
			handler.UpdateValue(nameof(IViewHandler.ContainerView));
			handler.ToPlatform().UpdateBackground(image);
		}

		public static void MapAspect(IImageHandler handler, IImage image) =>
			handler.PlatformView?.UpdateAspect(image);

		public static void MapIsAnimationPlaying(IImageHandler handler, IImage image) =>
			handler.PlatformView?.UpdateIsAnimationPlaying(image);

		public static void MapSource(IImageHandler handler, IImage image) =>
			MapSourceAsync(handler, image).FireAndForget(handler);

		public static Task MapSourceAsync(IImageHandler handler, IImage image)
		{
			System.Diagnostics.Debug.WriteLine($"XXX {DateTime.UtcNow:HH:mm:ss.fff} ImageSourcePartLoader.MapSourceAsync: [S/E]");
			return handler.SourceLoader.UpdateImageSourceAsync();
		}

		void OnImageOpened(object sender, RoutedEventArgs e)
		{
			System.Diagnostics.Debug.WriteLine($"XXX {DateTime.UtcNow:HH:mm:ss.fff} ImageHandler.OnImageOpened [S]");

			// Because this resolves from a task we should validate that the
			// handler hasn't been disconnected
			if (this.IsConnected())
				UpdateValue(nameof(IImage.IsAnimationPlaying));

			System.Diagnostics.Debug.WriteLine($"XXX {DateTime.UtcNow:HH:mm:ss.fff} ImageHandler.OnImageOpened [E]");
		}

		partial class ImageImageSourcePartSetter
		{
			public override void SetImageSource(ImageSource? platformImage)
			{
				System.Diagnostics.Debug.WriteLine($"XXX {DateTime.UtcNow:HH:mm:ss.fff} ImageImageSourcePartSetter.SetImageSource [S]");

				if (Handler?.PlatformView is not WImage image)
					return;

				image.Source = platformImage;

				System.Diagnostics.Debug.WriteLine($"XXX {DateTime.UtcNow:HH:mm:ss.fff} ImageImageSourcePartSetter.SetImageSource [E]");
			}
		}
	}
}