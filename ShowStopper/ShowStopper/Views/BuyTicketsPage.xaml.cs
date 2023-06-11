using ShowStopper.ViewModels;

namespace ShowStopper.Views;

public partial class BuyTicketsPage : ContentPage
{
	public BuyTicketsPage()
	{
		InitializeComponent();
		BindingContext = new BuyTicketsPageViewModel();
	}
}