using ShowStopper.Models;
using ShowStopper.ViewModels;

namespace ShowStopper.Views;

public partial class ReviewsPage : ContentPage
{
	public ReviewsPage(AppLocation location)
	{
		InitializeComponent();
        BindingContext = new ReviewsPageViewModel(Navigation, location);
	}
}