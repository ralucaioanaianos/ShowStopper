namespace ShowStopper.CustomComponents;

[XamlCompilation(XamlCompilationOptions.Compile)]	
public partial class RoundedImage : ContentView
{
	private static readonly BindableProperty ImageSourceProperty
		= BindableProperty.Create(nameof(ImageSource), typeof(string), typeof(RoundedImage));

	public string ImageSource
	{
		get => (string)GetValue(ImageSourceProperty);
		set => SetValue(ImageSourceProperty, value);
	}

    private static readonly BindableProperty RadiusXProperty
        = BindableProperty.Create(nameof(RadiusX), typeof(string), typeof(RoundedImage));

    public string RadiusX
    {
        get => (string)GetValue(RadiusXProperty);
        set => SetValue(RadiusXProperty, value);
    }

    private static readonly BindableProperty RadiusYProperty
        = BindableProperty.Create(nameof(RadiusY), typeof(string), typeof(RoundedImage));

    public string RadiusY
    {
        get => (string)GetValue(RadiusYProperty);
        set => SetValue(RadiusYProperty, value);
    }

    public RoundedImage()
	{
		InitializeComponent();
	}
}