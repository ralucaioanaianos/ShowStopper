using ShowStopper.ViewModels;

namespace ShowStopper.Views;

public partial class MyTicketsPage : ContentPage
{
	public MyTicketsPage()
	{
		InitializeComponent();
		BindingContext = new MyTicketsPageViewModel(Navigation);
	}
}