using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Storage;
using ShowStopper.Models;
using ShowStopper.Services;
using ShowStopper.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace ShowStopper.ViewModels
{
    internal class ProfilePageViewModel : INotifyPropertyChanged
    {
        private ImageSource _srcImg;
        public ImageSource SrcImg
        {
            get { return _srcImg; }
            set
            {
                if (_srcImg != value)
                {
                    _srcImg = value;
                    OnPropertyChanged(nameof(SrcImg)); // Raise the PropertyChanged event
                }
            }
        }
        public Command BackBtn { get; }
        public Command FavoriteLocationsBtn { get; }
        public Command ResetPasswordBtn { get; }
        public Command SendFeedbackBtn { get; }
        public Command SignOutBtn { get; set; }
        public Command MyTicketsBtn { get; }
        public Command PlusBtn { get; }
        private INavigation _navigation;
        private User _user;
        private AppUser _databaseUser;
        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name)); // Raise the PropertyChanged event
                }
            }
        }

        public Command EditProfileBtn { get; }
        public ImageSource ProfileImageSource { get; set; } 

        public ProfilePageViewModel(INavigation navigation)
        {
            _navigation = navigation;
            Initialize();
            BackBtn = new Command(BackButtonTappedAsync);
            PlusBtn = new Command(PlusButtonTappedAsync);
            SignOutBtn = new Command(SignOutButtonTappedAsync);
            EditProfileBtn = new Command(EditProfileBtnTappedAsync);
            FavoriteLocationsBtn = new Command(FavoriteLocationsBtnTappedAsync);
            ResetPasswordBtn = new Command(ResetPasswordBtnTappedAsync);
            SendFeedbackBtn = new Command(SendFeedbackBtnTappedAsync);
            MyTicketsBtn = new Command(MyTicketsBtnTappedAsync);
        }

        private async void Initialize()
        {
            await GetUser();
            OnPropertyChanged(nameof(Name)); // Notify the UI about the updated Name value
        }

        private async void BackButtonTappedAsync(object parameter)
        {
        }

        private async void PlusButtonTappedAsync(object parameter)
        {
        }

        private async void SignOutButtonTappedAsync(object parameter)
        {
            FirebaseAuthenticationService.LogoutUser();
            await _navigation.PushAsync(new LoginPage("false"));
        }

        private async Task GetUser()
        {
            string currentEmail = FirebaseAuthenticationService.GetLoggedUserEmail();
            _databaseUser = await FirebaseDatabaseService.GetUserByEmail(currentEmail);
            if (_databaseUser.FirstName == null) {
                _databaseUser = new AppUser
                {
                    FirstName = "nullfirstname",
                    LastName = "nulllastname",
                    Email = currentEmail,
                    UserType = "User",
                };
            }
            Name = _databaseUser.FirstName + " " + _databaseUser.LastName;
            SrcImg = _databaseUser.ProfileImage;
        }

        private async void EditProfileBtnTappedAsync(object parameter)
        {
            await _navigation.PushAsync(new EditProfilePage(_user, _databaseUser));
            await GetUser();
            OnPropertyChanged(nameof(Name));
            OnPropertyChanged(nameof(SrcImg));
        }

        private async void MyTicketsBtnTappedAsync(object parameter)
        {
            await _navigation.PushAsync(new MyTicketsPage());
        }

        private async void FavoriteLocationsBtnTappedAsync(object parameter)
        {
            await _navigation.PushAsync(new FavoriteLocationsPage());
        }

        private async void ResetPasswordBtnTappedAsync(object parameter)
        {
            bool userConfirmed = await Application.Current.MainPage.DisplayAlert("Reset Password", "If you press yes, you will be automatically logged out and you will receive an email with the instructions of resetting your password. Are you sure you want to continue?", "Yes", "No");

            if (userConfirmed)
            {
                await FirebaseAuthenticationService.ResetPassword();
                await _navigation.PushAsync(new LoginPage("false"));
            }
        }

        private async void SendFeedbackBtnTappedAsync(object parameter)
        {
            await _navigation.PushAsync(new FeedbackPage());
        }

        private async void ImageButton_Clicked(object sender, EventArgs e)
        {
            await _navigation.PushAsync(new EditProfilePage(_user, _databaseUser));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
