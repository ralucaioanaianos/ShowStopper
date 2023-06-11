using Firebase.Auth;
using ShowStopper.CustomComponents;
using ShowStopper.Models;
using ShowStopper.Services;
using ShowStopper.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ShowStopper.ViewModels
{
    internal class MyEventsPageViewModel : INotifyPropertyChanged
    {
        public bool IsListEmpty { get; set; }

        public string Name { get; set; } = "Name";

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
            await _navigation.PushAsync(new AddEventPage());
        }

        public MyEventsPageViewModel(INavigation navigation)
        {
            LoadEvents();
            
            _navigation = navigation;
            BackBtn = new Command(BackButtonTappedAsync);
            PlusBtn = new Command(PlusButtonTappedAsync);
            EventTapped = new Command(EventTappedAsync);
            
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

        private async void LoadEvents()
        {
            string email = FirebaseAuthenticationService.GetLoggedUserEmail();
            List<AppEvent> list = await FirebaseDatabaseService.getEventsByEmail(email);
            if (list.Count == 0)
            {
                await Application.Current.MainPage.DisplayAlert("list0", email, "ok");
            }
            ObservableCollection<AppEvent> collection = new ObservableCollection<AppEvent>(list);
            
            Events = collection;
                IsDataLoaded = true;
            await Task.Delay(1000);
            if (Events.Count == 0)
            {
                await Application.Current.MainPage.DisplayAlert("prolr", email, "ok");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        

        private async void EventTappedAsync(object parameter)
        {
            if (parameter is AppEvent selectedEvent)
            {
                await Application.Current.MainPage.DisplayAlert("Event Selected", $"You tapped on {selectedEvent.Name}", "OK");

                // Call your custom method with the selected event
                // Example:
                // DoSomethingWithSelectedEvent(selectedEvent);
            }
        }
    }
}
