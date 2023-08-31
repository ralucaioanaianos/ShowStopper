using ShowStopper.Models;
using ShowStopper.Services;
using ShowStopper.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ShowStopper.ViewModels
{
    class FavoriteEventsPageViewModel : INotifyPropertyChanged
    {
        public bool IsListEmpty { get; set; }
        public bool IsDataLoaded { get; set; } = false;
        public Command BackBtn { get; }
        public Command PlusBtn { get; }

        public Command EventTapped { get; }
        private ObservableCollection<AppEvent> _favoriteEvents;
        public ObservableCollection<AppEvent> FavoriteEvents
        {
            get { return _favoriteEvents; }
            set
            {
                _favoriteEvents = value;
                OnPropertyChanged(nameof(FavoriteEvents));
            }
        }

        private INavigation _navigation;

        private async void BackButtonTappedAsync(object parameter)
        {
            await _navigation.PopAsync();
        }

        private async void PlusButtonTappedAsync(object parameter)
        {
        }

        public FavoriteEventsPageViewModel(INavigation navigation)
        {
            _navigation = navigation;
            LoadEvents();
            BackBtn = new Command(BackButtonTappedAsync);
            PlusBtn = new Command(PlusButtonTappedAsync);
            //EventTapped = new Command(EventTappedAsync);
        }

        private AppEvent _selectedEvent;
        public AppEvent SelectedEvent
        {
            get { return _selectedEvent; }
            set
            {
                _selectedEvent = value;
                OnPropertyChanged(nameof(SelectedEvent));
                OnEventSelected();
            }
        }

        private async void OnEventSelected()
        {
            if (SelectedEvent != null)
            {
                await _navigation.PushAsync(new EventPage(SelectedEvent));
                SelectedEvent = null;
            }
        }                           

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async Task LoadEvents()
        {
            try
            {
                string email = FirebaseAuthenticationService.GetLoggedUserEmail();
                
                List<EventFavorite> favoritesList = await EventsService.GetFavoriteEventsByEmail(email);
                if (favoritesList.Count == 0)
                {
                    await Application.Current.MainPage.DisplayAlert("list0", email, "ok");
                }
                ObservableCollection<AppEvent> collection = new ObservableCollection<AppEvent>();
                foreach (EventFavorite favorite in favoritesList)
                {
                    AppEvent appEvent = await EventsService.GetEventByName(favorite.EventName);
                    collection.Add(appEvent);
                }
                FavoriteEvents = collection;
                IsDataLoaded = true;
                await Task.Delay(1000);
                if (FavoriteEvents.Count == 0)
                {
                    await Application.Current.MainPage.DisplayAlert("prolr", email, "ok");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("LoadLocations", ex.Message, "ok");
            }

        }

        private async void EventTappedAsync(object parameter)
        {
            if (parameter is AppEvent selectedEvent)
            {
                await Application.Current.MainPage.DisplayAlert("Location Selected", $"You tapped on {selectedEvent.Name}", "OK");

                // Call your custom method with the selected event
                // Example:
                // DoSomethingWithSelectedEvent(selectedEvent);
            }
        }
    }
}
