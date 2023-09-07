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
        }

        public static async Task<bool> AddEventToUser(string eventName, string eventImage)
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
                    BuyDate = DateTime.Now,
                    Image = eventImage
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

        public static async Task<List<Ticket>> GetTicketsForUser()
        {
            AppUser user = await GetUserByEmail(FirebaseAuthenticationService.GetLoggedUserEmail());  
            List<Ticket> tickets = user.AttendingEvents.ToList();
            return tickets;
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

        public static async Task<AppUser> LookForUserInDatabase(User loggedUser)
        {
            try
            {
                var databaseClient = new FirebaseClient(databaseUrl);
                var firebaseObjects = await databaseClient.Child("Users").OnceAsync<AppUser>();
                var replacedEmail = loggedUser.Info.Email.Replace('.', ',');
                var foundElements = firebaseObjects
                    .Where(obj => obj.Object.Email == replacedEmail).ToList();
                var foundUser = foundElements.FirstOrDefault();
                return foundUser.Object;
            } catch(Exception ex) {
                Console.WriteLine(ex);
                return null;
            }
           
        }
    }
}
