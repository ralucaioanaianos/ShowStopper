using System.Windows.Input;

namespace ShowStopper.CustomComponents;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class CustomNavigationBar : ContentView
{
    public static readonly BindableProperty PageNameProperty =
    BindableProperty.Create(nameof(PageName), typeof(string), typeof(CustomNavigationBar));

    public string PageName
    {
        get => (string)GetValue(PageNameProperty);
        set => SetValue(PageNameProperty, value);
    }

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

    private static readonly BindableProperty IsEmptyHeartButtonVisibleProperty
           = BindableProperty.Create(nameof(IsEmptyHeartButtonVisible), typeof(string), typeof(CustomNavigationBar));

    public string IsEmptyHeartButtonVisible
    {
        get => (string)GetValue(IsEmptyHeartButtonVisibleProperty);
        set => SetValue(IsEmptyHeartButtonVisibleProperty, value);
    }

    private static readonly BindableProperty IsFullHeartButtonVisibleProperty
           = BindableProperty.Create(nameof(IsFullHeartButtonVisible), typeof(string), typeof(CustomNavigationBar));

    public string IsFullHeartButtonVisible
    {
        get => (string)GetValue(IsFullHeartButtonVisibleProperty);
        set => SetValue(IsFullHeartButtonVisibleProperty, value);
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

    public Command PlusBtn { get; }

    public CustomNavigationBar()
    {
        InitializeComponent();
    }

    public static readonly BindableProperty BackBtnTapCommandProperty = BindableProperty.Create(
            nameof(BackBtnTapCommand),
            typeof(ICommand),
            typeof(CustomNavigationBar));

    public ICommand BackBtnTapCommand
    {
        get => (ICommand)GetValue(BackBtnTapCommandProperty);
        set => SetValue(BackBtnTapCommandProperty, value);
    }

    public static readonly BindableProperty BackBtnTapCommandParameterProperty = BindableProperty.Create(
        nameof(BackBtnTapCommandParameter),
        typeof(object),
        typeof(CustomNavigationBar));

    public object BackBtnTapCommandParameter
    {
        get => GetValue(BackBtnTapCommandParameterProperty);
        set => SetValue(BackBtnTapCommandParameterProperty, value);
    }

    public static readonly BindableProperty PlusBtnTapCommandProperty = BindableProperty.Create(
           nameof(PlusBtnTapCommand),
           typeof(ICommand),
           typeof(CustomNavigationBar));

    public ICommand PlusBtnTapCommand
    {
        get => (ICommand)GetValue(PlusBtnTapCommandProperty);
        set => SetValue(PlusBtnTapCommandProperty, value);
    }

    public static readonly BindableProperty PlusBtnTapCommandParameterProperty = BindableProperty.Create(
        nameof(PlusBtnTapCommandParameter),
        typeof(object),
        typeof(CustomNavigationBar));

    public object PlusBtnTapCommandParameter
    {
        get => GetValue(PlusBtnTapCommandParameterProperty);
        set => SetValue(PlusBtnTapCommandParameterProperty, value);
    }

    ////////////////////////////
    ///

    public static readonly BindableProperty EmptyHeartBtnTapCommandProperty = BindableProperty.Create(
           nameof(EmptyHeartBtnTapCommand),
           typeof(ICommand),
           typeof(CustomNavigationBar));

    public ICommand EmptyHeartBtnTapCommand
    {
        get => (ICommand)GetValue(EmptyHeartBtnTapCommandProperty);
        set => SetValue(EmptyHeartBtnTapCommandProperty, value);
    }

    public static readonly BindableProperty EmptyHeartBtnTapCommandParameterProperty = BindableProperty.Create(
        nameof(EmptyHeartBtnTapCommandParameter),
        typeof(object),
        typeof(CustomNavigationBar));

    public object EmptyHeartBtnTapCommandParameter
    {
        get => GetValue(EmptyHeartBtnTapCommandParameterProperty);
        set => SetValue(EmptyHeartBtnTapCommandParameterProperty, value);
    }

    ///////
    ///

    public static readonly BindableProperty FullHeartBtnTapCommandProperty = BindableProperty.Create(
           nameof(FullHeartBtnTapCommand),
           typeof(ICommand),
           typeof(CustomNavigationBar));

    public ICommand FullHeartBtnTapCommand
    {
        get => (ICommand)GetValue(FullHeartBtnTapCommandProperty);
        set => SetValue(FullHeartBtnTapCommandProperty, value);
    }

    public static readonly BindableProperty FullHeartBtnTapCommandParameterProperty = BindableProperty.Create(
        nameof(FullHeartBtnTapCommandParameter),
        typeof(object),
        typeof(CustomNavigationBar));

    public object FullHeartBtnTapCommandParameter
    {
        get => GetValue(FullHeartBtnTapCommandParameterProperty);
        set => SetValue(FullHeartBtnTapCommandParameterProperty, value);
    }
}