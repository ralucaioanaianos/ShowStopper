using ShowStopper.Models;
using ShowStopper.ViewModels;

namespace ShowStopper.Views;

public partial class FavoriteEventsPage : ContentPage
{
	public FavoriteEventsPage()
	{
		InitializeComponent();
		BindingContext = new FavoriteEventsPageViewModel(Navigation);
	}
}