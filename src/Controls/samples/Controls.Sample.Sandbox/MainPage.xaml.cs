using System;
using System.ComponentModel;
using Microsoft.Maui.Controls;

namespace Maui.Controls.Sample
{
	public partial class MainPage : ContentPage
	{
		private bool isEntryVisible;

		public bool IsEntryVisible
		{
			get { return isEntryVisible; }
			set {
				if (isEntryVisible != value)
				{
					isEntryVisible = value;
					OnPropertyChanged();
				}
			}
		}

		public MainPage()
		{
			BindingContext = this;
			InitializeComponent();
		}

		private void SwitchVisibility(object sender, EventArgs e)
		{
			IsEntryVisible = !isEntryVisible;
			myEntry.Focus();
		}

		private void SetFocus(object sender, EventArgs e)
		{
			myEntry.Focus();
		}

		private void myEntry_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (sender is not Entry entry)
				throw new Exception($"Entry expected as a sender, '{sender?.GetType().FullName}' provided.");

			if ((e.PropertyName == nameof(entry.IsVisible)) && entry.IsVisible)
				// this.FocusVisualElement(entry);
				myEntry.Focus();
		}
	}
}