using Firebase.Auth;
using Firebase.Auth.Providers;
using Firebase.Auth.Repository;
using Firebase.Auth.Requests;
using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Storage;
using Newtonsoft.Json;
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
    internal class LoginPageViewModel : INotifyPropertyChanged
    {
        private INavigation _navigation;
        private string userName;
        private string userPassword;
        public Command ForgottenPasswordBtn { get; }
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
            ForgottenPasswordBtn = new Command(ForgottenPasswordBtnTappedAsync);
        }

        public async void ForgottenPasswordBtnTappedAsync(object parameter)
        {
            string userInput = await Application.Current.MainPage.DisplayPromptAsync("Reset Password", "You will receive an email with the instructions for resetting your password.\n\nPlease enter your email:", "OK", "Cancel", "Email");

            if (!string.IsNullOrWhiteSpace(userInput))
            {
                await FirebaseAuthenticationService.ResetPasswordWithEmail(userInput);
            }
            
            
        }

        private async void LoginBtnTappedAsync(object obj)
        {
            try
            {
                User loggedUser = await FirebaseAuthenticationService.LoginUserFirebase(UserName, UserPassword, _navigation);
                AppUser foundUser = await FirebaseDatabaseService.LookForUserInDatabase(loggedUser);
                   
                    if (foundUser != null) 
                    {
                    //await _navigation.PushAsync(new ProfilePage(loggedUser, foundUser));
                    await _navigation.PushAsync(new TabbarPage());
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
