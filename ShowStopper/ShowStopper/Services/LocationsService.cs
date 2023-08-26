using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using Microsoft.Maui.Devices.Sensors;
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
                    Image = image,
                    Rating = 0,
                    RatingsNumber = 0,
                    Reviews = new List<LocationReview>()
                });
                string locationId = response.Key;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("addLocation", ex.Message, "ok");
            }
        }

        public static async Task<List<LocationReview>> GetLocationReviews(AppLocation location)
        {
            try
            {
                
                return location.Reviews;
            } catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("get rev", ex.Message, "ok");
                return null;
            }
        }

        public static async Task<List<AppLocation>> getLocationsByEmail(string email)
        {
            try
            {
                var firebaseClient = new FirebaseClient(databaseUrl);
                var locations = new List<AppLocation>();
                var newEmail = email.Replace('.', ',');
                var locationsQuery = firebaseClient
                    .Child("Locations")
                    .OrderBy("Owner")
                    .EqualTo(newEmail)
                    .OnceAsync<AppLocation>();

                var locationSnapshots = await locationsQuery;
                foreach (var snapshot in locationSnapshots)
                {
                    locations.Add(snapshot.Object);
                }
                return locations;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("getLocation", ex.Message, "OK");
                return null;
            }
        }
            

        public static async Task<List<AppLocation>> getAllLocations()
        {
            var firebaseClient = new FirebaseClient(databaseUrl);
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

        private static async Task<string> GetLocationIdByName(string name)
        {
            var firebaseClient = new FirebaseClient(databaseUrl);
            var locationQuery = firebaseClient
                .Child("Locations")
                .OrderBy("Name")
                .EqualTo(name)
                .LimitToFirst(1);
            var locationSnapshot = await locationQuery.OnceAsync<AppLocation>();
            var locationId = locationSnapshot.FirstOrDefault()?.Key;
            return locationId;
        }

        public static async Task ReviewLocation(AppLocation appLocation, int rating, string message)
        {
            try
            {
                string email = FirebaseAuthenticationService.GetLoggedUserEmail();
                var locationId = await GetLocationIdByName(appLocation.Name);
                var firebaseClient = new FirebaseClient(databaseUrl);
                var newEmail = email.Replace('.', ',');
                var toReviewLocation = (await firebaseClient
                .Child("Locations")
            .OnceAsync<AppLocation>()).Where(a => a.Object.Name == appLocation.Name).FirstOrDefault();
                if (toReviewLocation != null)
                {
                    if (toReviewLocation.Object.Reviews == null)
                    {
                        toReviewLocation.Object.Reviews = new List<LocationReview>
                        {
                            new LocationReview
                            {
                                Rating = rating,
                                Message = message,
                                Email = newEmail
                            }
                        };
                    }
                    else
                    {
                        toReviewLocation.Object.Reviews.Add(new LocationReview
                        {
                            Rating = rating,
                            Message = message,
                            Email = newEmail
                        });
                    }
                }
                toReviewLocation.Object.RatingsNumber += 1;
                toReviewLocation.Object.Rating = (toReviewLocation.Object.Rating + rating) / toReviewLocation.Object.RatingsNumber;
                await firebaseClient
                    .Child("Locations")
                    .Child(locationId)
                    .PutAsync(toReviewLocation.Object);

            }catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("review location", ex.Message, "ok");
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
