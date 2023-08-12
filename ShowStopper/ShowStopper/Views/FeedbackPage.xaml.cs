using ShowStopper.ViewModels;

namespace ShowStopper.Views;

public partial class FeedbackPage : ContentPage
{
	public FeedbackPage()
	{
		InitializeComponent();
		BindingContext = new FeedbackPageViewModel(Navigation);
	}
}