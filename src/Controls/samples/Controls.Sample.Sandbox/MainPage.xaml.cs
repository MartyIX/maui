using System;
using System.Diagnostics;
using System.Text;
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

		contentGrid.Clear();

		sw.Stop();

		info.Text = $"Clearing grid took: {sw.ElapsedMilliseconds} ms";
	}

	private void Button_Clicked(object sender, EventArgs e)
	{
		Stopwatch sw = Stopwatch.StartNew();
		contentGrid.Clear();

		for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
		{
			for (int columnIndex = 0; columnIndex < columnCount; columnIndex++)
			{
				Label label = new Label() { Text = $"[{columnIndex}x{rowIndex}]" };
				contentGrid.Add(label, column: columnIndex, row: rowIndex);
			}
		}

		sw.Stop();
		info.Text = $"Grid was created in: {sw.ElapsedMilliseconds} ms";
	}

	private async void BatchGenerate_ClickedAsync(object sender, EventArgs e)
	{
		Stopwatch sw = Stopwatch.StartNew();

		long sumMs = 0;
		int batchSize = int.Parse(BatchSize.Text);
		int measuredRuns = 0;

		for (int i = 0; i < batchSize; i++)
		{
			long runMs;

			sw.Reset();
			sw.Start();

			contentGrid.Clear();

			for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
			{
				for (int columnIndex = 0; columnIndex < columnCount; columnIndex++)
				{
					Label label = new Label() { Text = $"[{columnIndex}x{rowIndex}]" };
					contentGrid.Add(label, column: columnIndex, row: rowIndex);
				}
			}

			sw.Stop();
			runMs = sw.ElapsedMilliseconds;
			int runs = i + 1;

			if (i >= 10)
			{
				sumMs += runMs;
				measuredRuns++;
				info.Text = $"Grid was created {runs} times and it took {sumMs} ms in total. Avg run took {Math.Round(sumMs / (double)measuredRuns, 2)} ms";
			}
			else
			{
				info.Text = $"Run #{runs} is ignored. The run took {runMs} ms.";
			}

			await Task.Delay(1000).ConfigureAwait(true);
		}
	}

}