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

				System.Diagnostics.Debug.WriteLine($"XXX: > ConnectingHandler: Connecting view {VirtualView?.GetType().FullName} " +
					$"with platform view: {platformView.GetType().FullName} ({platformView.GetHashCode()}) ('{automationId}')");

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

			System.Diagnostics.Debug.WriteLine($"XXX: > DisconnectingHandler: Removing view {VirtualView?.GetType().FullName} " +
				$"with platform view: {platformView.GetType().FullName} ({platformView.GetHashCode()})");
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
			System.Diagnostics.Debug.WriteLine($"XXX: > OnPlatformViewGotFocus[{VirtualView?.GetType().FullName} ({VirtualView?.GetHashCode()}); " +
				$"platformView: {VirtualView?.Handler?.PlatformView?.GetType().FullName} ({VirtualView?.Handler?.PlatformView?.GetHashCode()})]");

			UpdateIsFocused(true);

			System.Diagnostics.Debug.WriteLine($"XXX: < OnPlatformViewGotFocus");
		}

		void OnPlatformViewLostFocus(object sender, RoutedEventArgs args)
		{
			System.Diagnostics.Debug.WriteLine($"XXX: > OnPlatformViewLostFocus[{VirtualView?.GetType().FullName} ({VirtualView?.GetHashCode()}); " +
				$"platformView: {VirtualView?.Handler?.PlatformView?.GetType().FullName} ({VirtualView?.Handler?.PlatformView?.GetHashCode()})]");

			UpdateIsFocused(false);

			System.Diagnostics.Debug.WriteLine($"XXX: < OnPlatformViewLostFocus");
		}

		private void FocusManager_LosingFocus(object? sender, LosingFocusEventArgs e)
		{
			System.Diagnostics.Debug.WriteLine($"XXX: > #FocusManager#LosingFocus(e.Direction:{e.Direction},e.FocusState:{e.FocusState},e.Handled:{e.Handled}," +
				$"sender:{sender?.GetType().FullName} ({sender?.GetHashCode()})," +
				$"correlationId:{e.CorrelationId}," +
				$"oldFocusedElement:{e.OldFocusedElement?.GetType().FullName} ({e.OldFocusedElement?.GetHashCode()}))" +
				$"newFocusedElement:{e.NewFocusedElement?.GetType().FullName} ({e.NewFocusedElement?.GetHashCode()}))"
				);
			_ = FocusManagerMapping ?? throw new InvalidOperationException($"{nameof(FocusManagerMapping)} should have been set.");

			if (e.OldFocusedElement is PlatformView oldPlatformView)
			{
				if (FocusManagerMapping.TryGetValue(oldPlatformView, out ViewHandler? viewHandler))
				{
					System.Diagnostics.Debug.WriteLine($"XXX: FocusManager_LosingFocus[OLD_FOCUSED]: OK!! [platform view: {oldPlatformView.GetType().FullName} ({oldPlatformView.GetHashCode()})]");
					// viewHandler.UpdateIsFocused(false, @new: true);
				}
				else
				{
					System.Diagnostics.Debug.WriteLine($"XXX: FocusManager_LosingFocus[OLD_FOCUSED]: Not FrameworkElement instance.");
				}
			}

			//if (e.NewFocusedElement is PlatformView newPlatformView)
			//{
			//	if (FocusManagerMapping.TryGetValue(newPlatformView, out ViewHandler? viewHandler))
			//	{
			//		System.Diagnostics.Debug.WriteLine($"XXX: FocusManager_LosingFocus[NEW_FOCUSED]: OK!! [platform view: {newPlatformView.GetType().FullName} ({newPlatformView.GetHashCode()})]");
			//		viewHandler.UpdateIsFocused(true, @new: true);
			//	}
			//	else
			//	{
			//		System.Diagnostics.Debug.WriteLine($"XXX: FocusManager_LosingFocus[NEW_FOCUSED]: Not FrameworkElement instance.");
			//	}
			//}

			System.Diagnostics.Debug.WriteLine($"XXX: < FocusManager_LosingFocus");
		}

		static void FocusManager_GettingFocus(object? sender, GettingFocusEventArgs e)
		{
			_ = FocusManagerMapping ?? throw new InvalidOperationException($"{nameof(FocusManagerMapping)} should have been set.");

			System.Diagnostics.Debug.WriteLine($"XXX: > #FocusManager#GettingFocus(e.Direction:{e.Direction},e.FocusState:{e.FocusState},e.Handled:{e.Handled}," +
				$"sender:{sender?.GetType().FullName} ({sender?.GetHashCode()})," +
				$"correlationId:{e.CorrelationId}," +
				$"oldFocusedElement:{e.OldFocusedElement?.GetType().FullName ?? "NULL"} ({e.OldFocusedElement?.GetHashCode()})," +
				$"newFocusedElement:{e.NewFocusedElement?.GetType().FullName ?? "NULL"}  ({e.NewFocusedElement?.GetHashCode()}))");

			//if (e.OldFocusedElement is PlatformView oldPlatformView)
			//{
			//	if (FocusManagerMapping.TryGetValue(oldPlatformView, out ViewHandler? viewHandler))
			//	{
			//		System.Diagnostics.Debug.WriteLine($"XXX: FocusManager_GettingFocus[OLD_FOCUSED]: OK!! [platform view: {oldPlatformView.GetType().FullName} ({oldPlatformView.GetHashCode()})]");
			//		viewHandler.UpdateIsFocused(false, @new: true);
			//	}
			//	else
			//	{
			//		System.Diagnostics.Debug.WriteLine($"XXX: FocusManager_GettingFocus[OLD_FOCUSED]: Not FrameworkElement instance.");
			//	}
			//}

			if (e.NewFocusedElement is PlatformView newPlatformView)
			{
				if (FocusManagerMapping.TryGetValue(newPlatformView, out ViewHandler? viewHandler))
				{
					System.Diagnostics.Debug.WriteLine($"XXX: FocusManager_GettingFocus[NEW_FOCUSED]: OK!! [platform view: '{newPlatformView.GetType().FullName} ({newPlatformView.GetHashCode()})]");
					// viewHandler.UpdateIsFocused(true, @new: true);
				}
				else
				{
					System.Diagnostics.Debug.WriteLine($"XXX: FocusManager_GettingFocus[NEW_FOCUSED]: Not FrameworkElement instance.");
				}
			}

			System.Diagnostics.Debug.WriteLine($"XXX: < FocusManager_GettingFocus");
		}

		static void FocusManager_GotFocus(object? sender, FocusManagerGotFocusEventArgs e)
		{
			_ = FocusManagerMapping ?? throw new InvalidOperationException($"{nameof(FocusManagerMapping)} should have been set.");

			System.Diagnostics.Debug.WriteLine($"XXX: > #FocusManager#GotFocus(sender:{sender?.GetType().FullName ?? "NULL"} ({sender?.GetHashCode()})," +
				$"correlationId:{e.CorrelationId}," +
				$"newFocusedElement:{e.NewFocusedElement?.GetType().FullName ?? "NULL"} ({e.NewFocusedElement?.GetHashCode()}))");

			if (e.NewFocusedElement is PlatformView platformView)
			{
				if (FocusManagerMapping.TryGetValue(platformView, out ViewHandler? viewHandler))
				{
					System.Diagnostics.Debug.WriteLine($"XXX: FocusManager_GotFocus: OK!! [platform view: {platformView.GetType().FullName} ({platformView.GetHashCode()})]");
					viewHandler.UpdateIsFocused(true, @new: true);
				}
				else
				{
					if (platformView is Button)
					{
						System.Diagnostics.Debug.WriteLine($"XXX: FocusManager_GotFocus[Button]: Not found platform view [platform view: {platformView.GetType().FullName} ({platformView.GetHashCode()})]");
					}
					else
					{
						System.Diagnostics.Debug.WriteLine($"XXX: FocusManager_GotFocus[Other]: Not found platform view [platform view: {platformView.GetType().FullName} ({platformView.GetHashCode()})]");
					}
				}
			}
			else if (e.NewFocusedElement is DependencyObject dependencyObject)
			{
				System.Diagnostics.Debug.WriteLine($"XXX: FocusManager_GotFocus[DEPENDENCY_OBJECT]: {dependencyObject.GetType().FullName} ({dependencyObject.GetHashCode()})");
				int child = 0;

				foreach (FrameworkElement next in FindDescendants<PlatformView>(dependencyObject))
				{
					child++;
					System.Diagnostics.Debug.WriteLine($"XXX: FocusManager_GotFocus[DEP][{child}]: First: {next.GetType().FullName} ({next.GetHashCode()})");

					if (FocusManagerMapping.TryGetValue(next, out ViewHandler? viewHandler))
					{
						System.Diagnostics.Debug.WriteLine($"XXX: FocusManager_GotFocus[DEP][{child}]: FOUND!!!");
						viewHandler.UpdateIsFocused(true, @new: true);
					}
					else
					{
						System.Diagnostics.Debug.WriteLine($"XXX: FocusManager_GotFocus[DEP][{child}]: NOT FOUND FOUND!!!");
					}
				}
			}
			else if (e.NewFocusedElement is null)
			{
				System.Diagnostics.Debug.WriteLine($"XXX: FocusManager_GotFocus: NEW focused element is NULL; {e.NewFocusedElement}");
			}
			else
			{
				System.Diagnostics.Debug.WriteLine($"XXX: FocusManager_GotFocus: Not a platform view but: {e.NewFocusedElement?.GetType().FullName}");
			}

			System.Diagnostics.Debug.WriteLine($"XXX: < FocusManager_GotFocus");
		}

		private static IEnumerable<T> FindDescendants<T>(Microsoft.UI.Xaml.DependencyObject dobj)
			where T : Microsoft.UI.Xaml.DependencyObject
		{
			int count = Microsoft.UI.Xaml.Media.VisualTreeHelper.GetChildrenCount(dobj);
			for (int i = 0; i < count; i++)
			{
				Microsoft.UI.Xaml.DependencyObject element = Microsoft.UI.Xaml.Media.VisualTreeHelper.GetChild(dobj, i);
				if (element is T t)
					yield return t;

				foreach (T descendant in FindDescendants<T>(element))
					yield return descendant;
			}
		}

		static void FocusManager_LostFocus(object? sender, FocusManagerLostFocusEventArgs e)
		{
			System.Diagnostics.Debug.WriteLine($"XXX: > #FocusManager#LostFocus(sender:{sender?.GetType().FullName} ({sender?.GetHashCode()}),correlationId:{e.CorrelationId},oldFocusedElement:{e.OldFocusedElement?.GetType().FullName} ({e.OldFocusedElement?.GetHashCode()}))");
			_ = FocusManagerMapping ?? throw new InvalidOperationException($"{nameof(FocusManagerMapping)} should have been set.");

			if (e.OldFocusedElement is PlatformView platformView)
			{
				if (FocusManagerMapping.TryGetValue(platformView, out ViewHandler? viewHandler))
				{
					System.Diagnostics.Debug.WriteLine($"XXX: FocusManager_LostFocus: OK!!! [platform view: {platformView.GetType().FullName} ({platformView.GetHashCode()})]");
					viewHandler.UpdateIsFocused(false, @new: true);
				}
				else
				{
					System.Diagnostics.Debug.WriteLine($"XXX: FocusManager_LostFocus: Not found FrameworkElement [platform view: {platformView.GetType().FullName} ({platformView.GetHashCode()})]");
				}
			}
			else if (e.OldFocusedElement is DependencyObject dependencyObject)
			{
				foreach (FrameworkElement next in FindDescendants<PlatformView>(dependencyObject))
				{
					System.Diagnostics.Debug.WriteLine($"XXX: FocusManager_LostFocus[DEP]: First: {next.GetType().FullName} ({next.GetHashCode()})");

					if (FocusManagerMapping.TryGetValue(next, out ViewHandler? viewHandler))
					{
						System.Diagnostics.Debug.WriteLine($"XXX: FocusManager_LostFocus[DEP]: FOUND!!!");
						viewHandler.UpdateIsFocused(false, @new: true);
					}
					else
					{
						System.Diagnostics.Debug.WriteLine($"XXX: FocusManager_LostFocus[DEP]: NOT FOUND FOUND!!!");
					}
				}
			}
			else if (e.OldFocusedElement is null)
			{
				System.Diagnostics.Debug.WriteLine($"XXX: FocusManager_LostFocus: OLD focused element is NULL; {e.OldFocusedElement}");
			}
			else
			{
				System.Diagnostics.Debug.WriteLine($"XXX: FocusManager_LostFocus: Not a platform view.");
			}

			System.Diagnostics.Debug.WriteLine($"XXX: < FocusManager_LostFocus");
		}

		void UpdateIsFocused(bool isFocused, bool @new = false)
		{
			if (VirtualView == null)
			{
				return;
			}

			bool updateIsFocused = (isFocused && !VirtualView.IsFocused) || (!isFocused && VirtualView.IsFocused);

			if (updateIsFocused)
			{
				if (@new)
				{
					System.Diagnostics.Debug.WriteLine($"XXX: * UpdateIsFocused [NEW]: VirtualView:{VirtualView.GetType().FullName} isFocused={isFocused} " +
						$"[platformView: {VirtualView.Handler?.PlatformView?.GetType().FullName} ({VirtualView.Handler?.PlatformView?.GetHashCode()})]");
					VirtualView.IsFocused = isFocused;
				}
				else
				{
					System.Diagnostics.Debug.WriteLine($"XXX: * UpdateIsFocused [OLD]: VirtualView:{VirtualView.GetType().FullName} isFocused={isFocused} " +
						$"[platformView: {VirtualView.Handler?.PlatformView?.GetType().FullName} ({VirtualView.Handler?.PlatformView?.GetHashCode()})]");
				}
			}
			else
			{
				System.Diagnostics.Debug.WriteLine($"XXX: * UpdateIsFocused [{(@new ? "NEW" : "OLD")}][NO_CHANGE]: VirtualView:{VirtualView.GetType().FullName} isFocused={isFocused} " +
					$"[platformView: {VirtualView.Handler?.PlatformView?.GetType().FullName} ({VirtualView.Handler?.PlatformView?.GetHashCode()})]");
			}
		}
	}
}