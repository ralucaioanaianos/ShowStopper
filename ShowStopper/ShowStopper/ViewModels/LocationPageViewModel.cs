using ShowStopper.Models;
using ShowStopper.Services;
using ShowStopper.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ShowStopper.ViewModels
{
    internal class LocationPageViewModel : INotifyPropertyChanged
    {
        public bool IsRatingVisible { get; set; }
        private AppLocation _location;
        public string Name { get; set; }
        public string Descriptionn { get; set; }
        public string Owner { get; set; }
        public string Image { get; set; }
        public string Address { get; set; }
        public decimal Rating { get; set; }

        public Command ReviewsBtn { get; set; }

        public string FavoritesText { get; set; }

        public System.Boolean IsEmptyHeartButtonVisible { get; } 
        public bool IsNotAddedToFavorites { get; } 

        public Command SeeEventsBtn { get; }


        public Command BackBtn { get; }
        public Command PlusBtn { get; }

        public Command EmptyHeartBtn { get; }

        public Command FullHeartBtn { get; }

        private INavigation _navigation;


        private async void BackButtonTappedAsync(object parameter)
        {
            await _navigation.PopAsync();
        }

        private async void PlusButtonTappedAsync(object parameter)
        {

        }

        private async void SeeEventsButtonTappedAsync(object parameter)
        {

        }

        private async void EmptyHeartButtonTappedAsync(object parameter)
        {
            bool result = await LocationsService.IsLocationInFavorites(_location);
            if (result)
            {
                await LocationsService.RemoveLocationFromFavorites(_location);
            }
            else
            {
                await LocationsService.AddLocationToFavorites(_location);

            }
        }

        private Action _locationModifiedCallback;

        public LocationPageViewModel(INavigation navigation, AppLocation appLocation, Action locationModifiedCallback)
        {
            IsEmptyHeartButtonVisible = false;
            _navigation = navigation;
            _location = appLocation;
            InitializeFavoritesButtons();
            Name = appLocation.Name;
            Descriptionn = appLocation.Description;
            Owner = appLocation.Owner;
            Address = appLocation.Address;
            Image = appLocation.Image;
            if (appLocation.Rating == 0)
            {
                IsRatingVisible = false;
            }
            else
            {
                IsRatingVisible = true;
            }
            Rating = appLocation.Rating;
            BackBtn = new Command(BackButtonTappedAsync);
            PlusBtn = new Command(PlusButtonTappedAsync);
            EmptyHeartBtn = new Command(EmptyHeartButtonTappedAsync);
            SeeEventsBtn = new Command(SeeEventsButtonTappedAsync);
            ReviewsBtn = new Command(ReviewsBtnTappedAsync);
            _locationModifiedCallback = locationModifiedCallback;
        }

        private async void ReviewsBtnTappedAsync(object parameter)
        {
            await _navigation.PushAsync(new ReviewsPage(_location, _locationModifiedCallback));
            _locationModifiedCallback.Invoke();
        }

        private async void InitializeFavoritesButtons()
        {
            bool result = await LocationsService.IsLocationInFavorites(_location);
            if (result)
            {
                FavoritesText = "Remove from Favorites";
            }
            else
            {
                FavoritesText = "Add to Favorites";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
