using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using ShowStopper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowStopper.Services
{
    class UserService
    {
        public string webApiKey = "AIzaSyCBEbT1yT0WqRG6Rsts6dYdMz5OQ9dBHVM";
        private static string databaseUrl = "https://showstopper-71398-default-rtdb.europe-west1.firebasedatabase.app/";

        public static async Task<bool> AddEvent(string eventName)
        {
            Console.WriteLine("EVENT NAME: " + eventName);
            var firebaseClient = new FirebaseClient(databaseUrl);
            var userEmail = FirebaseAuthenticationService.GetLoggedUserEmail();
            Console.WriteLine($"user email: {userEmail}");  
            var newEmail = userEmail.Replace('.', ',');
            var userId = await GetUserIdByEmail(userEmail);
            Console.WriteLine("USER ID: " + userId);
            var toUpdateUser = await FirebaseDatabaseService.GetUserByEmail(userEmail);
            Console.WriteLine("FOUND USER EMAIL: " + toUpdateUser.Email);
            if (toUpdateUser != null)
            {
                if (toUpdateUser.AttendingEvents == null)
                {
                    toUpdateUser.AttendingEvents = new List<string>();
                }
                toUpdateUser.AttendingEvents.Add(eventName);
                Console.WriteLine(toUpdateUser.AttendingEvents.Count.ToString());
                await firebaseClient
                   .Child("Users")
                   .Child(userId)
                   .PutAsync(toUpdateUser);
                return true;
            }
            else
                return false;
        }

        public static async Task AddEventToUser(AppEvent appEvent)
        {
            try
            {
                FirebaseClient firebaseClient = new FirebaseClient(databaseUrl);
                string email = FirebaseAuthenticationService.GetLoggedUserEmail();
                var newEmail = email.Replace('.', ',');
                Console.WriteLine($"NEW EMAIL: {newEmail}");

                // Fetch the user's data
                //var user = await firebaseClient.Child("Users").Child(newEmail).OnceSingleAsync<AppUser>();
                var user = (await firebaseClient
                            .Child("Users")
                            .OnceAsync<AppUser>()).Where(a => a.Object.Email == newEmail).FirstOrDefault().Object;
                // Add the event name to the AttendingEvents list
                if (user.AttendingEvents == null)
                {
                    user.AttendingEvents = new List<string>();
                }
                user.AttendingEvents.Add(appEvent.Name);
                Console.WriteLine($"EVENT NAME: {appEvent.Name}");
                // Update the user's data
                //await firebaseClient.Child("Users").Child(newEmail).PutAsync(user);
                //await FirebaseDatabaseService.UpdateUserData()
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("addTicket", ex.Message, "ok");
            }
        }



        public static async Task<bool> UpdateUserData(AppUser user, string firstName, string lastName, string phoneNumber, string companyName, string photoUrl)
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
                toUpdateUser.Object.ProfileImage = photoUrl;
                //toUpdateUser.Object.AttendingEvents = attendingEvents;
                await firebaseClient
                   .Child("Users")
                   .Child(userId)
                   .PutAsync(toUpdateUser.Object);
                return true;
            }
            else
                return false;
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
            Console.WriteLine("ENTERED GETUSER");
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

    }
}
