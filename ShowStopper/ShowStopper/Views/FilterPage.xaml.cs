using ShowStopper.ViewModels;

namespace ShowStopper.Views;

public partial class FilterPage : ContentPage
{
	public FilterPage()
	{
		InitializeComponent();
		BindingContext = new ExplorePageViewModel(Navigation);
    }
}