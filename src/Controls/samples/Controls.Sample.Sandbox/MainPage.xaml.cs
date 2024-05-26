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
		private int i = 0;

		public MainPage()
		{
			InitializeComponent();
		}

		private void Button_Clicked(object sender, EventArgs e)
		{
			i++;
			if (i % 2 == 0)
			{
				myLabel.TextDecorations = TextDecorations.Underline | TextDecorations.Strikethrough;
			}
			else
			{
				myLabel.TextDecorations = TextDecorations.None;
			}
		}
    }
}