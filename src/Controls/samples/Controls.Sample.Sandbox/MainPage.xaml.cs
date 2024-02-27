using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Maui.Controls.Sample;

public partial class MainPage : ContentPage
{
	private int i = 0;

	public MainPage()
	{
		Debug.WriteLine(">>> Initializing");
		InitializeComponent();
	
		//Debug.WriteLine(">>> Register DoubleTap1");
		//{
		//	TapGestureRecognizer doubleTapGestureRecognizer = new();
		//	doubleTapGestureRecognizer.Tapped += OnDoubleTap1;
		//	doubleTapGestureRecognizer.NumberOfTapsRequired = 2;
		//	myLabel.GestureRecognizers.Add(doubleTapGestureRecognizer);
		//}

		//Debug.WriteLine(">>> Register Tap1");
		//{
		//	TapGestureRecognizer tapGestureRecognizer = new();
		//	tapGestureRecognizer.Tapped += OnTap1;
		//	myLabel.GestureRecognizers.Add(tapGestureRecognizer);
		//}

		//Debug.WriteLine(">>> Register Secondary Tap");
		//{
		//	TapGestureRecognizer tapGestureRecognizerSecondary = new();
		//	tapGestureRecognizerSecondary.Buttons = ButtonsMask.Secondary;
		//	tapGestureRecognizerSecondary.Tapped += OnSecondaryTap;
		//	myLabel.GestureRecognizers.Add(tapGestureRecognizerSecondary);
		//}

		//Debug.WriteLine(">>> Register Tap2");
		//{
		//	TapGestureRecognizer tapGestureRecognizer = new();
		//	tapGestureRecognizer.Tapped += OnTap2;
		//	myLabel.GestureRecognizers.Add(tapGestureRecognizer);
		//}

		//Debug.WriteLine(">>> Register DoubleTap2");
		//{
		//	TapGestureRecognizer doubleTapGestureRecognizer = new();
		//	doubleTapGestureRecognizer.Tapped += OnDoubleTap2;
		//	doubleTapGestureRecognizer.NumberOfTapsRequired = 2;
		//	myLabel.GestureRecognizers.Add(doubleTapGestureRecognizer);
		//}
	}

	private void OnTap1(object? sender, TappedEventArgs e)
	{
		Debug.WriteLine(">>> OnTap1 Called");
		i++;

		if (i == 2)
		{
			Debug.WriteLine(">>> OnTap1 Called (second)");
		}
	}

	private void OnTap2(object? sender, TappedEventArgs e)
	{
		Debug.WriteLine(">>> OnTap2 Called");
	}

	private void OnSecondaryTap(object? sender, TappedEventArgs e)
	{
		Debug.WriteLine(">>> OnSecondaryTap Called");
	}

	private void OnDoubleTap1(object? sender, TappedEventArgs e)
	{
		Debug.WriteLine(">>> OnDoubleTap1 Called");
	}

	private void OnDoubleTap2(object? sender, TappedEventArgs e)
	{
		Debug.WriteLine(">>> OnDoubleTap2 Called");
	}
}