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

        public static async Task<bool> AddEventToUser(string eventName)
        {
            var firebaseClient = new FirebaseClient(databaseUrl);
            var userEmail = FirebaseAuthenticationService.GetLoggedUserEmail();
            var userId = await GetUserIdByEmail(userEmail);
            var toUpdateUser = await GetUserByEmail(userEmail);
            if (toUpdateUser != null)
            {
                if (toUpdateUser.AttendingEvents == null)
                {
                    toUpdateUser.AttendingEvents = new List<Ticket>();
                }
                toUpdateUser.AttendingEvents.Add(new Ticket
                {
                    EventName = eventName,
                    BuyDate = DateTime.Now
                });
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
