using NUnit.Framework;
using UITest.Appium;
using UITest.Core;
using System.Drawing;

namespace Microsoft.Maui.AppiumTests.Issues
{
	public class Issue19733 : _IssuesUITest
	{
		public Issue19733(TestDevice device) : base(device) { }

		public override string Issue => "No unintended focus change occurs";

		[Test]
		public async Task NoUnintendedFocusShouldOccur()
		{
			_ = App.WaitForElement("Entry3");

			App.Click("Entry3");
			App.Click("Label3");

			// Wait if there is a focus change or not.
			await Task.Delay(2000);

			App.WaitForElement("Success");
		}
	}
}