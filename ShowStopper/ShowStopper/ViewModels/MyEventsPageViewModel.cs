using Firebase.Auth;
using ShowStopper.Models;
using ShowStopper.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ShowStopper.ViewModels
{
    internal class MyEventsPageViewModel
    {
        public Command BackBtn { get; }
        private INavigation _navigation;

        private async void BackButtonTappedAsync(object parameter)
        {
            await Application.Current.MainPage.DisplayAlert("aaa", "aaaa", "ok");
            //await _navigation.PushAsync(new MyLocationsPage());
        }

        public MyEventsPageViewModel(INavigation navigation)
        {
            _navigation = navigation;
            BackBtn = new Command(BackButtonTappedAsync);
        }
    }
}
