using ShowStopper.Views;

namespace ShowStopper;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
		//MainPage = new PaymentPage();
		MainPage = new NavigationPage(new LoginPage("False"));
		//MainPage = new BuyTicketsPage();	
		//MainPage = new PanoramaView();
	}
}
