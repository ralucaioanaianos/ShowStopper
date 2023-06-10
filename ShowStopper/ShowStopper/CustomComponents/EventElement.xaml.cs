namespace ShowStopper.CustomComponents;


[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class EventElement : ContentView
{
    private static readonly BindableProperty NameProperty
        = BindableProperty.Create(nameof(Name), typeof(string), typeof(EventElement));

    public string Name
    {
        get => (string)GetValue(NameProperty);
        set => SetValue(NameProperty, value);
    }

    private static readonly BindableProperty DateProperty
        = BindableProperty.Create(nameof(Date), typeof(string), typeof(EventElement));

    public string Date
    {
        get => (string)GetValue(DateProperty);
        set => SetValue(DateProperty, value);
    }

    private static readonly BindableProperty LocationProperty
        = BindableProperty.Create(nameof(Location), typeof(string), typeof(EventElement));

    public string Location
    {
        get => (string)GetValue(LocationProperty);
        set => SetValue(LocationProperty, value);
    }

    public EventElement()
	{
		InitializeComponent();
	}
}