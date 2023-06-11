using ShowStopper.Models;
using ShowStopper.ViewModels;

namespace ShowStopper.Views;

public partial class LocationPage : ContentPage
{
    public LocationPage(AppLocation appLocation)
    {
        InitializeComponent();
        BindingContext = new LocationPageViewModel(Navigation, appLocation);
    }
}