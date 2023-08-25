using ShowStopper.ViewModels;

namespace ShowStopper.Views;

public partial class ExplorePage : ContentPage
{
	public ExplorePage()
	{
		InitializeComponent();
		BindingContext = new ExplorePageViewModel(Navigation);
	}

    private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (BindingContext is ExplorePageViewModel viewModel)
        {
            viewModel.UpdateSearchResults(e.NewTextValue);
        }
    }
}