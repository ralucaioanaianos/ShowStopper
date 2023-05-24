using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using Microsoft.Maui.ApplicationModel.Communication;
using Microsoft.Maui.Storage;
using ShowStopper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowStopper.Services
{
    class FirebaseDatabaseService
    {
        public string webApiKey = "AIzaSyCBEbT1yT0WqRG6Rsts6dYdMz5OQ9dBHVM";

        private string authDomain = "showstopper-71398.firebaseapp.com";
        private static string databaseUrl = "https://showstopper-71398-default-rtdb.europe-west1.firebasedatabase.app/";
        private INavigation _navigation;

        public static async Task AddUserToDatabase(string firstName, string lastName, string email, string photoUrl, string userType)
        {
            FirebaseClient firebaseClient = new FirebaseClient(databaseUrl);
            var response = await firebaseClient.Child("Users").PostAsync(new AppUser
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                ProfileImage = photoUrl,
                UserType = userType,
            });
            string userId = response.Key;
        }
        public static async Task SavePhotoToDatabase(string photoUrl)
        {
            // Save the photo URL to Firebase Realtime Database
            var database = new FirebaseClient(databaseUrl);
            var photosNode = database.Child("photos");
            await photosNode.PostAsync(new { Url = photoUrl });
        }

        public static async Task<AppUser> LookForUserInDatabase(User loggedUser)
        {
            var databaseClient = new FirebaseClient(databaseUrl);
            var firebaseObjects = await databaseClient.Child("Users").OnceAsync<AppUser>();
            var foundElements = firebaseObjects
                .Where(obj => obj.Object.Email == loggedUser.Info.Email).ToList();
            var foundUser = foundElements.FirstOrDefault();
            return foundUser.Object;
        }

        private static async Task<string> GetUserIdByEmail(string firstName)
        {
            // Get a reference to the Firebase Realtime Database
            var firebaseClient = new FirebaseClient(databaseUrl);

            // Query the database to find the user with the specified email
            var userQuery = firebaseClient
                .Child("Users")
                .OrderBy("FirstName")
                .EqualTo(firstName)
                .LimitToFirst(1);

            // Retrieve the query result
            var userSnapshot = await userQuery.OnceAsync<AppUser>();

            // Get the user ID from the query result
            var userId = userSnapshot.FirstOrDefault()?.Key;

            //return userId;
            return userId;
        }

        //public static async Task UpdateUserData(AppUser user, string firstName, string lastName, string email)
        //{
        //    var firebaseClient = new FirebaseClient(databaseUrl);
        //    var userId = await GetUserIdByEmail(user.FirstName);
        //    var userNode = firebaseClient.Child("Users").Child(userId);
        //    await Application.Current.MainPage.DisplayAlert("alert", firstName + ' ' + lastName, "ok");
        //     await userNode.Child("LastName").PutAsync(lastName);
        //    //await userNode.Child("LastName").PutAsync(lastName);
        //    //await userNode.Child("Email").PutAsync(email);  
        //}

        public static async Task<bool> UpdateUserData(AppUser user, string firstName, string lastName)
        {
            var firebaseClient = new FirebaseClient(databaseUrl);
            var userId = await GetUserIdByEmail(user.FirstName);
            var toUpdateUser = (await firebaseClient
            .Child("Users")
            .OnceAsync<AppUser>()).Where(a => a.Object.FirstName == user.FirstName).FirstOrDefault();

            if (toUpdateUser != null)
            {
                toUpdateUser.Object.LastName = lastName;
                toUpdateUser.Object.FirstName = firstName;
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
