namespace Maui.Controls.Sample;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();

		PointerGestureRecognizer pointerGestureRecognizer = new();
		pointerGestureRecognizer.PointerEnteredCommand = new Command(() =>
		{
			button3.Text = "Button 3 (pointer over)";
			button3.BackgroundColor = Colors.Red;
		});

		pointerGestureRecognizer.PointerExitedCommand = new Command(() =>
		{
			button3.Text = "Button 3";
			button3.BackgroundColor = null;
		});

		button3.GestureRecognizers.Add(pointerGestureRecognizer);
	}

	private void PointerGestureRecognizer_PointerEntered(object sender, PointerEventArgs e)
	{
		button2.Text = "Button 2 (pointer over)";
		button2.BackgroundColor = Colors.Red;
	}

	private void PointerGestureRecognizer_PointerExited(object sender, PointerEventArgs e)
	{
		button2.Text = "Button 2";
		button2.BackgroundColor = null;
	}
}
