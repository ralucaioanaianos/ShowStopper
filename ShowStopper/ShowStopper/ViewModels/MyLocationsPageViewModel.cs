using Microsoft.Extensions.Logging;
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
    internal class MyLocationsPageViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<AppLocation> _originalLocations;

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

        private async void LoadLocationsAfterReviewAdded()
        {
            await LoadLocations();
        }

        private async void PlusButtonTappedAsync(object parameter)
        {
            await _navigation.PushAsync(new AddLocationPage(LoadLocationsAfterReviewAdded));
            OnPropertyChanged(nameof(Locations));
        }

        public MyLocationsPageViewModel(INavigation navigation)
        {
            LoadLocations();

            _navigation = navigation;
            BackBtn = new Command(BackButtonTappedAsync);
            PlusBtn = new Command(PlusButtonTappedAsync);
            LocationTapped = new Command(LocationTappedAsync);
        }

        public void UpdateSearchResults(string searchText)
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
                await _navigation.PushAsync(new LocationPage(SelectedLocation, LoadLocationsAfterReviewAdded));
                SelectedLocation = null;
            }
        }

        private async Task LoadLocations()
        {
            try
            {
                string email = FirebaseAuthenticationService.GetLoggedUserEmail();
                List<AppLocation> list = await LocationsService.getLocationsByEmail(email);
                ObservableCollection<AppLocation> collection = new ObservableCollection<AppLocation>(list);
                Locations = collection;
                _originalLocations = new ObservableCollection<AppLocation>(Locations);
                IsDataLoaded = true;
                await Task.Delay(1000);
            } catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async void LocationTappedAsync(object parameter)
        {
        }
    }
}
