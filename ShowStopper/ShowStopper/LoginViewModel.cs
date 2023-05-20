﻿using Firebase.Auth;
using Firebase.Auth.Providers;
using Firebase.Auth.Repository;
using Firebase.Auth.Requests;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowStopper
{
    internal class LoginViewModel : INotifyPropertyChanged
    {
        public string webApiKey = "AIzaSyCBEbT1yT0WqRG6Rsts6dYdMz5OQ9dBHVM";
        private INavigation _navigation;
        private string userName;
        private string userPassword;

        public event PropertyChangedEventHandler PropertyChanged;

        public Command RegisterBtn { get; }
        public Command LoginBtn { get; }

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

        public LoginViewModel(INavigation navigation)
        {
            this._navigation = navigation;
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
                var userCredential = await client.SignInWithEmailAndPasswordAsync(UserName, UserPassword);
                var user = userCredential.User;
                var uid = user.Uid;
                //var content = await auth.AuthCredential();
                //var content = await auth.GetFreshAuthAsync();
                //var content = await auth.
                //var content = await GetAccountInfo(auth);
                //var serializedContent = JsonConvert.SerializeObject(content);
                //Preferences.Set("FreshFirebaseToken", serializedContent);
                //await this._navigation.PushAsync(new Dashboard());
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Alert", ex.Message, "OK");
                throw;
            }
        }

        private async void RegisterBtnTappedAsync(object obj)
        {
            await this._navigation.PushAsync(new RegisterPage());
        }

        private void RaisePropertyChanged(string v)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(v));
        }
    }
}
