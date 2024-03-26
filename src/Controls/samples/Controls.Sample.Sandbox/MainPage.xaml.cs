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
		public MainPage()
		{
			InitializeComponent();
		}

		private void DragGestureRecognizer_DragStarting(object? sender, DragStartingEventArgs e)
		{

		}

		private void Button_Clicked(object sender, EventArgs e)
		{
			for (int i = 0; i < 2000; i++)
			{
				// Add the following XAML.
				/*
					<Rectangle Stroke="Red" Fill="DarkBlue" StrokeThickness="4" HeightRequest="200" WidthRequest="200">
						<Rectangle.GestureRecognizers>
							<DragGestureRecognizer DragStarting="DragGestureRecognizer_DragStarting" />
						</Rectangle.GestureRecognizers>
					</Rectangle>
				 */

				Microsoft.Maui.Controls.Shapes.Rectangle rectangle = new()
				{
					Fill = Colors.DarkBlue,
					Stroke = Colors.Red,
					StrokeThickness = 4,
					WidthRequest = 200,
					HeightRequest = 200,
				};

				DragGestureRecognizer recognizer = new();
				recognizer.DragStarting += DragGestureRecognizer_DragStarting;

				rectangle.GestureRecognizers.Add(recognizer);

				myLayout.Add(rectangle);
			}
		}
	}
}