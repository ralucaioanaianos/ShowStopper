using Firebase.Auth;
using ShowStopper.ViewModels;

namespace ShowStopper.Views;
public partial class MyEventsPage : ContentPage
{
    public MyEventsPage()
    {
		InitializeComponent();
        BindingContext = new MyEventsPageViewModel(Navigation);
    }
}