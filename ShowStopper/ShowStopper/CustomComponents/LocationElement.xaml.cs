namespace ShowStopper.CustomComponents;

public partial class LocationElement : ContentView
{
    private static readonly BindableProperty NameProperty
       = BindableProperty.Create(nameof(Name), typeof(string), typeof(LocationElement));

    public string Name
    {
        get => (string)GetValue(NameProperty);
        set => SetValue(NameProperty, value);
    }

    private static readonly BindableProperty AddressProperty
       = BindableProperty.Create(nameof(Address), typeof(string), typeof(LocationElement));

    public string Address
    {
        get => (string)GetValue(AddressProperty);
        set => SetValue(AddressProperty, value);
    }

    private static readonly BindableProperty OwnerProperty
       = BindableProperty.Create(nameof(Owner), typeof(string), typeof(LocationElement));

    public string Owner
    {
        get => (string)GetValue(AddressProperty);
        set => SetValue(AddressProperty, value);
    }
    public LocationElement()
	{
		InitializeComponent();
	}
}