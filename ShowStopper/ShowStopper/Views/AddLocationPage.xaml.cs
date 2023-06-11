using ShowStopper.ViewModels;

namespace ShowStopper.Views;

public partial class AddLocationPage : ContentPage
{
	public AddLocationPage()
	{
		InitializeComponent();
		BindingContext = new AddLocationPageViewModel(Navigation);
	}
}