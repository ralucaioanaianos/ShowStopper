using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using ShowStopper.Models;
using ShowStopper.Services;
using ShowStopper.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowStopper.ViewModels
{
    internal class ProfilePageViewModel : INotifyPropertyChanged
    {
        public Command BackBtn { get; }
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
            Application.Current.MainPage.DisplayAlert("bb", Name, "OK");
            BackBtn = new Command(BackButtonTappedAsync);
            PlusBtn = new Command(PlusButtonTappedAsync);
            EditProfileBtn = new Command(EditProfileBtnTappedAsync);
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
            await Application.Current.MainPage.DisplayAlert("aa", Name, "ok");
            // they are correctly retrieved
            string profileImageUrl = _databaseUser.ProfileImage;
        }

        private async void EditProfileBtnTappedAsync(object parameter)
        {
            await _navigation.PushAsync(new EditProfilePage(_user, _databaseUser));
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
