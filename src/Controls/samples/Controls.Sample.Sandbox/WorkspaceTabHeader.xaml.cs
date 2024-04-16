using Microsoft.Maui.Controls;

namespace Maui.Controls.Sample.Sandbox;

public partial class WorkspaceTabHeader : TemplatedView
{
	public static readonly BindableProperty TitleProperty = BindableProperty.Create(propertyName: nameof(Title), returnType: typeof(string), declaringType: typeof(WorkspaceTabHeader),
		defaultValue: default(string), defaultBindingMode: BindingMode.TwoWay);

	public string Title
	{
		get => (string)GetValue(TitleProperty);
		set => SetValue(TitleProperty, value);
	}

	public WorkspaceTabHeader()
	{
		InitializeComponent();
	}
}