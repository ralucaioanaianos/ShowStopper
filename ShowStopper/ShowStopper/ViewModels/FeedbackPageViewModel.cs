using ShowStopper.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowStopper.ViewModels
{
    internal class FeedbackPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string _feedbackMessage;

        public string FeedbackMessage
        {
            get => _feedbackMessage;
            set
            {
                _feedbackMessage = value;
                OnPropertyChanged(FeedbackMessage);
            }
        }

        private INavigation _navigation;
        public Command BackBtn { get; }
        public Command PlusBtn { get; }
        public Command SendBtn { get; set; }

        public FeedbackPageViewModel(INavigation navigation)
        {
            _navigation = navigation;
            BackBtn = new Command(BackButtonTappedAsync);
            PlusBtn = new Command(PlusButtonTappedAsync);
            SendBtn = new Command(SendBtnTappedAsync);
        }

        private async void BackButtonTappedAsync(object parameter)
        {
            await _navigation.PopAsync();
        }

        private async void PlusButtonTappedAsync(object parameter)
        {
        }

        private async void SendBtnTappedAsync(object parameter)
        {
            await Application.Current.MainPage.DisplayAlert("", "Thank you for your message!", "Close");
            string email = FirebaseAuthenticationService.GetLoggedUserEmail();
            await FirebaseDatabaseService.AddFeedbackMessageToDatabase(email, FeedbackMessage);
            await Application.Current.MainPage.DisplayAlert("", "Thank you for your message!", "Close");
            FeedbackMessage = string.Empty;
        }
    }
}
