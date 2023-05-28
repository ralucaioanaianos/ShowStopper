﻿using Firebase.Auth;
using Firebase.Database;
using ShowStopper.Models;
using ShowStopper.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowStopper.ViewModels
{
    class EditProfilePageViewModel
    {

        private INavigation _navigation;

        private User _user;

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EmailAddress { get; set; }

        AppUser DatabaseUser { get; set; }

        public Command SaveBtn { get; }

        public ImageSource ProfileImageSource { get; set; }

        public EditProfilePageViewModel(INavigation navigation, User user, AppUser databaseUser)
        {
            DatabaseUser = databaseUser;
            _navigation = navigation;
            _user = user;
            FirstName = databaseUser.FirstName;
            LastName = databaseUser.LastName;
            EmailAddress = databaseUser.Email;
            string profileImageUrl = databaseUser.ProfileImage;
            SaveBtn = new Command(SaveBtnTappedAsync);
        }

        private async void SaveBtnTappedAsync(object parameter)
        {
            await FirebaseDatabaseService.UpdateUserData(DatabaseUser, FirstName, LastName);
            await _navigation.PopAsync();
        }
    }
}