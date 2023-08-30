using Firebase.Auth;
using ShowStopper.Models;
using ShowStopper.ViewModels;

namespace ShowStopper.Views;

public partial class EditProfilePage : ContentPage
{
	public EditProfilePage(User user, AppUser databaseUser, Action initializeUserAfterEdit)
	{
		InitializeComponent();
        BindingContext = new EditProfilePageViewModel(Navigation, user, databaseUser, initializeUserAfterEdit);
    }
}