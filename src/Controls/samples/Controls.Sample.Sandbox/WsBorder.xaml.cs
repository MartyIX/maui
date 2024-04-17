using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Maui.Controls.Sample.Sandbox;

public partial class WsBorder : ContentView
{
	/// <summary>Bindable property for the color of the left side.</summary>
	public static readonly BindableProperty BorderColorLeftProperty = BindableProperty.Create(propertyName: nameof(BorderColorLeft), returnType: typeof(Color),
		declaringType: typeof(WsBorder), defaultBindingMode: BindingMode.OneWay);

	/// <summary>Bindable property for the color of the top side.</summary>
	public static readonly BindableProperty BorderColorTopProperty = BindableProperty.Create(propertyName: nameof(BorderColorTop), returnType: typeof(Color),
		declaringType: typeof(WsBorder), defaultBindingMode: BindingMode.OneWay);

	/// <summary>Bindable property for the color of the right side.</summary>
	public static readonly BindableProperty BorderColorRightProperty = BindableProperty.Create(propertyName: nameof(BorderColorRight), returnType: typeof(Color),
		declaringType: typeof(WsBorder), defaultBindingMode: BindingMode.OneWay);

	/// <summary>Bindable property for the color of the bottom side.</summary>
	public static readonly BindableProperty BorderColorBottomProperty = BindableProperty.Create(propertyName: nameof(BorderColorBottom), returnType: typeof(Color),
		declaringType: typeof(WsBorder), defaultBindingMode: BindingMode.OneWay);

	/// <summary>Bindable property for the width of the border.</summary>
	public static readonly BindableProperty BorderWidthProperty = BindableProperty.Create(propertyName: nameof(BorderWidth), returnType: typeof(Thickness),
		declaringType: typeof(WsBorder), defaultBindingMode: BindingMode.OneWay);

	/// <summary>Bindable property for the padding of the content inside of the border.</summary>
	public static new readonly BindableProperty PaddingProperty = BindableProperty.Create(propertyName: nameof(Padding), returnType: typeof(Thickness),
		declaringType: typeof(WsBorder), defaultBindingMode: BindingMode.OneWay);

	/// <summary>Color of the left side.</summary>
	public Color BorderColorLeft
	{
		get => (Color)GetValue(BorderColorLeftProperty);
		set => SetValue(BorderColorLeftProperty, value);
	}

	/// <summary>Color of the right side.</summary>
	public Color BorderColorRight
	{
		get => (Color)GetValue(BorderColorRightProperty);
		set => SetValue(BorderColorRightProperty, value);
	}

	/// <summary>Color of the top side.</summary>
	public Color BorderColorTop
	{
		get => (Color)GetValue(BorderColorTopProperty);
		set => SetValue(BorderColorTopProperty, value);
	}

	/// <summary>Color of bottom bottom side.</summary>
	public Color BorderColorBottom
	{
		get => (Color)GetValue(BorderColorBottomProperty);
		set => SetValue(BorderColorBottomProperty, value);
	}

	/// <summary>Width of the border.</summary>
	public Thickness BorderWidth
	{
		get => (Thickness)GetValue(BorderWidthProperty);
		set => SetValue(BorderWidthProperty, value);
	}

	/// <summary>Padding of the content inside of the border.</summary>
	public new Thickness Padding
	{
		get => (Thickness)GetValue(PaddingProperty);
		set => SetValue(PaddingProperty, value);
	}

	/// <summary>
	/// Creates a new instance of the object.
	/// </summary>
	public WsBorder() : base()
	{
		InitializeComponent();
	}

	// Uncomment these lines to fix the crash when one clicks the "Button wrapped in custom control border control" button.
	/// <inheritdoc/>
	protected override void OnBindingContextChanged()
	{
		// Make sure that WsBorder is not a parent of the content. WsBorder is purely a visual adorner.
		Content.Parent = Parent;
		base.OnBindingContextChanged();
	}
}
