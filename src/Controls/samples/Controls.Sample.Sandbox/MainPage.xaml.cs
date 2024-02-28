using System.Diagnostics;
using Microsoft.Maui.Controls;

namespace Maui.Controls.Sample;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		Debug.WriteLine(">>> Initializing");
		InitializeComponent();

		Debug.WriteLine(">>> Register DoubleTap1");
		{
			TapGestureRecognizer doubleTapGestureRecognizer = new();
			doubleTapGestureRecognizer.Tapped += OnDoubleTap1;
			doubleTapGestureRecognizer.NumberOfTapsRequired = 2;
			myLabel.GestureRecognizers.Add(doubleTapGestureRecognizer);
		}

		Debug.WriteLine(">>> Initialized");
	}

	private void OnTap1(object? sender, TappedEventArgs e)
	{
		Debug.WriteLine(">>> OnTap1 Called");
	}

	private void OnDoubleTap1(object? sender, TappedEventArgs e)
	{
		Debug.WriteLine(">>> OnDoubleTap1 Called");
	}

	private void AddNewTapHandler(object? sender, TappedEventArgs e)
	{
		Debug.WriteLine(">>> AddNewTapHandler Called");

		TapGestureRecognizer tapGestureRecognizer = new();
		tapGestureRecognizer.Tapped += OnTap1;
		myLabel.GestureRecognizers.Add(tapGestureRecognizer);
	}

	private void AddNewDoubleTapHandler(object? sender, TappedEventArgs e)
	{
		Debug.WriteLine(">>> AddNewDoubleTapHandler Called");

		TapGestureRecognizer doubleTapGestureRecognizer = new();
		doubleTapGestureRecognizer.Tapped += OnDoubleTap1;
		doubleTapGestureRecognizer.NumberOfTapsRequired = 2;
		myLabel.GestureRecognizers.Add(doubleTapGestureRecognizer);
	}

}