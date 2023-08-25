using ShowStopper.Models;
using ShowStopper.ViewModels;

namespace ShowStopper.Views;

public partial class SimilarEventsPage : ContentPage
{
	public SimilarEventsPage(AppEvent currentEvent)
	{
		InitializeComponent();
		BindingContext = new SimilarEventsPageViewModel(Navigation, currentEvent);
	}
}