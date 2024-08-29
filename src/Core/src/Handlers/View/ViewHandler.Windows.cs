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
				platformView.GotFocus += OnPlatformViewGotFocus;
				platformView.LostFocus += OnPlatformViewLostFocus;

				Counter++;
				string automationId = $"X{platformView.GetType().Name}{Counter}";
				AutomationProperties.SetAutomationId(platformView, automationId);

				System.Diagnostics.Debug.WriteLine($"XXX: > ConnectingHandler: Platform view: {platformView.GetType().FullName} ({platformView.GetHashCode()}) ('{automationId}')");

				if (FocusManagerMapping is null)
				{
					FocusManagerMapping = new(EqualityComparer<PlatformView>.Default);

					FocusManager.GettingFocus += FocusManager_GettingFocus;
					FocusManager.GotFocus += FocusManager_GotFocus;
					FocusManager.LosingFocus += FocusManager_LosingFocus;
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

			platformView.GotFocus -= OnPlatformViewGotFocus;
			platformView.LostFocus -= OnPlatformViewLostFocus;
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

		void OnPlatformViewGotFocus(object sender, RoutedEventArgs args)
		{
			UpdateIsFocused(true);
			System.Diagnostics.Debug.WriteLine($"XXX: OnPlatformViewGotFocus(sender:{sender?.GetType().FullName} ({sender?.GetHashCode()}))");
		}

		void OnPlatformViewLostFocus(object sender, RoutedEventArgs args)
		{
			UpdateIsFocused(false);
			System.Diagnostics.Debug.WriteLine($"XXX: OnPlatformViewLostFocus(sender:{sender?.GetType().FullName} ({sender?.GetHashCode()}))");
		}

		static void FocusManager_GettingFocus(object? sender, GettingFocusEventArgs e)
		{
			_ = FocusManagerMapping ?? throw new InvalidOperationException($"{nameof(FocusManagerMapping)} should have been set.");

			System.Diagnostics.Debug.WriteLine($"XXX: > FocusManager_GettingFocus(sender:{sender?.GetType().FullName} ({sender?.GetHashCode()}),correlationId:{e.CorrelationId}," +
				$"oldFocusedElement:{e.OldFocusedElement?.GetType().FullName} ({e.OldFocusedElement?.GetHashCode()})," +
				$"newFocusedElement:{e.NewFocusedElement?.GetType().FullName}  ({e.NewFocusedElement?.GetHashCode()}))");

			if (e.NewFocusedElement is PlatformView platformView)
			{
				string automationId = e.NewFocusedElement is not null ? AutomationProperties.GetAutomationId(e.NewFocusedElement) : "NULL";

				if (FocusManagerMapping.TryGetValue(platformView, out ViewHandler? viewHandler))
				{
					System.Diagnostics.Debug.WriteLine($"XXX: FocusManager_GettingFocus: OK!! [platform view: '{platformView.GetType().FullName}'|{platformView.GetHashCode()}|'{automationId}']");
					viewHandler.UpdateIsFocused(true, dry: true);
				}
				else
				{
					if (platformView is Button)
					{
						System.Diagnostics.Debug.WriteLine($"XXX: FocusManager_GettingFocus[Button]: Not found platform view [platform view: '{platformView.GetType().FullName}'|{platformView.GetHashCode()}|'{automationId}']");
					}
					else
					{
						System.Diagnostics.Debug.WriteLine($"XXX: FocusManager_GettingFocus[Other]: Not found platform view [platform view: '{platformView.GetType().FullName}'|{platformView.GetHashCode()}]");
					}
				}
			}

			System.Diagnostics.Debug.WriteLine($"XXX: < FocusManager_GettingFocus");
		}

		static void FocusManager_GotFocus(object? sender, FocusManagerGotFocusEventArgs e)
		{
			_ = FocusManagerMapping ?? throw new InvalidOperationException($"{nameof(FocusManagerMapping)} should have been set.");

			if (sender is null)
			{
				var v = FocusManager.GetFocusedElement();
				System.Diagnostics.Debug.WriteLine($"XXX: NO SENDER: {v?.GetType().FullName} ({v?.GetHashCode()})");
			}

			System.Diagnostics.Debug.WriteLine($"XXX: > FocusManager_GotFocus(sender:{sender?.GetType().FullName} ({sender?.GetHashCode()}),correlationId:{e.CorrelationId},newFocusedElement:{e.NewFocusedElement?.GetType().FullName} ({e.NewFocusedElement?.GetHashCode()}))");

			if (e.NewFocusedElement is PlatformView platformView)
			{
				string automationId = e.NewFocusedElement is not null ? AutomationProperties.GetAutomationId(e.NewFocusedElement) : "NULL";

				if (FocusManagerMapping.TryGetValue(platformView, out ViewHandler? viewHandler))
				{
					System.Diagnostics.Debug.WriteLine($"XXX: FocusManager_GotFocus: OK!! [platform view: '{platformView.GetType().FullName}'|{platformView.GetHashCode()}|'{automationId}']");
					viewHandler.UpdateIsFocused(true, dry: true);
				}
				else
				{
					if (platformView is Button)
					{
						System.Diagnostics.Debug.WriteLine($"XXX: FocusManager_GotFocus[Button]: Not found platform view [platform view: '{platformView.GetType().FullName}'|{platformView.GetHashCode()}|'{automationId}']");
					}
					else
					{
						System.Diagnostics.Debug.WriteLine($"XXX: FocusManager_GotFocus[Other]: Not found platform view [platform view: '{platformView.GetType().FullName}'|{platformView.GetHashCode()}]");
					}
				}
			}
			else if (e.NewFocusedElement is DependencyObject dependencyObject)
			{
				System.Diagnostics.Debug.WriteLine($"XXX: FocusManager_GotFocus[DEPENDENCY_OBJECT]: {dependencyObject?.GetType().FullName} ({dependencyObject?.GetHashCode()})");
			} 
			else if (e.NewFocusedElement is null)
			{
				System.Diagnostics.Debug.WriteLine($"XXX: FocusManager_GotFocus: NEW focused element is NULL; {e.NewFocusedElement}");
			}
			else
			{
				System.Diagnostics.Debug.WriteLine($"XXX: FocusManager_GotFocus: not a platform view but: {e.NewFocusedElement?.GetType().FullName}");
			}

			System.Diagnostics.Debug.WriteLine($"XXX: < FocusManager_GotFocus");
		}

		private void FocusManager_LosingFocus(object? sender, LosingFocusEventArgs e)
		{
			System.Diagnostics.Debug.WriteLine($"XXX: > FocusManager_LosingFocus(sender:{sender?.GetType().FullName} ({sender?.GetHashCode()}),correlationId:{e.CorrelationId}," +
				$"oldFocusedElement:{e.OldFocusedElement?.GetType().FullName} ({e.OldFocusedElement?.GetHashCode()}))" + 
				$"newFocusedElement:{e.NewFocusedElement?.GetType().FullName} ({e.NewFocusedElement?.GetHashCode()}))"
				);
			_ = FocusManagerMapping ?? throw new InvalidOperationException($"{nameof(FocusManagerMapping)} should have been set.");

			if (e.OldFocusedElement is PlatformView platformView)
			{
				string automationId = e.OldFocusedElement is not null ? AutomationProperties.GetAutomationId(e.OldFocusedElement) : "NULL";

				if (FocusManagerMapping.TryGetValue(platformView, out ViewHandler? viewHandler))
				{
					System.Diagnostics.Debug.WriteLine($"XXX: FocusManager_LosingFocus: OK!!! [platform view: '{platformView.GetType().FullName}'|{platformView.GetHashCode()}|'{automationId}']");
					// viewHandler.UpdateIsFocused(false, dry: true);
				}
				else
				{
					if (platformView is Button)
					{
						System.Diagnostics.Debug.WriteLine($"XXX: FocusManager_LosingFocus[Button]: Not found platform view [platform view: '{platformView.GetType().FullName}'|{platformView.GetHashCode()}|'{automationId}']");
					}
					else
					{
						System.Diagnostics.Debug.WriteLine($"XXX: FocusManager_LosingFocus[Other]: Not found platform view [platform view: '{platformView.GetType().FullName}'|{platformView.GetHashCode()}]");
					}
				}
			}
			else
			{
				System.Diagnostics.Debug.WriteLine($"XXX: FocusManager_LosingFocus: not a platform view but: {e.OldFocusedElement?.GetType().FullName}");
			}

			System.Diagnostics.Debug.WriteLine($"XXX: < FocusManager_LosingFocus");
		}

		static void FocusManager_LostFocus(object? sender, FocusManagerLostFocusEventArgs e)
		{
			System.Diagnostics.Debug.WriteLine($"XXX: > FocusManager_LostFocus(sender:{sender?.GetType().FullName} ({sender?.GetHashCode()}),correlationId:{e.CorrelationId},oldFocusedElement:{e.OldFocusedElement?.GetType().FullName} ({e.OldFocusedElement?.GetHashCode()}))");
			_ = FocusManagerMapping ?? throw new InvalidOperationException($"{nameof(FocusManagerMapping)} should have been set.");

			if (e.OldFocusedElement is PlatformView platformView)
			{
				string automationId = e.OldFocusedElement is not null ? AutomationProperties.GetAutomationId(e.OldFocusedElement) : "NULL";

				if (FocusManagerMapping.TryGetValue(platformView, out ViewHandler? viewHandler))
				{
					System.Diagnostics.Debug.WriteLine($"XXX: FocusManager_LostFocus: OK!!! [platform view: '{platformView.GetType().FullName}'|{platformView.GetHashCode()}|'{automationId}']");
					viewHandler.UpdateIsFocused(false, dry: true);
				}
				else
				{
					if (platformView is Button)
					{
						System.Diagnostics.Debug.WriteLine($"XXX: FocusManager_LostFocus[Button]: Not found platform view [platform view: '{platformView.GetType().FullName}'|{platformView.GetHashCode()}|'{automationId}']");
					}
					else
					{
						System.Diagnostics.Debug.WriteLine($"XXX: FocusManager_LostFocus[Other]: Not found platform view [platform view: '{platformView.GetType().FullName}'|{platformView.GetHashCode()}]");
					}
				}
			}
			else
			{
				System.Diagnostics.Debug.WriteLine($"XXX: FocusManager_LostFocus: not a platform view but: {e.OldFocusedElement?.GetType().FullName}");
			}

			System.Diagnostics.Debug.WriteLine($"XXX: < FocusManager_LostFocus");
		}

		void UpdateIsFocused(bool isFocused, bool dry = false)
		{
			if (VirtualView == null)
			{
				return;
			}

			bool updateIsFocused = (isFocused && !VirtualView.IsFocused) || (!isFocused && VirtualView.IsFocused);

			if (updateIsFocused)
			{
				if (dry)
				{
					System.Diagnostics.Debug.WriteLine($"XXX: UpdateIsFocused [NEW]: VirtualView:{VirtualView.GetType().FullName} isFocused={isFocused} [{VirtualView.Handler?.PlatformView?.GetType().FullName}|{VirtualView.Handler?.PlatformView?.GetHashCode()}]");
				}
				else
				{
					System.Diagnostics.Debug.WriteLine($"XXX: UpdateIsFocused [OLD]: VirtualView:{VirtualView.GetType().FullName} isFocused={isFocused} [{VirtualView.Handler?.PlatformView?.GetType().FullName}|{VirtualView.Handler?.PlatformView?.GetHashCode()}]");
					VirtualView.IsFocused = isFocused;
				}
			}
		}
	}
}