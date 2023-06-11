using ShowStopper.Models;
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
        public string Name { get; set; }
        public string Description = "desctptiron";
        public string Owner { get; set; }
        public string Address { get; set; }

        public Command SeeEventsBtn { get; }


        public Command BackBtn { get; }
        public Command PlusBtn { get; }

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

        public LocationPageViewModel(INavigation navigation, AppLocation appLocation)
        {
            _navigation = navigation;
            Name = "Location";
            Description = "location description";
            Owner = appLocation.Owner;
            Address = appLocation.Address;
            BackBtn = new Command(BackButtonTappedAsync);
            PlusBtn = new Command(PlusButtonTappedAsync);
            SeeEventsBtn = new Command(SeeEventsButtonTappedAsync);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
