using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.ApplicationModel;
using System.Collections.Generic;

namespace Maui.Controls.Sample
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
			
			MyPicker.ItemsSource = new List<string> { "One", "Two", "Three" };
			MyPicker.SelectedIndex = 1;
		}

		private void ThemeLight_CheckedChanged(object sender, CheckedChangedEventArgs e)
		{
			if (ThemeLight.IsChecked)
			{
				Application.Current!.UserAppTheme = AppTheme.Light;
			}
			else
			{
				Application.Current!.UserAppTheme = AppTheme.Dark;
			}
		}
	}
}