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
        public string ImageName { get; set; }
        public ImageSource SrcImg { get; set; }
        public Command BackBtn { get; }
        public Command FavoriteLocationsBtn { get; }
        public Command ResetPasswordBtn { get; }
        public Command SignOutBtn { get; set; }
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
            //SrcImg = "https://firebasestorage.googleapis.com/v0/b/showstopper-71398.appspot.com/o/profile_images%2Fcat.jpg?alt=media&token=82358ea8-ab06-4942-bd23-cef6151358bf";
            _navigation = navigation;
            Initialize();
            BackBtn = new Command(BackButtonTappedAsync);
            PlusBtn = new Command(PlusButtonTappedAsync);
            SignOutBtn = new Command(SignOutButtonTappedAsync);
            EditProfileBtn = new Command(EditProfileBtnTappedAsync);
            FavoriteLocationsBtn = new Command(FavoriteLocationsBtnTappedAsync);
            ResetPasswordBtn = new Command(ResetPasswordBtnTappedAsync);
        }

        private async void Initialize()
        {
            await GetUser();
            var firebaseStorage = new FirebaseStorage("showstopper-71398.appspot.com");
            var downloadUrl = await firebaseStorage
                .Child("profile_images")
                .Child("cat.jpg")
                .GetDownloadUrlAsync();
            int tokenIndex = downloadUrl.IndexOf("token=");

            //if (tokenIndex >= 0)
            //{
            //    downloadUrl = downloadUrl.Substring(tokenIndex + 6);
            //    //Console.WriteLine(token);
            //}
            SrcImg = "https://firebasestorage.googleapis.com/v0/b/showstopper-71398.appspot.com/o/profile_images%2Fcat.jpg?alt=media&token=82358ea8-ab06-4942-bd23-cef6151358bf";
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
            string profileImageUrl = _databaseUser.ProfileImage;
            int startIndex = profileImageUrl.IndexOf("photos%2F") + 9;
            int endIndex = profileImageUrl.LastIndexOf("?alt");

            if (startIndex >= 0 && endIndex >= 0 && endIndex > startIndex)
            {
                ImageName = profileImageUrl.Substring(startIndex, endIndex - startIndex);
            }
        }

        private async void EditProfileBtnTappedAsync(object parameter)
        {
            await _navigation.PushAsync(new EditProfilePage(_user, _databaseUser));
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
