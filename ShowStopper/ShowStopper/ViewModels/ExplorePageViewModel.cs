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
using System.Windows.Input;

namespace ShowStopper.ViewModels
{
    internal class ExplorePageViewModel : INotifyPropertyChanged
    {
        private bool _isShowingLocations;
        public bool IsShowingLocations
        {
            get { return _isShowingLocations; }
            set
            {
                if (_isShowingLocations != value)
                {
                    _isShowingLocations = value;
                    OnPropertyChanged(nameof(IsShowingLocations));
                    UpdateListVisibility();
                }
            }
        }

        private void UpdateListVisibility()
        {
            IsShowingEvents = !IsShowingLocations;
            IsShowingLocations = !IsShowingEvents;
        }

        private bool _isShowingEvents;
        public bool IsShowingEvents
        {
            get { return _isShowingEvents; }
            set
            {
                _isShowingEvents = value;
                OnPropertyChanged(nameof(IsShowingEvents));
            }
        }
        private bool _isMusicExpanded;

        public bool IsMusicExpanded
        {
            get { return _isMusicExpanded; }
            set
            {
                _isMusicExpanded = value;
            }
        }
        public ICommand ShowEventsCommand { get; }
        public ICommand ShowLocationsCommand { get; }
        public bool IsListEmpty { get; set; }

        public string Name { get; set; } = "Name";

        public bool IsDataLoaded { get; set; } = false;
        public Command BackBtn { get; }
        public Command PlusBtn { get; }

        public Command SaveBtn { get; }
        public ICommand MusicTappedCommand { get; }

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

        public Command LocationTapped { get; }

        private ObservableCollection<AppLocation> _locations;
        public ObservableCollection<AppLocation> Locations
        {
            get { return _locations; }
            set
            {
                _locations = value;
                OnPropertyChanged(nameof(Locations));
            }
        }

        private INavigation _navigation;

        private async void BackButtonTappedAsync(object parameter)
        {
        }

        private async void PlusButtonTappedAsync(object parameter)
        {
        }

        public ExplorePageViewModel(INavigation navigation)
        {
            LoadEvents();
            LoadLocations();

            _navigation = navigation;
            BackBtn = new Command(BackButtonTappedAsync);
            PlusBtn = new Command(PlusButtonTappedAsync);
            EventTapped = new Command(EventTappedAsync);
            LocationTapped = new Command(LocationTappedAsync);
            ShowEventsCommand = new Command(ShowEvents);
            ShowLocationsCommand = new Command(ShowLocations);
            MusicTappedCommand = new Command(ExpandMusicCategories);
            IsShowingEvents = true;
            IsShowingLocations = false;
            IsMusicExpanded = false;
            SaveBtn = new Command(SaveBtnTappedAsync);
        }

        private async void SaveBtnTappedAsync(object parameter)
        {
            await Application.Current.MainPage.DisplayAlert("ok", "ok", "ok");
        }

        public void ExpandMusicCategories()
        {
            IsMusicExpanded = !IsMusicExpanded;
            OnPropertyChanged(nameof(IsMusicExpanded));
            Console.WriteLine("MUSIC WAS TAPPED !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            Console.WriteLine(IsMusicExpanded);
            Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!");
        }

        private void ShowEvents()
        {
            IsShowingEvents = true;
            IsShowingLocations = false;

            // Update the ListView data source
            if (Events == null)
            {
                LoadEvents();
            }
        }

        private void ShowLocations()
        {
            IsShowingEvents = false;
            IsShowingLocations = true;

            // Update the ListView data source
            if (Locations == null)
            {
                LoadLocations();
            }
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

        private AppLocation _selectedLocation;
        public AppLocation SelectedLocation
        {
            get { return _selectedLocation; }
            set
            {
                _selectedLocation = value;
                OnPropertyChanged(nameof(SelectedLocation));
                OnLocationSelected();
            }
        }

        private async void OnLocationSelected()
        {
            if (SelectedLocation != null)
            {
                await _navigation.PushAsync(new LocationPage(SelectedLocation));
                SelectedLocation = null;
            }
        }

        private async void LoadEvents()
        {
            List<AppEvent> list = await EventsService.getAllEvents();
            if (list.Count == 0)
            {
                await Application.Current.MainPage.DisplayAlert("list0", "events empty", "ok");
            }
            ObservableCollection<AppEvent> collection = new ObservableCollection<AppEvent>(list);

            Events = collection;
            IsDataLoaded = true;
            await Task.Delay(1000);
            if (Events.Count == 0)
            {
                await Application.Current.MainPage.DisplayAlert("prolr", "events empty", "ok");
            }
        }

        private async void LoadLocations()
        {
            List<AppLocation> list = await LocationsService.getAllLocations();
            if (list.Count == 0)
            {
                await Application.Current.MainPage.DisplayAlert("list0", "empty locations", "ok");
            }
            ObservableCollection<AppLocation> collection = new ObservableCollection<AppLocation>(list);

            Locations = collection;
            IsDataLoaded = true;
            await Task.Delay(1000);
            if (Locations.Count == 0)
            {
                await Application.Current.MainPage.DisplayAlert("prolr", "locations empty", "ok");
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

        private async void LocationTappedAsync(object parameter)
        {
            if (parameter is AppLocation selectedLocation)
            {
                await Application.Current.MainPage.DisplayAlert("Event Selected", $"You tapped on {selectedLocation.Name}", "OK");

                // Call your custom method with the selected event
                // Example:
                // DoSomethingWithSelectedEvent(selectedEvent);
            }
        }
    }
}
