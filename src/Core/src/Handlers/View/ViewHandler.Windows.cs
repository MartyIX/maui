#nullable enable
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Input;
using System.Linq;
using PlatformView = Microsoft.UI.Xaml.FrameworkElement;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Automation;

namespace Microsoft.Maui.Handlers
{
	public partial class ViewHandler
	{
		static Dictionary<PlatformView, ViewHandler>? FocusManagerMapping;
		static int Counter;

		partial void ConnectingHandler(PlatformView? platformView)
		{
			if (platformView is not null)
			{
				Counter++;
				string automationId = $"X{platformView.GetType().Name}{Counter}";
				AutomationProperties.SetAutomationId(platformView, automationId);

				System.Diagnostics.Debug.WriteLine($"XXX: > ConnectingHandler: Platform view: {platformView.GetType().FullName} ({platformView.GetHashCode()}) ('{automationId}')");

				if (FocusManagerMapping is null)
				{
					FocusManagerMapping = new(EqualityComparer<PlatformView>.Default);

					FocusManager.GotFocus += FocusManager_GotFocus;
					FocusManager.GettingFocus += FocusManager_GettingFocus;
					FocusManager.LostFocus += FocusManager_LostFocus;
				}

				FocusManagerMapping.Add(platformView, this);
			}
		}

		partial void DisconnectingHandler(PlatformView platformView)
		{
			_ = FocusManagerMapping ?? throw new InvalidOperationException($"{nameof(FocusManagerMapping)} should have been set.");

			UpdateIsFocused(false);

			System.Diagnostics.Debug.WriteLine($"XXX: > DisconnectingHandler: Removing platform view: {platformView.GetHashCode()}");
			FocusManagerMapping.Remove(platformView);
		}

		static partial void MappingFrame(IViewHandler handler, IView view)
		{
			// Both Clip and Shadow depend on the Control size.
			handler.ToPlatform().UpdateClip(view);
			handler.ToPlatform().UpdateShadow(view);
		}

		public static void MapTranslationX(IViewHandler handler, IView view)
		{
			handler.ToPlatform().UpdateTransformation(view);
		}

		public static void MapTranslationY(IViewHandler handler, IView view)
		{
			handler.ToPlatform().UpdateTransformation(view);
		}

		public static void MapScale(IViewHandler handler, IView view)
		{
			handler.ToPlatform().UpdateTransformation(view);
		}

		public static void MapScaleX(IViewHandler handler, IView view)
		{
			handler.ToPlatform().UpdateTransformation(view);
		}

		public static void MapScaleY(IViewHandler handler, IView view)
		{
			handler.ToPlatform().UpdateTransformation(view);
		}

		public static void MapRotation(IViewHandler handler, IView view)
		{
			handler.ToPlatform().UpdateTransformation(view);
		}

		public static void MapRotationX(IViewHandler handler, IView view)
		{
			handler.ToPlatform().UpdateTransformation(view);
		}

		public static void MapRotationY(IViewHandler handler, IView view)
		{
			handler.ToPlatform().UpdateTransformation(view);
		}

		public static void MapAnchorX(IViewHandler handler, IView view)
		{
			handler.ToPlatform().UpdateTransformation(view);
		}

		public static void MapAnchorY(IViewHandler handler, IView view)
		{
			handler.ToPlatform().UpdateTransformation(view);
		}

		public static void MapToolbar(IViewHandler handler, IView view)
		{
			if (view is IToolbarElement tb)
			{
				MapToolbar(handler, tb);
			}
		}

		internal static void MapToolbar(IElementHandler handler, IToolbarElement toolbarElement)
		{
			_ = handler.MauiContext ?? throw new InvalidOperationException($"{nameof(handler.MauiContext)} null");

			if (toolbarElement.Toolbar != null)
			{
				var toolBar = toolbarElement.Toolbar.ToPlatform(handler.MauiContext);
				handler.MauiContext.GetNavigationRootManager().SetToolbar(toolBar);
			}
		}

		public static void MapContextFlyout(IViewHandler handler, IView view)
		{
			if (view is IContextFlyoutElement contextFlyoutContainer)
			{
				MapContextFlyout(handler, contextFlyoutContainer);
			}
		}

		internal static void MapContextFlyout(IElementHandler handler, IContextFlyoutElement contextFlyoutContainer)
		{
			_ = handler.MauiContext ?? throw new InvalidOperationException($"The handler's {nameof(handler.MauiContext)} cannot be null.");

			if (handler.PlatformView is Microsoft.UI.Xaml.UIElement uiElement)
			{
				if (contextFlyoutContainer.ContextFlyout != null)
				{
					var contextFlyoutHandler = contextFlyoutContainer.ContextFlyout.ToHandler(handler.MauiContext);
					var contextFlyoutPlatformView = contextFlyoutHandler.PlatformView;

					if (contextFlyoutPlatformView is FlyoutBase flyoutBase)
					{
						uiElement.ContextFlyout = flyoutBase;
					}
				}
				else
				{
					uiElement.ClearValue(UIElement.ContextFlyoutProperty);
				}
			}
		}

