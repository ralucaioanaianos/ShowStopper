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

        public static async Task addEventToDatabase(string name, string description, string type, string date, string location)
        {

            try
            {
                FirebaseClient firebaseClient = new FirebaseClient(databaseUrl);
                string email = FirebaseAuthenticationService.GetLoggedUserEmail();
                //AppUser currentUser = await getUserByEmail(email);
                var newEmail = email.Replace('.', ',');
                var response = await firebaseClient.Child("Events").PostAsync(new AppEvent
                {
                    Name = name,
                    Location = location,
                    Date = date,
                    Organizer = newEmail,
                    Type = type,
                    Description = description
                });
                await Application.Current.MainPage.DisplayAlert("aa", name + ' ' + location + ' ' + date + ' ' + newEmail + ' ' + location, "ok");
                string eventId = response.Key;
            }catch(Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("addEvent", ex.Message, "ok");
            }
           
        }

        public static async Task addLocationToDatabase(string name, string address, string description, string owner)
        {
            FirebaseClient firebaseClient = new FirebaseClient(databaseUrl);
            var response = await firebaseClient.Child("Locations").PostAsync(new AppLocation
            {
                Name = name,
                Address = address,  
                Description = description,
                Owner = owner
            });
            string locationId = response.Key;
        }

        public static async Task AddUserToDatabase(string phoneNumber, string firstName, string lastName, string email, string photoUrl, string userType, string companyName)
        {
            FirebaseClient firebaseClient = new FirebaseClient(databaseUrl);
            var response = await firebaseClient.Child("Users").PostAsync(new AppUser
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
            var replacedEmail = loggedUser.Info.Email.Replace('.', ',');
            var foundElements = firebaseObjects
                .Where(obj => obj.Object.Email == replacedEmail).ToList();
            var foundUser = foundElements.FirstOrDefault();
            return foundUser.Object;
        }

        private static async Task<string> GetUserIdByFirstName(string firstName)
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

        public static async Task<List<AppEvent>> getEventsByEmail(string email)
        {
            var firebaseClient = new FirebaseClient(databaseUrl);
            var events = new ConcurrentBag<AppEvent>();
            var newEmail= email.Replace('.',',');
            var eventQuery = firebaseClient
                .Child("Events").AsObservable<AppEvent>().Subscribe(async (e) =>
                {
                    Console.WriteLine(e.Object.Organizer + ' ' + email);
                    if (e.Object.Organizer == newEmail)
                    {
                        Console.WriteLine("found");
                        events.Add(e.Object);

                    }
                });
            await Application.Current.MainPage.DisplayAlert("geteventsbyemail", events.Count.ToString(), "ok");
            return events.ToList();
            // Get a reference to the Firebase Realtime Database
            //try
            //{
            //var firebaseClient = new FirebaseClient(databaseUrl);
            //var newEmail = email.Replace(".", ",");
            //var userQuery = firebaseClient
            //                .Child("Events")
            //                .OrderBy("Organizer")
            //                .EqualTo(newEmail);

            //            // Retrieve the query result
            //var userSnapshot = await userQuery.OnceAsync<List<AppEvent>>();

            //            // Get the user ID from the query result
            //var events = userSnapshot.FirstOrDefault()?.Object;

            //            //return userId;
            //if (events.Count == 0)
            //    {
            //        await Application.Current.MainPage.DisplayAlert("getevents", "events empty", "ok");
            //        return null;

            //    }
            //    return events;
            //} catch (Exception ex)
            //{
            //    await Application.Current.MainPage.DisplayAlert("getevents", ex.Message, "ok");
            //    return null;
            //}


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
            // Get a reference to the Firebase Realtime Database
            var firebaseClient = new FirebaseClient(databaseUrl);

            // Query the database to find the user with the specified email
            var newEmail = email.Replace(".", ",");
            var userQuery = firebaseClient
                .Child("Users")
                .OrderBy("Email")
                .EqualTo(newEmail)
                .LimitToFirst(1);

            // Retrieve the query result
            var userSnapshot = await userQuery.OnceAsync<AppUser>();

            // Get the user ID from the query result
            var user = userSnapshot.FirstOrDefault()?.Object;
            await Application.Current.MainPage.DisplayAlert("GetUserByEmai", user.Email, "OK");

            //return userId;
            return user;
        }


        public static async Task<List<AppEvent>> getAllEvents()
        {
            var firebaseClient = new FirebaseClient(databaseUrl);
            var events = new List<AppEvent>();
            var eventQuery = firebaseClient
                .Child("Events").AsObservable<AppEvent>().Subscribe(e =>
                {
                    events.Add(e.Object);
                });
            await Task.Delay(500);
            return events;


            //var appEvent = new List<AppEvent>();
            //foreach (var firebaseObject in eventSnapshot)
            //{
            //    appEvent = firebaseObject.Object;
            //    await Application.Current.MainPage.DisplayAlert("1", appEvent.First().ToString(), "ok");

            //}
           // return appEvent;
            //FirebaseClient firebaseClient = new FirebaseClient(databaseUrl);
            //try
            //{
            //    var firebaseObjects = await firebaseClient.Child("Events").OnceAsync<AppEvent>();
            //    var foundElements = firebaseObjects
            //        .ToList();
            //    List<AppEvent> events = new List<AppEvent>();
            //    foreach (var foundElement in foundElements)
            //    {
            //        events.Add(foundElement.Object);
            //    }
            //    return events;
            //}
            //catch (Exception ex)
            //{
            //    await Application.Current.MainPage.DisplayAlert("alert getAll", ex.Message, "ok");
            //    return null;
            //}

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
            var userId = await GetUserIdByFirstName(user.FirstName);
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
