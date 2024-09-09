using System.Threading.Tasks;
using Microsoft.Maui.DeviceTests.Stubs;
using Microsoft.Maui.Hosting;
using Xunit;

namespace Microsoft.Maui.DeviceTests
{
	public abstract partial class FocusHandlerTests<THandler, TStub, TLayoutStub> : HandlerTestBasement<THandler, TStub>
		where THandler : class, IViewHandler, new()
		where TStub : IStubBase, new()
		where TLayoutStub : IStubBase, ILayout, new()
	{
		[Fact]
		public async Task FocusAndIsFocusedIsWorking()
		{
			EnsureHandlerCreated(builder =>
			{
				builder.ConfigureMauiHandlers(handler =>
				{
					handler.AddHandler<TLayoutStub, LayoutHandler>();
					handler.AddHandler<TStub, THandler>();
				});
			});

			// create layout with 2 elements
			TStub inputControl1;
			TStub inputControl2;
			var layout = new TLayoutStub
			{
				(inputControl1 = new TStub { Width = 100, Height = 50, AutomationId = "TestControl1" }),
				(inputControl2 = new TStub { Width = 100, Height = 50, AutomationId = "TestControl2" })
			};
			layout.Width = 100;
			layout.Height = 150;

			await AttachAndRun<LayoutHandler>(layout, async (contentViewHandler) =>
			{
				var platform1 = inputControl1.ToPlatform();
				var platform2 = inputControl2.ToPlatform();

				// focus the first control
				var result1 = inputControl1.Handler.InvokeWithResult(nameof(IView.Focus), new FocusRequest());
				Assert.True(result1);

				// assert
				await inputControl1.WaitForFocused();
				Assert.True(inputControl1.IsFocused);
				Assert.False(inputControl2.IsFocused);

				// focus the second control
				var result2 = inputControl2.Handler.InvokeWithResult(nameof(IView.Focus), new FocusRequest());
				Assert.True(result2);

				// assert
				await inputControl2.WaitForFocused();
				Assert.False(inputControl1.IsFocused);
				Assert.True(inputControl2.IsFocused);
			});
		}

		// TODO: Android is not unfocusing
#if IOS || MACCATALYST || WINDOWS
		[Fact]
		public async Task UnfocusAndIsFocusedIsWorking()
		{
			System.Diagnostics.Debug.WriteLine($"XXX: [TEST] UnfocusAndIsFocusedIsWorking STARTS!");

			EnsureHandlerCreated(builder =>
			{
				builder.ConfigureMauiHandlers(handler =>
				{
					handler.AddHandler<TLayoutStub, LayoutHandler>();
					handler.AddHandler<TStub, THandler>();
				});
			});

			// create layout with 2 elements
			TStub inputControl1;
			TStub inputControl2;
			var layout = new TLayoutStub
			{
				(inputControl1 = new TStub { Width = 100, Height = 50 }),
				(inputControl2 = new TStub { Width = 100, Height = 50 })
			};
			layout.Width = 100;
			layout.Height = 150;

			await AttachAndRun<LayoutHandler>(layout, async (contentViewHandler) =>
			{
				var platform1 = inputControl1.ToPlatform();
				var platform2 = inputControl2.ToPlatform();

				// focus the first control
				var result1 = inputControl1.Handler.InvokeWithResult(nameof(IView.Focus), new FocusRequest());
				Assert.True(result1);

				// assert
				System.Diagnostics.Debug.WriteLine($"XXX: #TEST: WaitForFocused (HC: {inputControl1.Handler?.PlatformView?.GetHashCode()})");
				await inputControl1.WaitForFocused();

				System.Diagnostics.Debug.WriteLine($"XXX: #TEST: Assert#1: inputControl1.IsFocused (HC: {inputControl1.Handler?.PlatformView?.GetHashCode()})");
				Assert.True(inputControl1.IsFocused);

				System.Diagnostics.Debug.WriteLine($"XXX: #TEST: Assert#2: !inputControl2.IsFocused (HC: {inputControl2.Handler?.PlatformView?.GetHashCode()})");
				Assert.False(inputControl2.IsFocused);

				// UNfocus the first control (revert the focus)
				System.Diagnostics.Debug.WriteLine($"XXX: #TEST: Unfocus inputControl1! (HC: {inputControl1.Handler?.PlatformView?.GetHashCode()})");
				inputControl1.Handler.Invoke(nameof(IView.Unfocus));

				// assert
				System.Diagnostics.Debug.WriteLine($"XXX: #TEST: Wait until inputControl1 is not focused (HC: {inputControl1.Handler?.PlatformView?.GetHashCode()})");
				await inputControl1.WaitForUnFocused();

				System.Diagnostics.Debug.WriteLine($"XXX: #TEST: Assert#3: !inputControl1.IsFocused (HC: {inputControl1.Handler?.PlatformView?.GetHashCode()})");
				Assert.False(inputControl1.IsFocused);

				System.Diagnostics.Debug.WriteLine($"XXX: #TEST: Wait until inputControl2 is not focused (HC: {inputControl2.Handler?.PlatformView?.GetHashCode()})");
				await inputControl2.WaitForUnFocused();

				System.Diagnostics.Debug.WriteLine($"XXX: #TEST: Assert#4: !inputControl2.IsFocused (HC: {inputControl2.Handler?.PlatformView?.GetHashCode()})");
				Assert.False(inputControl2.IsFocused);

				System.Diagnostics.Debug.WriteLine($"XXX: #TEST: DONE!");
			});
		}
#endif
	}
}