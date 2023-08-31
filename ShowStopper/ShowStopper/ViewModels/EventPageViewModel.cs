using PayPalCheckoutSdk.Core;
using PayPalCheckoutSdk.Orders;
using ShowStopper.Models;
using ShowStopper.Services;
using ShowStopper.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ShowStopper.ViewModels
{
    class EventPageViewModel : INotifyPropertyChanged
    {
        public Command SimilarBtn { get; }
        public string Name { get; set; }
        public string Descriptionn { get; set; }
        public DateTime Date { get; set; }
        public string Location { get; set; }

        public AppEvent AppEvent { get; set; }

        public string Organizer { get; set; }
        public string Image { get; set; }
        public decimal Price { get; set; }

        public Command BuyBtn { get; }

        public Command EmptyHeartBtn { get; }

        public Command BackBtn { get; }
        public Command PlusBtn { get; }
        private string _heartSrc;
        public string HeartSrc
        {
            get { return _heartSrc; }
            set
            {
                if (_heartSrc != value)
                {
                    _heartSrc = value;
                    OnPropertyChanged(nameof(HeartSrc)); // Raise the PropertyChanged event
                }
            }
        }
        private INavigation _navigation;

        public ICommand StartPaymentCommand => new Command(async () => await StartPaymentAsync());

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

        private async void EmptyHeartButtonTappedAsync(object parameter)
        {
            bool result = await EventsService.IsEventInFavorites(AppEvent);
            if (result)
            {
                await EventsService.RemoveEventFromFavorites(AppEvent);
                HeartSrc = "empty_heart.png";
            }
            else
            {
                await EventsService.AddEventToFavorites(AppEvent);
                HeartSrc = "red_heart.png";
            }
        }

        public EventPageViewModel(INavigation navigation, AppEvent appEvent)
        {
            _navigation = navigation;
            AppEvent = appEvent;
            Name = appEvent.Name;
            Descriptionn = appEvent.Description;
            Date = appEvent.Date;
            Location = appEvent.Location;
            Organizer = appEvent.Organizer;
            Price = appEvent.Price;
            Image = appEvent.Image;
            InitializeHeart();
            //HeartSrc = "empty_heart.png";
            BackBtn = new Command(BackButtonTappedAsync);
            PlusBtn = new Command(PlusButtonTappedAsync);
            BuyBtn = new Command(BuyButtonTappedAsync);
            SimilarBtn = new Command(SimilarBtnTappedAsync);
            EmptyHeartBtn = new Command(EmptyHeartButtonTappedAsync);

        }

        public async void InitializeHeart()
        {
            bool result = await EventsService.IsEventInFavorites(AppEvent);
            if (result)
            {
                HeartSrc = "red_heart.png";
            }
            else
            {
                HeartSrc = "empty_heart.png";
            }
            await Application.Current.MainPage.DisplayAlert("heart", HeartSrc, "ok");
        }

        private async void SimilarBtnTappedAsync(object parameter)
        {
            await _navigation.PushAsync(new SimilarEventsPage(AppEvent));
        }
        private async Task StartPaymentAsync()
        {
            decimal result = await PaymentService.StartPayment(Price);
            if (result != 0)
                for (var i = 0; i < result;i++)
                {
                    await UserService.AddEventToUser(AppEvent.Name, AppEvent.Image);
                }     
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
