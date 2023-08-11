using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using Microsoft.Maui.ApplicationModel.Communication;
using Microsoft.Maui.Storage;
using ShowStopper.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ShowStopper.Services
{
    class FirebaseDatabaseService
    {
        public string webApiKey = "AIzaSyCBEbT1yT0WqRG6Rsts6dYdMz5OQ9dBHVM";
        private static string databaseUrl = "https://showstopper-71398-default-rtdb.europe-west1.firebasedatabase.app/";

        public static async Task AddUserToDatabase(string phoneNumber, string firstName, string lastName, string email, string photoUrl, string userType, string companyName)
        {
            FirebaseClient firebaseClient = new FirebaseClient(databaseUrl);
            var response = await firebaseClient.Child("Users2").PostAsync(new AppUser
            {
                PhoneNumber = phoneNumber,
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                ProfileImage = photoUrl,
                UserType = userType,
                CompanyName = companyName
            });
            string userId = response.Key;
        }

        public static async Task AddUserToDatabase2(string phoneNumber, string firstName, string lastName, string email, string photoUrl, string userType, string companyName, FileResult photo)
        {
            FirebaseClient firebaseClient = new FirebaseClient(databaseUrl);
            var response = await firebaseClient.Child("Users").PostAsync(new AppUser
            {
                PhoneNumber = phoneNumber,
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Photo = photo,
                UserType = userType,
                CompanyName = companyName,
            });
            string userId = response.Key;
        }

        public static async Task SavePhotoToDatabase(string photoUrl)
        {
            var database = new FirebaseClient(databaseUrl);
            var photosNode = database.Child("photos");
            await photosNode.PostAsync(new { Url = photoUrl });
            ; ;
        }

        public static async Task<AppUser> LookForUserInDatabase(User loggedUser)
        {
            var databaseClient = new FirebaseClient(databaseUrl);
            var firebaseObjects = await databaseClient.Child("Users").OnceAsync<AppUser>();
            var replacedEmail = loggedUser.Info.Email.Replace('.', ',');
            var foundElements = firebaseObjects
                .Where(obj => obj.Object.Email == replacedEmail).ToList();
            var foundUser = foundElements.FirstOrDefault();
            return foundUser.Object;
        }

        private static async Task<string> GetUserIdByEmail(string email)
        {
            var firebaseClient = new FirebaseClient(databaseUrl);
            var replacedEmail = email.Replace(".", ",");
            var userQuery = firebaseClient
                .Child("Users")
                .OrderBy("Email")
                .EqualTo(replacedEmail)
                .LimitToFirst(1);
            var userSnapshot = await userQuery.OnceAsync<AppUser>();
            var userId = userSnapshot.FirstOrDefault()?.Key;
            return userId;
        }

        public static async Task<AppUser> getUserByEmail(string email)
        {
            var firebaseClient = new FirebaseClient(databaseUrl);
            var user = new AppUser();
            var newEmail = email.Replace('.', ',');
            var userQuery = firebaseClient
                .Child("Users").AsObservable<AppUser>().Subscribe(u =>
                {
                    if (u.Object.Email == newEmail)
                      
                    user = u.Object;
                });
            await Task.Delay(500);
            return user;
        }

        public static async Task<AppUser> GetUserByEmail(string email)
        {
            var firebaseClient = new FirebaseClient(databaseUrl);
            var newEmail = email.Replace(".", ",");
            var userQuery = firebaseClient
                .Child("Users")
                .OrderBy("Email")
                .EqualTo(newEmail)
                .LimitToFirst(1);
            var userSnapshot = await userQuery.OnceAsync<AppUser>();
            var user = userSnapshot.FirstOrDefault()?.Object;
            await Application.Current.MainPage.DisplayAlert("GetUserByEmai", user.Email, "OK");
            return user;
        }

        

        public static async Task<bool> UpdateUserData(AppUser user, string firstName, string lastName, string phoneNumber, string companyName)
        {
            var firebaseClient = new FirebaseClient(databaseUrl);
            var userId = await GetUserIdByEmail(user.Email);
            var toUpdateUser = (await firebaseClient
            .Child("Users")
            .OnceAsync<AppUser>()).Where(a => a.Object.FirstName == user.FirstName).FirstOrDefault();

            if (toUpdateUser != null)
            {
                toUpdateUser.Object.LastName = lastName;
                toUpdateUser.Object.FirstName = firstName;
                toUpdateUser.Object.PhoneNumber = phoneNumber;
                toUpdateUser.Object.CompanyName = companyName;
                await firebaseClient
                   .Child("Users")
                   .Child(userId)
                   .PutAsync(toUpdateUser.Object);
                return true;
            }
            else
                return false;
        }
    }
}
