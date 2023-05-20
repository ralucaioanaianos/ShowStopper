using ShowStopper.ViewModels; 
namespace ShowStopper;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
		BindingContext = new LoginPageViewModel(Navigation);
	}
}

