using Firebase.Auth;
using ShowStopper.ViewModels;

namespace ShowStopper.Views;

public partial class AddEventPage : ContentPage
{
    public AddEventPage()
    {
		InitializeComponent();
        BindingContext = new AddEventPageViewModel(Navigation);
    }
}