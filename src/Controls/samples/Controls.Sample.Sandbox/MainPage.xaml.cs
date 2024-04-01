using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Maui.Controls;

namespace Maui.Controls.Sample;

public partial class MainPage : ContentPage
{
	private ObservableCollection<string> _cardNames = new();

	public ObservableCollection<string> CardNames
	{
		get => _cardNames;
		set => _cardNames = value;
	}

	public MainPage()
	{
		BindingContext = this;
		InitializeComponent();

		foreach (var card in Enumerable.Range(1, 5))
		{
			_cardNames.Add(card.ToString());
		}
	}
}