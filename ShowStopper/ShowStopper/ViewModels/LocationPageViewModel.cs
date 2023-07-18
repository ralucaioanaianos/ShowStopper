using ShowStopper.Models;
using ShowStopper.Services;
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
        private AppLocation _location;
        public string Name { get; set; }
        public string Descriptionn { get; set; }
        public string Owner { get; set; }
        public string Address { get; set; }

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
            await LocationsService.AddLocationToFavorites(_location);
        }

        public LocationPageViewModel(INavigation navigation, AppLocation appLocation)
        {
            _navigation = navigation;
            _location = appLocation;
            Name = appLocation.Name;
            Descriptionn = appLocation.Description;
            Owner = appLocation.Owner;
            Address = appLocation.Address;
            BackBtn = new Command(BackButtonTappedAsync);
            PlusBtn = new Command(PlusButtonTappedAsync);
            EmptyHeartBtn = new Command(EmptyHeartButtonTappedAsync);
            SeeEventsBtn = new Command(SeeEventsButtonTappedAsync);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
