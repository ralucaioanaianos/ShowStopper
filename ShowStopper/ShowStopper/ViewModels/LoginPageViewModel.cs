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

        private async void LoginBtnTappedAsync(object obj)
        {
            FirebaseAuthConfig authConfig = new FirebaseAuthConfig
            {
                ApiKey = webApiKey,
                AuthDomain = "showstopper-71398.firebaseapp.com",
                Providers = new FirebaseAuthProvider[]
                {
                    new GoogleProvider().AddScopes("email"),
                    new EmailProvider()
                },
                UserRepository = new FileUserRepository("FirebaseSample"),

            };
            try
            {
                var client = new FirebaseAuthClient(authConfig);
                var result = await client.FetchSignInMethodsForEmailAsync(UserName);
                if (result == null || !result.UserExists)
                {
                    await _navigation.PushAsync(new LoginPage("True"));

                }
                else
                {
                    var userCredential = await client.SignInWithEmailAndPasswordAsync(UserName, UserPassword);
                    User user = userCredential.User;
                    var uid = user.Uid;
                    var databaseClient = new FirebaseClient("https://showstopper-71398-default-rtdb.europe-west1.firebasedatabase.app/");
                    //AppUser firebaseObject = await databaseClient.Child("Users").OrderByChild("");
                    //var query = await databaseClient.Child("Users").OrderByChild("ddd@gmail.com")
                    var firebaseObjects = await databaseClient.Child("Users").OnceAsync<AppUser>();
                    var filteredElements = firebaseObjects
                        .Where(obj => obj.Object.Email == user.Info.Email).ToList();
                    var firstElement = filteredElements.FirstOrDefault();
                    if (firstElement != null) 
                    {
                        AppUser retrievedUser = firstElement.Object;
                        user.Info.FirstName = retrievedUser.FirstName;
                        user.Info.LastName = retrievedUser.LastName;
                        await Application.Current.MainPage.DisplayAlert("aa", retrievedUser.FirstName, "ok");

                        await _navigation.PushAsync(new ProfilePage(user, retrievedUser));
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("aa", "element not retrieed", "ok");
                    }
                    
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
