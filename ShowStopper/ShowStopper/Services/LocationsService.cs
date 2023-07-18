using Firebase.Database;
using Firebase.Database.Query;
using ShowStopper.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowStopper.Services
{
    class LocationsService
    {
        public string webApiKey = "AIzaSyCBEbT1yT0WqRG6Rsts6dYdMz5OQ9dBHVM";
        private static string databaseUrl = "https://showstopper-71398-default-rtdb.europe-west1.firebasedatabase.app/";

        public static async Task addLocationToDatabase(string name, string description, string address)
        {
            try
            {
                FirebaseClient firebaseClient = new FirebaseClient(databaseUrl);
                string email = FirebaseAuthenticationService.GetLoggedUserEmail();
                var newEmail = email.Replace('.', ',');
                var response = await firebaseClient.Child("Locations").PostAsync(new AppLocation
                {
                    Name = name,
                    Description = description,
                    Address = address,
                    Owner = newEmail
                });
                string locationId = response.Key;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("addLocation", ex.Message, "ok");
            }
        }

        public static async Task<List<AppLocation>> getLocationsByEmail(string email)
        {
            try
            {
                var firebaseClient = new FirebaseClient(databaseUrl);
                var locations = new ConcurrentBag<AppLocation>();
                var newEmail = email.Replace('.', ',');
                var locationsQuery = firebaseClient
                    .Child("Locations").AsObservable<AppLocation>().Subscribe(async (e) =>
                    {
                        Console.WriteLine(e.Object.Owner + ' ' + email);
                        if (e.Object.Owner == newEmail)
                        {
                            locations.Add(e.Object);
                        }
                    });
                await Task.Delay(TimeSpan.FromSeconds(2));
                locationsQuery.Dispose();
                return locations.ToList();
            } catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("getLocation", ex.Message, "ok");
                return null;
            }
        }
            

        public static async Task<List<AppLocation>> getAllLocations()
        {
            var firebaseClient = new FirebaseClient(databaseUrl);
            var locations = new List<AppLocation>();
            var locationQuery = firebaseClient
                .Child("Locations").AsObservable<AppLocation>().Subscribe(l =>
                {
                    locations.Add(l.Object);
                });
            await Task.Delay(500);
            return locations;
        }

        public static async Task AddLocationToFavorites(AppLocation appLocation)
        {
            try
            {
                FirebaseClient firebaseClient = new FirebaseClient(databaseUrl);
                string email = FirebaseAuthenticationService.GetLoggedUserEmail();
                var newEmail = email.Replace('.', ',');
                var response = await firebaseClient.Child("LocationFavorites").PostAsync(new LocationFavorite
                {
                    LocationName = appLocation.Name,
                    UserEmail = newEmail,

                });
                await Application.Current.MainPage.DisplayAlert("added favorite location", "yey", "ok");

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("addFavoriteLocation", ex.Message, "ok");
            }
        }
    }
}
