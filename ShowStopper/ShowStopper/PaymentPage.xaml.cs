using ShowStopper.ViewModels;

namespace ShowStopper;

public partial class PaymentPage : ContentPage
{
	public PaymentPage()
	{
		InitializeComponent();
		BindingContext = new PaymentPageViewModel();
	}
}