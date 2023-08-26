using ShowStopper.Models;
using ShowStopper.ViewModels;

namespace ShowStopper.Views;

public partial class GiveReviewPage : ContentPage
{
	public GiveReviewPage(AppLocation location)
	{
		InitializeComponent();
		BindingContext = new GiveReviewPageViewModel(Navigation, location);
	}
}