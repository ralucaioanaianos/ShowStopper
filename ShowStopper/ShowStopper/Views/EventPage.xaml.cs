using ShowStopper.Models;
using ShowStopper.ViewModels;

namespace ShowStopper.Views;

public partial class EventPage : ContentPage
{
	public EventPage(AppEvent appEvent)
	{
		InitializeComponent();
        BindingContext = new EventPageViewModel(Navigation, appEvent);
    }
}