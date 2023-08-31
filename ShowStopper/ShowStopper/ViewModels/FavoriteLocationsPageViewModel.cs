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
    class FavoriteLocationsPageViewModel : INotifyPropertyChanged
    {
        public bool IsListEmpty { get; set; }
        public bool IsDataLoaded { get; set; } = false;
        public Command BackBtn { get; }
        public Command PlusBtn { get; }

        public Command LocationTapped { get; }
        private ObservableCollection<AppLocation> _favoriteLocations;
        public ObservableCollection<AppLocation> FavoriteLocations
        {
            get { return _favoriteLocations; }
            set
            {
                _favoriteLocations = value;
                OnPropertyChanged(nameof(FavoriteLocations));
            }
        }

        private INavigation _navigation;

        private async void BackButtonTappedAsync(object parameter)
        {
            await _navigation.PopAsync();
        }

        private async void PlusButtonTappedAsync(object parameter)
        { }

        public FavoriteLocationsPageViewModel(INavigation navigation)
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
                await _navigation.PushAsync(new LocationPage(SelectedLocation, LoadLocationsAfterReviewAdded));
                SelectedLocation = null;
            }
        }

        private async void LoadLocationsAfterReviewAdded()
        {
            await LoadLocations();
        }

        private async Task LoadLocations()
        {
            try
            {
                string email = FirebaseAuthenticationService.GetLoggedUserEmail();
                List<LocationFavorite> favoritesList = await LocationsService.GetFavoriteLocationsByEmail(email);
                ObservableCollection<AppLocation> collection = new ObservableCollection<AppLocation>();
                foreach (LocationFavorite favorite in favoritesList)
                {
                    AppLocation location = await LocationsService.GetLocationByName(favorite.LocationName);
                    collection.Add(location);
                }
                FavoriteLocations = collection;
                IsDataLoaded = true;
                await Task.Delay(1000);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
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
