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

    private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (BindingContext is MyEventsPageViewModel viewModel)
        {
            viewModel.UpdateSearchResults(e.NewTextValue);
        }
    }
}