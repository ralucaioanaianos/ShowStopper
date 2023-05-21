using ShowStopper.ViewModels;

namespace ShowStopper.Views;

public partial class ProfilePage : ContentPage
{
	public ProfilePage()
	{
		InitializeComponent();
        BindingContext = new ProfilePageViewModel(Navigation);
    }

}