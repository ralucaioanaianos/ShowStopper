using Firebase.Auth;
using Firebase.Auth.Providers;
using Firebase.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShowStopper.Models;
using Firebase.Database.Query;
using Firebase.Storage;
using System.Runtime.CompilerServices;
using ShowStopper.Services;

namespace ShowStopper.ViewModels
{
    internal class RegisterPageViewModel
    {
        public string webApiKey = "AIzaSyCBEbT1yT0WqRG6Rsts6dYdMz5OQ9dBHVM";

        private string storageUrl = "showstopper-71398.appspot.com";
        private string authDomain = "showstopper-71398.firebaseapp.com";
        private string databaseUrl = "https://showstopper-71398-default-rtdb.europe-west1.firebasedatabase.app/";
        private INavigation _navigation;
        private string email;
        private string password;
        private string firstName;
        private string lastName;
        private FileResult photo;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Email
        {
            get => email;
            set
            {
                email = value;
                RaisePropertyChanged("Email");
            }
        }

        public string FirstName
        {
            get => firstName;
            set
            {
                firstName = value;
                RaisePropertyChanged("FirstName");
            }
        }

        public string LastName
        {
            get => lastName;
            set
            {
                lastName = value;
                RaisePropertyChanged("LastName");
            }
        }

        public string Password
        {
            get => password; set
            {
                password = value;
                RaisePropertyChanged("Password");
            }
        }

        public Command RegisterUser { get; }

        public Command SelectPhoto { get; }

        private void RaisePropertyChanged(string v)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(v));
        }

        public RegisterPageViewModel(INavigation navigation)
        {
            _navigation = navigation;
            SelectPhoto = new Command(SelectPhotoTappedAsync);
            RegisterUser = new Command(RegisterUserTappedAsync);
        }

        private async void RegisterUserTappedAsync(object obj)
        {
            try
            {
                await FirebaseAuhenticationService.CreateUserFirebase(webApiKey, authDomain, email, password);
                if (photo != null)
                {
                    string photoUrl = await FirebaseStorageService.UploadPhotoToStorage(storageUrl, photo);
                    await FirebaseDatabaseService.SavePhotoToDatabase(databaseUrl, photoUrl);
                    //TODO: userType instead of "User"
                    await FirebaseDatabaseService.AddUserToDatabase(databaseUrl, firstName, lastName, email, photoUrl, "User");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("error", "please upload photo", "ok");
                }

                await _navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Alert", ex.Message, "OK");
                throw;
            }
            await _navigation.PushAsync(new RegisterPage());
        }
        
        private async void SelectPhotoTappedAsync(object sender)
        {
            try
            {
                photo = await MediaPicker.PickPhotoAsync();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("error", ex.Message, "ok");
            }
        }

    }
}
