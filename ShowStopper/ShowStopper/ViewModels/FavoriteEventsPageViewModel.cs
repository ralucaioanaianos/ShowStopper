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
                ObservableCollection<AppEvent> collection = new ObservableCollection<AppEvent>();
                foreach (EventFavorite favorite in favoritesList)
                {
                    AppEvent appEvent = await EventsService.GetEventByName(favorite.EventName);
                    collection.Add(appEvent);
                }
                FavoriteEvents = collection;
                IsDataLoaded = true;
                await Task.Delay(1000);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    }
}
