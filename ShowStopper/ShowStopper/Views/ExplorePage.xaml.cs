using ShowStopper.Models;
using ShowStopper.ViewModels;
using System.Collections.ObjectModel;

namespace ShowStopper.Views;

public partial class ExplorePage : ContentPage
{
	public ExplorePage()
	{
		InitializeComponent();
		BindingContext = new ExplorePageViewModel(Navigation);
	}

    public ExplorePage(ObservableCollection<AppEvent> eventsList)
    {
        InitializeComponent();
        BindingContext = new ExplorePageViewModel(Navigation, eventsList);

    }

    private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (BindingContext is ExplorePageViewModel viewModel)
        {
            viewModel.UpdateSearchResults(e.NewTextValue);
        }
    }
}