using ShowStopper.ViewModels;

namespace ShowStopper.Views;

public partial class AddLocationPage : ContentPage
{
	public AddLocationPage(Action locationAddedCallback)
	{
		InitializeComponent();
		BindingContext = new AddLocationPageViewModel(Navigation, locationAddedCallback);
	}
}