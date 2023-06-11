using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using ShowStopper.Models;
using ShowStopper.Services;
using ShowStopper.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowStopper.ViewModels
{
    internal class ProfilePageViewModel
    {
        private INavigation _navigation;

        private User _user;
        private AppUser _databaseUser;

        //private FirebaseClient _firebaseClient;

        public string Name { get; set; }

        public Command EditProfileBtn { get; }

        public ImageSource ProfileImageSource { get; set; } 

        public ProfilePageViewModel(INavigation navigation)
        {
            _navigation = navigation;
            GetUser();
            EditProfileBtn = new Command(EditProfileBtnTappedAsync);
        }

        private async void GetUser()
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
            await Application.Current.MainPage.DisplayAlert("aa", _databaseUser.Email + ' ' + _databaseUser.FirstName, "ok");
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
    }
}
