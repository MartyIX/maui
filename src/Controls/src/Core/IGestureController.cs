#nullable disable
using System.Collections.Generic;
using Microsoft.Maui.Graphics;

namespace Microsoft.Maui.Controls.Internals
{
	public interface IGestureController
	{
		IList<GestureElement> GetChildElements(Point point);

		// XXX-GESTURE-STORAGE: Note that this is not an observable collection so
		// we cannot subscribe for changes.
		IList<IGestureRecognizer> CompositeGestureRecognizers { get; }
	}
}