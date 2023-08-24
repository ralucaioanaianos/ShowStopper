using PayPalCheckoutSdk.Core;
using PayPalCheckoutSdk.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ShowStopper.ViewModels
{
    internal class PaymentPageViewModel
    {
        private INavigation _navigation;

        private decimal _totalAmount;
        public decimal TotalAmount
        {
            get => _totalAmount;
            set => _totalAmount = value;
        }

        public PaymentPageViewModel()
        {
            
            
        }

        public ICommand StartPaymentCommand => new Command(async () => await StartPaymentAsync());

        private async Task StartPaymentAsync()
        {
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
                            Value = TotalAmount.ToString("0.00"),
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
            }
            catch (Exception ex)
            {
                // Handle exception
                await Application.Current.MainPage.DisplayAlert("payment error", ex.Message, "ok");
            }
        }
    }
}
