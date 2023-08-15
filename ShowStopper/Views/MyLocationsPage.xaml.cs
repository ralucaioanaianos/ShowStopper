using ShowStopper.ViewModels;

namespace ShowStopper.Views;

public partial class MyLocationsPage : ContentPage
{
	public MyLocationsPage()
	{
		InitializeComponent();
		BindingContext = new MyLocationsPageViewModel(Navigation);
	}

    private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (BindingContext is MyLocationsPageViewModel viewModel)
        {
            viewModel.UpdateSearchResults(e.NewTextValue);
        }
    }
}