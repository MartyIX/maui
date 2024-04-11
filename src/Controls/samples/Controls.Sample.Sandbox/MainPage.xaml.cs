using System;
using System.Diagnostics;
using Microsoft.Maui.Controls;

namespace Maui.Controls.Sample;

public partial class MainPage : ContentPage
{
	const int rowCount = 30;
	const int columnCount = 30;

	public MainPage()
	{
		InitializeComponent();
	}

	private void ClearGrid_Clicked(object sender, EventArgs e)
	{
		Stopwatch sw = Stopwatch.StartNew();

		myContentWrapper.Clear();

		sw.Stop();

		info.Text = $"Clearing all content took: {sw.ElapsedMilliseconds} ms";
	}

	private void BatchGenerate_Clicked(object sender, EventArgs e)
	{
		Stopwatch sw = Stopwatch.StartNew();

		int batchSize = int.Parse(BatchSize.Text);
		myContentWrapper.Clear();

		for (int i = 0; i < batchSize; i++)
		{
			Border border = new() { Stroke = Brush.AliceBlue, StrokeThickness = 2, Content = new Label() { Text = $"Test {i}" } };
			myContentWrapper.Add(border);
		}

		sw.Stop();
		info.Text = $"Created {batchSize} borders and it took {sw.ElapsedMilliseconds} ms in total. Avg run took {Math.Round(sw.ElapsedMilliseconds / (double)batchSize, 2)} ms";
	}
}