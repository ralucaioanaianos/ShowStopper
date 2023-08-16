using ShowStopper.ViewModels;

namespace ShowStopper.Views;

public partial class FilterEventsPage : ContentPage
{
	public FilterEventsPage()
	{
		InitializeComponent();
		BindingContext = new ExplorePageViewModel(Navigation);
	}
}