﻿using System;
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
		public MainPage()
		{
			InitializeComponent();
		}

		private void Button_Clicked(object sender, EventArgs e)
		{
			Image image = new()
			{
				Source = ImageSource.FromFile("groceries.png"),
				WidthRequest = 512,
				HeightRequest = 512,
			};

			myLayout.Add(image);
		}
	}
}