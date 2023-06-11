using ShowStopper.ViewModels;

namespace ShowStopper.Views;

public partial class MyLocationsPage : ContentPage
{
	public MyLocationsPage()
	{
		InitializeComponent();
		BindingContext = new MyLocationsPageViewModel(Navigation);
	}
}