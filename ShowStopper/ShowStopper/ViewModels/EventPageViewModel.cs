using PayPalCheckoutSdk.Core;
using PayPalCheckoutSdk.Orders;
using ShowStopper.Models;
using ShowStopper.Services;
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
        public string Name { get; set; }
        public string Descriptionn { get; set; }
        public DateTime Date { get; set; }
        public string Location { get; set; }

        public AppEvent AppEvent { get; set; }

        public string Organizer { get; set; }
        public string Image { get; set; }
        public decimal Price { get; set; }

        public Command BuyBtn { get; }

        
        public Command BackBtn { get; }
        public Command PlusBtn { get; }
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
            BackBtn = new Command(BackButtonTappedAsync);
            PlusBtn = new Command(PlusButtonTappedAsync);
            BuyBtn = new Command(BuyButtonTappedAsync);

        }

        private async Task StartPaymentAsync()
        {
            if (Price > 0)
            {
                string result = await Application.Current.MainPage.DisplayPromptAsync("Purchase tickets", "How many tickets do you want to purchase?", initialValue: "0", maxLength: 2, keyboard: Keyboard.Numeric);

                // await Application.Current.MainPage.DisplayAlert("ok", "ok", "ok");
                // Set up PayPal environment and client
                var environment = new LiveEnvironment("AfsCfY_-W3mkduSG4GWQg9jifpZuW1p0SY7WPF_CATT2MuBMkr6tZu6eCrdoJo8Crn8FlJ1g35zFbeXt", "EHOY5cjeM9vT1NPYzWGU4ezYLEXaIV0bWmTZqzg6L0cZhKsbgSgoFPMTAd9HWGvNiRpJPD1iXqTkQe1c");
                var client = new PayPalHttpClient(environment);

                // Create an order with items and amount
                var order = new OrderRequest()
                {
                    CheckoutPaymentIntent = "CAPTURE",
                    PurchaseUnits = new List<PurchaseUnitRequest>
                {
                    new PurchaseUnitRequest
                    {
                        AmountWithBreakdown = new AmountWithBreakdown
                        {
                            CurrencyCode = "USD",
                            Value = (Price*(decimal.Parse(result))).ToString("0.00"),
                        }
                    }
                }
                };
                try
                {
                    // Create order and get the approval URL
                    var request = new OrdersCreateRequest();
                    request.RequestBody(order);
                    var response = await client.Execute(request);
                    var orderResult = response.Result<Order>();

                    var approvalUrl = orderResult.Links
                        .Find(link => link.Rel.Equals("approve", StringComparison.OrdinalIgnoreCase))
                        .Href;

                    // Open a browser or in-app browser to complete payment
                    await Browser.OpenAsync(new Uri(approvalUrl), BrowserLaunchMode.SystemPreferred);
                    for (var i = 0; i < decimal.Parse(result);i++)
                    {
                        await UserService.AddEventToUser(AppEvent.Name);
                    }
                }
                catch (Exception ex)
                {
                    // Handle exception
                    await Application.Current.MainPage.DisplayAlert("payment error", ex.Message, "ok");
                }
            }
            else
            {
                string result = await Application.Current.MainPage.DisplayPromptAsync("Purchase tickets", "How many tickets do you want to purchase?", initialValue: "0", maxLength: 2, keyboard: Keyboard.Numeric);
                for (var i = 0;i < decimal.Parse(result);i++)
                {
                    await UserService.AddEventToUser(AppEvent.Name);
                }
            }  
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
