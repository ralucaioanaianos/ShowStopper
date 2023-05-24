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

namespace ShowStopper.ViewModels
{
    internal class RegisterPageViewModel
    {
        public string webApiKey = "AIzaSyCBEbT1yT0WqRG6Rsts6dYdMz5OQ9dBHVM";

        private string authDomain = "showstopper-71398.firebaseapp.com";
        private string databaseUrl = "https://showstopper-71398-default-rtdb.europe-west1.firebasedatabase.app/";
        private INavigation _navigation;
        private string email;
        private string password;
        private string firstName;
        private string lastName;

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

        private void RaisePropertyChanged(string v)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(v));
        }

        public RegisterPageViewModel(INavigation navigation)
        {
            _navigation = navigation;

            RegisterUser = new Command(RegisterUserTappedAsync);
        }

        private async Task CreateFirebaseUser()
        {
            FirebaseAuthConfig authConfig = new FirebaseAuthConfig
            {
                ApiKey = webApiKey,
                AuthDomain = authDomain,
                Providers = new FirebaseAuthProvider[]
                   {
                        new GoogleProvider().AddScopes("email"),
                        new EmailProvider(),
                   }
            };
            var client = new FirebaseAuthClient(authConfig);
            var auth = await client.CreateUserWithEmailAndPasswordAsync(email, password);
        }

        private async Task  AddUserToDatabase()
        {
            FirebaseClient firebaseClient = new FirebaseClient(databaseUrl);
            await firebaseClient.Child("Users").PostAsync(new AppUser
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                ProfileImage = "cat_user.jpg",

            });
        }

        private async void RegisterUserTappedAsync(object obj)
        {
            try
            {
                await CreateFirebaseUser();
                await AddUserToDatabase();
                await _navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Alert", ex.Message, "OK");
                throw;
            }
            await _navigation.PushAsync(new RegisterPage());
        }
    }
}
