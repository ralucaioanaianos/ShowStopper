using Microsoft.Extensions.Logging;
using ShowStopper.Models;
using ShowStopper.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowStopper.ViewModels
{
    internal class MyLocationsPageViewModel : INotifyPropertyChanged
    {
        public bool IsListEmpty { get; set; }
        public bool IsDataLoaded { get; set; } = false;
        public Command BackBtn { get; }
        public Command PlusBtn { get; }

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
            await _navigation.PopAsync();
        }

        private async void PlusButtonTappedAsync(object parameter)
        {
            //await _navigation.PushAsync(new AddEventPage());
        }

        public MyLocationsPageViewModel(INavigation navigation)
        {
            LoadLocations();

            _navigation = navigation;
            BackBtn = new Command(BackButtonTappedAsync);
            PlusBtn = new Command(PlusButtonTappedAsync);
            LocationTapped = new Command(LocationTappedAsync);
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
                //await _navigation.PushAsync(new LocationPage(SelectedLocation));
                SelectedLocation = null;
            }
        }

        private async void LoadLocations()
        {
            string email = FirebaseAuthenticationService.GetLoggedUserEmail();
            List<AppLocation> list = await LocationsService.getLocationsByEmail(email);
            if (list.Count == 0)
            {
                await Application.Current.MainPage.DisplayAlert("list0", email, "ok");
            }
            ObservableCollection<AppLocation> collection = new ObservableCollection<AppLocation>(list);

            Locations = collection;
            IsDataLoaded = true;
            await Task.Delay(1000);
            if (Locations.Count == 0)
            {
                await Application.Current.MainPage.DisplayAlert("prolr", email, "ok");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async void LocationTappedAsync(object parameter)
        {
            if (parameter is AppLocation selectedLocation)
            {
                await Application.Current.MainPage.DisplayAlert("Location Selected", $"You tapped on {selectedLocation.Name}", "OK");

                // Call your custom method with the selected event
                // Example:
                // DoSomethingWithSelectedEvent(selectedEvent);
            }
        }
    }
}
