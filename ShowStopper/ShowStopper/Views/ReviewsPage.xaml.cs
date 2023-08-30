using ShowStopper.Models;
using ShowStopper.ViewModels;

namespace ShowStopper.Views;

public partial class ReviewsPage : ContentPage
{
	public ReviewsPage(AppLocation location, Action locationReviewedCallback)
	{
		InitializeComponent();
        BindingContext = new ReviewsPageViewModel(Navigation, location, locationReviewedCallback);
	}
}