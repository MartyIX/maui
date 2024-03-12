using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.ApplicationModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maui.Controls.Sample
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();

			Picker.ItemsSource = new List<string> { "One", "Two", "Three" };
			Picker.SelectedIndex = 1;

			App.Current!.UserAppTheme = AppTheme.Light;
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

			Window.Width = 360;
			Window.Height = 700;
		}

		private void ThemeLight_CheckedChanged(object sender, CheckedChangedEventArgs e)
		{
			if (ThemeLight.IsChecked)
			{
				App.Current!.UserAppTheme = AppTheme.Light;
			}
			else
			{
				App.Current!.UserAppTheme = AppTheme.Dark;
			}
		}
	}
}