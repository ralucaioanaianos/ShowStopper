using ShowStopper.ViewModels;

namespace ShowStopper.Views;

public partial class ExplorePage : ContentPage
{
	public ExplorePage()
	{
		InitializeComponent();
		BindingContext = new ExplorePageViewModel(Navigation);
	}
}