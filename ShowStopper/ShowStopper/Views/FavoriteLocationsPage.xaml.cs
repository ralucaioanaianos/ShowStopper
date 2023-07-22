using ShowStopper.ViewModels;

namespace ShowStopper.Views;

public partial class FavoriteLocationsPage : ContentPage
{
	public FavoriteLocationsPage()
	{
		InitializeComponent();
		BindingContext = new FavoriteLocationsPageViewModel(Navigation);
	}
}