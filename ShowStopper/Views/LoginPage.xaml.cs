using ShowStopper.ViewModels; 
namespace ShowStopper;

public partial class LoginPage : ContentPage
{
	public LoginPage(string isVisible)
	{
		InitializeComponent();
		BindingContext = new LoginPageViewModel(Navigation, isVisible);
	}
}

