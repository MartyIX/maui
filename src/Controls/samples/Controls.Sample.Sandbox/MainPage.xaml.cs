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
		private static readonly ImageSource _imageSource = ImageSource.FromFile("circle.png");

		public MainPage()
		{
			InitializeComponent();
		}

		private void Button_Clicked(object sender, EventArgs e)
		{
			Image image = new()
			{
				Source = _imageSource,
				WidthRequest = 82,
				HeightRequest = 77,
			};

			HorizontalStackLayout item = 
			[
				new Label() { Text = "My label"},
				image
			];

			myLayout.Add(item);
		}
	}
}