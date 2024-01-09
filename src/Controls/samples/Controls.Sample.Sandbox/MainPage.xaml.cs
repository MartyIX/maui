using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Maui.Controls.Sample
{
	public partial class MainPage : ContentPage
	{
		/// <summary>Command to initiate the start of a new script task run with the same parameters as the previous run.</summary>
		public Command ClickCommand { get; }

		public MainPage()
		{
			ClickCommand = new(execute: () => {
				DisplayAlert("Title", "Button 1 clicked", "Cancel");
			});

			InitializeComponent();
			BindingContext = this;
		}

		private void button2_Clicked(object sender, EventArgs e)
		{
			DisplayAlert("Title", "Button 2 clicked", "Cancel");
		}
	}
}