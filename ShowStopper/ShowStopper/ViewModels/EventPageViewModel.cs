using ShowStopper.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowStopper.ViewModels
{
    class EventPageViewModel : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public string Description = "desctptiron";
        public string Date { get; set; }
        public string Location { get; set; }

        public string Organizer { get; set; }
        public Command BuyBtn { get; }

        
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

        private async void BuyButtonTappedAsync(object parameter)
        {

        }

        public EventPageViewModel(INavigation navigation, AppEvent appEvent)
        {
            _navigation = navigation;
            Name = "Event";
            Description = "event description";
            Date = appEvent.Date;
            Location = appEvent.Location;
            Organizer = appEvent.Organizer;
            BackBtn = new Command(BackButtonTappedAsync);
            PlusBtn = new Command(PlusButtonTappedAsync);
            BuyBtn = new Command(BuyButtonTappedAsync);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
