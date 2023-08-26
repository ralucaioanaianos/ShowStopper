using ShowStopper.Models;
using ShowStopper.Services;
using ShowStopper.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowStopper.ViewModels
{
    internal class EventsFromLocationPageViewModel : INotifyPropertyChanged
    {
        public bool IsListEmpty { get; set; }
        public bool IsDataLoaded { get; set; } = false;
        public Command BackBtn { get; }
        public Command PlusBtn { get; }
        public Command EventTapped { get; }
        private ObservableCollection<AppEvent> _events;
        public ObservableCollection<AppEvent> Events
        {
            get { return _events; }
            set
            {
                _events = value;
                OnPropertyChanged(nameof(Events));
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

        public EventsFromLocationPageViewModel(INavigation navigation, AppLocation location)
        {
            LoadEvents();
            _navigation = navigation;
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

        private async Task LoadEvents()
        {
            string email = FirebaseAuthenticationService.GetLoggedUserEmail();
            List<AppEvent> list = await EventsService.getEventsByEmail(email);
            ObservableCollection<AppEvent> collection = new ObservableCollection<AppEvent>(list);
            Events = collection;
            IsDataLoaded = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
