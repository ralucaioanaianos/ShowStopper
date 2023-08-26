using ShowStopper.Models;
using ShowStopper.ViewModels;

namespace ShowStopper.Views;

public partial class GiveReviewPage : ContentPage
{
	public GiveReviewPage(AppLocation location, Action reviewAddedCallback)
	{
		InitializeComponent();
		BindingContext = new GiveReviewPageViewModel(Navigation, location, reviewAddedCallback);
	}
}