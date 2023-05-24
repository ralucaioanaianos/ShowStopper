using Firebase.Database;
using Firebase.Database.Query;
using Microsoft.Maui.ApplicationModel.Communication;
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
            await firebaseClient.Child("Users").PostAsync(new AppUser
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                ProfileImage = photoUrl,
                UserType = userType,
            });
        }
        public static async Task SavePhotoToDatabase(string photoUrl)
        {
            // Save the photo URL to Firebase Realtime Database
            var database = new FirebaseClient(databaseUrl);
            var photosNode = database.Child("photos");
            await photosNode.PostAsync(new { Url = photoUrl });
        }
    }
}
