using Firebase.Database;
using Firebase.Database.Query;
using Microsoft.Maui.ApplicationModel.Communication;
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

        public static async Task addLocationToDatabase(string name, string description, string address, string image)
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
                    Owner = newEmail,
                    Image = image
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
                await Application.Current.MainPage.DisplayAlert("count email", locations.Count.ToString(), "ok");

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
            //var eventQuery = firebaseClient
            //    .Child("Locations").AsObservable<AppLocation>().Subscribe(e =>
            //    {
            //        locations.Add(e.Object);
            //    });
            //await Task.Delay(500);
            //await Application.Current.MainPage.DisplayAlert("count all", locations.Count.ToString(), "ok");

            //return locations;
            var locationsTask = firebaseClient.Child("Locations").OnceAsync<AppLocation>();
            await Task.WhenAll(locationsTask);
            var locations = locationsTask.Result.Select(snapshot => snapshot.Object).ToList();
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

        public static async Task RemoveLocationFromFavorites(AppLocation appLocation)
        {
            try
            {
                var firebaseClient = new FirebaseClient(databaseUrl);
                var locations = new ConcurrentBag<AppLocation>();
                string email = FirebaseAuthenticationService.GetLoggedUserEmail();
                var newEmail = email.Replace('.', ',');
                var locationFavorites = await firebaseClient
            .Child("LocationFavorites")
            .OrderBy("LocationName")
            .EqualTo(appLocation.Name)
            .OnceAsync<LocationFavorite>();

                // Iterate through the matching favorites and remove them
                foreach (var favorite in locationFavorites)
                {
                    if (favorite.Object.UserEmail == newEmail)
                    {
                        await firebaseClient.Child("LocationFavorites").Child(favorite.Key).DeleteAsync();
                    }
                }
                await Application.Current.MainPage.DisplayAlert("removed favorite location", "yey", "ok");

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("delete location favorite", ex.Message, "ok");
            }
        }

        public static async Task<bool> IsLocationInFavorites(AppLocation appLocation)
        {
            try
            {
                bool isAdded = false;
                var firebaseClient = new FirebaseClient(databaseUrl);
                var locations = new ConcurrentBag<AppLocation>();
                string email = FirebaseAuthenticationService.GetLoggedUserEmail();
                var newEmail = email.Replace('.', ',');
                var locationFavorites = await firebaseClient
            .Child("LocationFavorites")
            .OrderBy("LocationName")
            .EqualTo(appLocation.Name)
            .OnceAsync<LocationFavorite>();

                // Iterate through the matching favorites and remove them
                foreach (var favorite in locationFavorites)
                {
                    if (favorite.Object.UserEmail == newEmail)
                    {
                        isAdded = true;
                    }
                }
                return isAdded;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("check location favorite", ex.Message, "ok");
                return false;
            }
        }

        public static async Task<AppLocation> GetLocationByName(string name)
        {
            try
            {
                var firebaseClient = new FirebaseClient(databaseUrl);
                var locationQuery = firebaseClient
                    .Child("Locations")
                    .OrderBy("Name")
                    .EqualTo(name)
                    .LimitToFirst(1);
                var locationSnapshot = await locationQuery.OnceAsync<AppLocation>();
                var location = locationSnapshot.FirstOrDefault()?.Object;
                await Application.Current.MainPage.DisplayAlert("GetUserByEmai", location.Name, "OK");
                return location;
            } catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("getlocation by name", ex.Message, "ok");
                return null;
            }
            }
            

        public static async Task<List<LocationFavorite>> GetFavoriteLocationsByEmail(string email)
        {
            try
            {
                var firebaseClient = new FirebaseClient(databaseUrl);
                var favoriteLocations = new ConcurrentBag<LocationFavorite>();
                var newEmail = email.Replace('.', ',');
                var locationsQuery = firebaseClient
                    .Child("LocationFavorites").AsObservable<LocationFavorite>().Subscribe(async (e) =>
                    {
                        Console.WriteLine(e.Object.UserEmail + ' ' + email);
                        if (e.Object.UserEmail == newEmail)
                        {
                            favoriteLocations.Add(e.Object);
                        }
                    });
                await Task.Delay(TimeSpan.FromSeconds(2));
                locationsQuery.Dispose();
                return favoriteLocations.ToList();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("getLocationfavorite", ex.Message, "ok");
                return null;
            }
        }
    }
}
