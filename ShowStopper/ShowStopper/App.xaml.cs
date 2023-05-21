using ShowStopper.Views;

namespace ShowStopper;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new NavigationPage(new LoginPage("False"));
	}
}
