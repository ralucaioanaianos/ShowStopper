using ShowStopper.ViewModels;
namespace ShowStopper;

public partial class RegisterPage : ContentPage
{
	public RegisterPage()
	{
		InitializeComponent();
        BindingContext = new RegisterPageViewModel(Navigation);
    }
}