using PayPalCheckoutSdk.Core;
using PayPalCheckoutSdk.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowStopper.Services
{
    class PaymentService
    {
        public static async Task<decimal> StartPayment(decimal Price)
        {
            if (Price > 0)
            {
                string result = await Application.Current.MainPage.DisplayPromptAsync("Purchase tickets", "How many tickets do you want to purchase?", initialValue: "0", maxLength: 2, keyboard: Keyboard.Numeric);
                var environment = new LiveEnvironment("AfsCfY_-W3mkduSG4GWQg9jifpZuW1p0SY7WPF_CATT2MuBMkr6tZu6eCrdoJo8Crn8FlJ1g35zFbeXt", "EHOY5cjeM9vT1NPYzWGU4ezYLEXaIV0bWmTZqzg6L0cZhKsbgSgoFPMTAd9HWGvNiRpJPD1iXqTkQe1c");
                var client = new PayPalHttpClient(environment);
                var order = new OrderRequest()
                {
                    CheckoutPaymentIntent = "CAPTURE",
                    PurchaseUnits = new List<PurchaseUnitRequest>
                {
                    new PurchaseUnitRequest
                    {
                        AmountWithBreakdown = new AmountWithBreakdown
                        {
                            CurrencyCode = "EUR",
                            Value = (Price*(decimal.Parse(result))).ToString("0.00"),
                        }
                    }
                }
                };
                try
                {
                    var request = new OrdersCreateRequest();
                    request.RequestBody(order);
                    var response = await client.Execute(request);
                    var orderResult = response.Result<Order>();

                    var approvalUrl = orderResult.Links
                        .Find(link => link.Rel.Equals("approve", StringComparison.OrdinalIgnoreCase))
                        .Href;
                    await Browser.OpenAsync(new Uri(approvalUrl), BrowserLaunchMode.SystemPreferred);
                    return decimal.Parse(result);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return 0;
                }
            }
            else
            {
                string result = await Application.Current.MainPage.DisplayPromptAsync("Purchase tickets", "How many tickets do you want to purchase?", initialValue: "0", maxLength: 2, keyboard: Keyboard.Numeric);
                return decimal.Parse(result);
            }
        }
    }
}
