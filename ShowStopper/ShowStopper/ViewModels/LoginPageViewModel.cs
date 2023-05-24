using Firebase.Auth;
using Firebase.Auth.Providers;
using Firebase.Auth.Repository;
using Firebase.Auth.Requests;
using Firebase.Database;
using Firebase.Database.Query;
using Newtonsoft.Json;
using ShowStopper.Models;
using ShowStopper.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowStopper.ViewModels
{
    internal class LoginPageViewModel : INotifyPropertyChanged
    {
        public string webApiKey = "AIzaSyCBEbT1yT0WqRG6Rsts6dYdMz5OQ9dBHVM";

        private string authDomain = "showstopper-71398.firebaseapp.com";
        private string databaseUrl = "https://showstopper-71398-default-rtdb.europe-west1.firebasedatabase.app/";
        private INavigation _navigation;
        private string userName;
        private string userPassword;

        public event PropertyChangedEventHandler PropertyChanged;

        public Command RegisterBtn { get; }
        public Command LoginBtn { get; }

        public string IsVisible { get; set; }
        
        public string UserName
        {
            get => userName; set
            {
                userName = value;
                RaisePropertyChanged("UserName");
            }
        }

        public string UserPassword
        {
            get => userPassword; set
            {
                userPassword = value;
                RaisePropertyChanged("UserPassword");
            }
        }

        public LoginPageViewModel(INavigation navigation, string isVisible)
        {
            IsVisible = isVisible;
            _navigation = navigation;
            RegisterBtn = new Command(RegisterBtnTappedAsync);
            LoginBtn = new Command(LoginBtnTappedAsync);
        }

        private async Task<User> LoginUserFirebase()
        {
            User user = null;
            FirebaseAuthConfig authConfig = new FirebaseAuthConfig
            {
                ApiKey = webApiKey,
                AuthDomain = authDomain,
                Providers = new FirebaseAuthProvider[]
                {
                    new GoogleProvider().AddScopes("email"),
                    new EmailProvider()
                },
                UserRepository = new FileUserRepository("FirebaseSample"),

            };
            var client = new FirebaseAuthClient(authConfig);
            var result = await client.FetchSignInMethodsForEmailAsync(UserName);
            if (result == null || !result.UserExists)
            {
                await _navigation.PushAsync(new LoginPage("True"));

            }
            else
            {
                var userCredential = await client.SignInWithEmailAndPasswordAsync(UserName, UserPassword);
                user = userCredential.User;
            }
            return user;
        }

        private async Task<AppUser> LookForUserInDatabase(User loggedUser)
        {
            var databaseClient = new FirebaseClient(databaseUrl);
            var firebaseObjects = await databaseClient.Child("Users").OnceAsync<AppUser>();
            var foundElements = firebaseObjects
                .Where(obj => obj.Object.Email == loggedUser.Info.Email).ToList();
            var foundUser = foundElements.FirstOrDefault();
            return foundUser.Object;
        }

        private async void LoginBtnTappedAsync(object obj)
        {
            try
            {
                    User loggedUser = await LoginUserFirebase();
                    AppUser foundUser = await LookForUserInDatabase(loggedUser);
                   
                    if (foundUser != null) 
                    {
                        //user.Info.FirstName = retrievedUser.FirstName;
                        //user.Info.LastName = retrievedUser.LastName;
                        await _navigation.PushAsync(new ProfilePage(loggedUser, foundUser));
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("aa", "element not retrieed", "ok");
                    }
                
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Alert", ex.Message, "OK");
                throw;
            }
        }

        private async void RegisterBtnTappedAsync(object obj)
        {
            await _navigation.PushAsync(new RegisterPage());
        }

        private void RaisePropertyChanged(string v)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(v));
        }
    }
}
