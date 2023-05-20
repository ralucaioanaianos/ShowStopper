﻿using Firebase.Auth;
using Firebase.Auth.Providers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowStopper
{
    internal class RegisterViewModel
    {
        public string webApiKey = "AIzaSyCBEbT1yT0WqRG6Rsts6dYdMz5OQ9dBHVM";

        private INavigation _navigation;
        private string email;
        private string password;

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

        public RegisterViewModel(INavigation navigation)
        {
            this._navigation = navigation;

            RegisterUser = new Command(RegisterUserTappedAsync);
        }

        private async void RegisterUserTappedAsync(object obj)
        {
            try
            {
                FirebaseAuthConfig authConfig = new FirebaseAuthConfig
                {
                    ApiKey = webApiKey,
                    AuthDomain = "showstopper-71398.firebaseapp.com",
                    Providers = new FirebaseAuthProvider[]
                    {
                        new GoogleProvider().AddScopes("email"),
                        new EmailProvider(),
                    }
                };
                var client = new FirebaseAuthClient(authConfig);
                var auth = await client.CreateUserWithEmailAndPasswordAsync(email, password);
                //var authProvider = new FirebaseAuthProvider(new FirebaseConfig(webApiKey));
                //var auth = await authProvider.CreateUserWithEmailAndPasswordAsync(Email, Password);
               // string token = auth.FirebaseToken;
                //if (token != null)
                  //  await App.Current.MainPage.DisplayAlert("Alert", "User Registered successfully", "OK");
                await this._navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Alert", ex.Message, "OK");
                throw;
            }
            await this._navigation.PushAsync(new RegisterPage());
        }
    }
}
