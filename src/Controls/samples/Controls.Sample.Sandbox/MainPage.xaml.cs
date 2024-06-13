using System;
using System.Diagnostics;
using System.Threading;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Maui;
using Microsoft.Maui.Storage;
using Microsoft.Maui.Controls;
using System.IO;

namespace Maui.Controls.Sample;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}

	private async void OnOpenClickedAsync(object sender, EventArgs e)
	{
		Stopwatch sw = Stopwatch.StartNew();

		IEnumerable<FileResult> filePickerResults = await XxxPickFilesAsync();

		if (filePickerResults == null || !filePickerResults.Any())
		{
			await DisplayAlert("Notification", "No files were selected.", "OK");
			return;
		}

		XxxReadFiles(filePickerResults);

		sw.Stop();
		await DisplayAlert("Notification", $"All files were opened and closed successfully: {sw.ElapsedMilliseconds} ms", "OK");
	}

	private void XxxReadFiles(IEnumerable<FileResult> filePickerResults)
	{
		foreach (var result in filePickerResults)
		{
			XxxReadSingleFile(result);
		}
	}

	private void XxxReadSingleFile(FileResult result)
	{
		string s = File.ReadAllText(result.FullPath);
		Debug.WriteLine(s);
	}

	private async Task<IEnumerable<FileResult>> XxxPickFilesAsync()
	{
		return await FilePicker.PickMultipleAsync();
	}

	private void Button_Clicked(object sender, EventArgs e)
	{
		const int FileCount = 1000;     // I made this 1000, but I found typically around 200 will do
		const string folder = "c:\\textfiles";

		if (Directory.Exists(folder) && !Directory.EnumerateFileSystemEntries(folder).Any())
		{
			for (var i = 0; i < FileCount; i++)
			{
				var filename = i.ToString("D4") + ".txt";
				var fullPath = folder + "/" + filename;

				using (var streamWriter = new StreamWriter(fullPath))
				{
					streamWriter.WriteLine("test file");
					streamWriter.Close();
				}
			}
		}
	}
}
