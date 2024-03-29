﻿using Firebase.Auth;
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
        private ObservableCollection<AppEvent> _originalEvents;
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
            await _navigation.PushAsync(new AddEventPage(LoadReviewsAfterReviewAdded));
            LoadEvents();
            OnPropertyChanged(nameof(Events));
        }

        private async void LoadReviewsAfterReviewAdded()
        {
            await LoadEvents();
        }

        public MyEventsPageViewModel(INavigation navigation)
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

        public void UpdateSearchResults(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                _originalEvents = new ObservableCollection<AppEvent>(Events);
            }
            else
            {
                Events = new ObservableCollection<AppEvent>(
                    _originalEvents.Where(l => l.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                                                 l.Location.Contains(searchText, StringComparison.OrdinalIgnoreCase)));
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
            List<AppEvent> list = await EventsService.GetEventsByEmail(email);
            ObservableCollection<AppEvent> collection = new ObservableCollection<AppEvent>(list);
            Events = collection;
            _originalEvents = new ObservableCollection<AppEvent>(Events);
            IsDataLoaded = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
