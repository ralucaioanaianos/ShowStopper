﻿using ShowStopper.Models;
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
        public Command Refresh { get; set; }
        public Command ExitBtn { get; }
        public DateTime FromTime { get; set; }
        public DateTime ToTime { get; set; }
        public decimal FromPrice { get; set; }
        public decimal ToPrice { get; set; } 
        public Command SaveBtn { get; }
        private string _searchQuery;
        public string SearchQuery
        {
            get { return _searchQuery; }
            set
            {
                _searchQuery = value;
                OnPropertyChanged(nameof(SearchQuery));
                ApplySearch();
            }
        }

        private ObservableCollection<AppEvent> _originalEvents;
        private ObservableCollection<AppLocation> _originalLocations;


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
        public Command ShowFiltersBtn { get; }
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

        private DateTime GetMaximumEventsDate()
        {
            DateTime maximumDate = DateTime.Now;
            //Console.WriteLine("LENGTH: " + Events.Count);
            if(Events is null)
            {
                return maximumDate;
            }
            foreach (AppEvent ev in Events)
            {
                if (ev.Date > maximumDate)
                    maximumDate = ev.Date;
            }
            return maximumDate;
        }

        private decimal GetMaximumEventsPrice()
        {
            decimal maximumPrice = 0;
            foreach (AppEvent ev in Events)
            {
                if (ev.Price > maximumPrice)
                    maximumPrice = ev.Price;
            }
            return maximumPrice;
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
            IsShowingEvents = true;
            IsShowingLocations = false;
            IsMusicExpanded = false;
            SaveBtn = new Command(SaveBtnTappedAsync);
            ShowFiltersBtn = new Command(ShowFiltersBtnTappedAsync);
            ExitBtn = new Command(ExitBtnTappedAsync);
            Refresh = new Command(RefreshTriggered);
            FromTime = DateTime.Today;
            //ToTime = DateTime.Today;
            
            //FromPrice = 0;
            //ToPrice = GetMaximumEventsPrice();
        }

        private async void RefreshTriggered()
        {
            OnPropertyChanged(nameof(Events));  
        }

        private async void ExitBtnTappedAsync(object parameter)
        {
            await _navigation.PopAsync();
        }

        public void UpdateSearchResults(string searchText)
        {
            if (IsShowingEvents)
            {
                if (string.IsNullOrWhiteSpace(searchText))
                {
                    Events = new ObservableCollection<AppEvent>(_originalEvents);
                }
                else
                {
                    Events = new ObservableCollection<AppEvent>(
                        _originalEvents.Where(e => e.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                                                   e.Description.Contains(searchText, StringComparison.OrdinalIgnoreCase)));
                }
            }
            else if (IsShowingLocations)
            {
                if (string.IsNullOrWhiteSpace(searchText))
                {
                    Locations = new ObservableCollection<AppLocation>(_originalLocations);
                }
                else
                {
                    Locations = new ObservableCollection<AppLocation>(
                        _originalLocations.Where(l => l.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                                                     l.Address.Contains(searchText, StringComparison.OrdinalIgnoreCase)));
                }
            }
        }

        private void ApplySearch()
        {
            if (IsShowingEvents)
            {
                Events = new ObservableCollection<AppEvent>(
                    Events.Where(e => e.Name.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase) ||
                                       e.Description.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase)));
            }
            else if (IsShowingLocations)
            {
                Locations = new ObservableCollection<AppLocation>(
                    Locations.Where(l => l.Name.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase) ||
                                         l.Address.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase)));
            }
        }

        private async void ShowFiltersBtnTappedAsync(object parameter)
        {
            await _navigation.PushAsync(new FilterEventsPage());
        }

        private async void SaveBtnTappedAsync(object parameter)
        {
            if (FromPrice > ToPrice)
            {
                await Application.Current.MainPage.DisplayAlert("Invalid price range", "Minimum price cannot be greater than maximum price!", "ok");
            }

            else if (FromTime <  DateTime.Now || ToTime < DateTime.Now || ToTime < FromTime)
            {
                await Application.Current.MainPage.DisplayAlert("Invalid date", "", "ok");
            }
            else
            {
                Events = new ObservableCollection<AppEvent>(Events.Where(e => e.Date >= FromTime && e.Date <= ToTime && e.Price >= FromPrice && e.Price <= ToPrice));
                await Application.Current.MainPage.DisplayAlert("events filtered", Events.Count.ToString(), "ok");
                //OnPropertyChanged(nameof(Events));
                await _navigation.PopAsync();
            }
        }
        private void ShowEvents()
        {
            IsShowingEvents = true;
            IsShowingLocations = false;
            if (Events == null)
            {
                LoadEvents();
            }
        }

        private void ShowLocations()
        {
            IsShowingEvents = false;
            IsShowingLocations = true;
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
            _originalEvents = new ObservableCollection<AppEvent>(Events); // Initialize with your original events data
            IsDataLoaded = true;
            ToTime = DateTime.MinValue;
            if (Events.Count == 0)
            {
                await Application.Current.MainPage.DisplayAlert("prolr", "events empty", "ok");
            }
            else
            {
                foreach(AppEvent e in Events) 
                { 
                    if (e.Date > ToTime)
                    {
                        ToTime = e.Date;
                        OnPropertyChanged(nameof(ToTime));
                    }
                }
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
            _originalLocations = new ObservableCollection<AppLocation>(Locations); // Initialize with your original locations data
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
