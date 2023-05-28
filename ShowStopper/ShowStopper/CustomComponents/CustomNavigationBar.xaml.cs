namespace ShowStopper.CustomComponents;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class CustomNavigationBar : ContentView
{
    private static readonly BindableProperty IsBackButtonVisibleProperty
           = BindableProperty.Create(nameof(IsBackButtonVisible), typeof(string), typeof(CustomNavigationBar));

    public string IsBackButtonVisible
    {
        get => (string)GetValue(IsBackButtonVisibleProperty);
        set => SetValue(IsBackButtonVisibleProperty, value);
    }

    private static readonly BindableProperty IsPlusButtonVisibleProperty
          = BindableProperty.Create(nameof(IsPlusButtonVisible), typeof(string), typeof(CustomNavigationBar));

    public string IsPlusButtonVisible
    {
        get => (string)GetValue(IsPlusButtonVisibleProperty);
        set => SetValue(IsPlusButtonVisibleProperty, value);
    }


    private static readonly BindableProperty GradientColor1Property
           = BindableProperty.Create(nameof(GradientColor1), typeof(Color), typeof(CustomNavigationBar));

    public Color GradientColor1
    {
        get => (Color)GetValue(GradientColor1Property);
        set => SetValue(GradientColor1Property, value);
    }

    private static readonly BindableProperty GradientColor2Property
           = BindableProperty.Create(nameof(GradientColor2), typeof(Color), typeof(CustomNavigationBar));

    public Color GradientColor2
    {
        get => (Color)GetValue(GradientColor2Property);
        set => SetValue(GradientColor2Property, value);
    }

    public Command BackBtn { get; }

    private async void BackBtnTappedAsync(object parameter)
    {
        //await _navigation.PopAsync();
    }

    public Command PlusBtn { get; }

    private async void PlusBtnTappedAsync(object parameter)
    {
        //await _navigation.PopAsync();
    }

    public CustomNavigationBar()
    {
        InitializeComponent();
        BackBtn = new Command(BackBtnTappedAsync);
        PlusBtn = new Command(PlusBtnTappedAsync);
    }
}