		private void FocusManager_GettingFocus(object? sender, GettingFocusEventArgs e)
		{
			System.Diagnostics.Debug.WriteLine($"XXX: > FocusManager_GettingFocus(oldFocusedElement:{e.OldFocusedElement?.GetType().FullName}, newFocusedElement:{e.NewFocusedElement?.GetType().FullName})");

			if (e.NewFocusedElement is PlatformView platformView)
			{
				string automationId = e.NewFocusedElement is not null ? AutomationProperties.GetAutomationId(e.NewFocusedElement) : "NULL";
				System.Diagnostics.Debug.WriteLine($"XXX: FocusManager_GettingFocus: is platform view: '{platformView.GetType().FullName}'|'{automationId}'");
			}

			System.Diagnostics.Debug.WriteLine($"XXX: < FocusManager_GettingFocus");
		}

		static void FocusManager_GotFocus(object? sender, FocusManagerGotFocusEventArgs e)
		{
			System.Diagnostics.Debug.WriteLine($"XXX: > FocusManager_GotFocus({e.NewFocusedElement?.GetType().FullName})");
			_ = FocusManagerMapping ?? throw new InvalidOperationException($"{nameof(FocusManagerMapping)} should have been set.");

			if (e.NewFocusedElement is PlatformView platformView)
			{
				string automationId = e.NewFocusedElement is not null ? AutomationProperties.GetAutomationId(e.NewFocusedElement) : "NULL";
				System.Diagnostics.Debug.WriteLine($"XXX: FocusManager_GotFocus: is platform view: '{platformView.GetType().FullName}'|'{automationId}'");

				if (FocusManagerMapping.TryGetValue(platformView, out ViewHandler? viewHandler))
				{
					System.Diagnostics.Debug.WriteLine($"XXX: FocusManager_GotFocus: true");
					viewHandler.UpdateIsFocused(true);
				}
				else
				{
					if (platformView is Button)
					{
						System.Diagnostics.Debug.WriteLine($"XXX: FocusManager_GotFocus[Button]: Not found platform view {platformView.GetHashCode()} (automationId: {automationId})!");
					}

					System.Diagnostics.Debug.WriteLine($"XXX: FocusManager_GotFocus: Not found platform view {platformView.GetHashCode()}!");
				}
			}
			else
			{
				System.Diagnostics.Debug.WriteLine($"XXX: FocusManager_GotFocus: not a platform view but: {e.NewFocusedElement?.GetType().FullName}");
			}

			System.Diagnostics.Debug.WriteLine($"XXX: < FocusManager_GotFocus");
		}

		static void FocusManager_LostFocus(object? sender, FocusManagerLostFocusEventArgs e)
		{			
			System.Diagnostics.Debug.WriteLine($"XXX: > FocusManager_LostFocus({e.OldFocusedElement?.GetType().FullName})");
			_ = FocusManagerMapping ?? throw new InvalidOperationException($"{nameof(FocusManagerMapping)} should have been set.");

			if (e.OldFocusedElement is PlatformView platformView)
			{
				string automationId = e.OldFocusedElement is not null ? AutomationProperties.GetAutomationId(e.OldFocusedElement) : "NULL";
				System.Diagnostics.Debug.WriteLine($"XXX: FocusManager_LostFocus: is platform view: '{platformView.GetType().FullName}'|{automationId}");

				if (FocusManagerMapping.TryGetValue(platformView, out ViewHandler? viewHandler))
				{
					System.Diagnostics.Debug.WriteLine($"XXX: FocusManager_LostFocus: false");
					viewHandler.UpdateIsFocused(false);
				}
				else
				{
					System.Diagnostics.Debug.WriteLine($"XXX: FocusManager_LostFocus: Not found platform view {platformView.GetHashCode()}!");
				}
			}
			else
			{
				System.Diagnostics.Debug.WriteLine($"XXX: FocusManager_LostFocus: not a platform view but: {e.OldFocusedElement?.GetType().FullName}");
			}

			System.Diagnostics.Debug.WriteLine($"XXX: < FocusManager_LostFocus");
		}

		void UpdateIsFocused(bool isFocused)
		{
			if (VirtualView == null)
			{
				return;
			}

			bool updateIsFocused = (isFocused && !VirtualView.IsFocused) || (!isFocused && VirtualView.IsFocused);

			if (updateIsFocused)
			{
				VirtualView.IsFocused = isFocused;
			}
		}
	}
}