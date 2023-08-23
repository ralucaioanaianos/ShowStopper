using Firebase.Database;
using Firebase.Database.Query;
using Microsoft.Maui.ApplicationModel.Communication;
using Microsoft.Maui.Devices.Sensors;
using ShowStopper.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Xml.Linq;

namespace ShowStopper.Services
{
    class EventsService
    {
        public string webApiKey = "AIzaSyCBEbT1yT0WqRG6Rsts6dYdMz5OQ9dBHVM";
        private static string databaseUrl = "https://showstopper-71398-default-rtdb.europe-west1.firebasedatabase.app/";

        public static async Task addEventToDatabase(string name, string description, string type, DateTime date, string location, int price, string photo)
        {
            try
            {
                FirebaseClient firebaseClient = new FirebaseClient(databaseUrl);
                string email = FirebaseAuthenticationService.GetLoggedUserEmail();
                var newEmail = email.Replace('.', ',');
                var response = await firebaseClient.Child("Events").PostAsync(new AppEvent
                {
                    Name = name,
                    Location = location,
                    Date = date,
                    Organizer = newEmail,
                    Type = type,
                    Description = description,
                    Price = price,
                    Image = photo,
                });
                await Application.Current.MainPage.DisplayAlert("aa", name + ' ' + location + ' ' + date + ' ' + newEmail + ' ' + location, "ok");
                string eventId = response.Key;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("addEvent", ex.Message, "ok");
            }
        }

        public static async Task<int> getEventsCount()
        {
            var firebaseClient = new FirebaseClient(databaseUrl);
            int count = 0;
            var eventQuery = firebaseClient
                .Child("Events").AsObservable<AppEvent>().Subscribe(e =>
                {
                    count++;
                });
            await Task.Delay(TimeSpan.FromSeconds(2)); // Delay to allow time for events to be populated
            eventQuery.Dispose();
            return count;
        }

        public static async Task<List<AppEvent>> getEventsByEmail(string email)
        {
            var firebaseClient = new FirebaseClient(databaseUrl);
            var events = new ConcurrentBag<AppEvent>();
            var newEmail = email.Replace('.', ',');
            var eventQuery = firebaseClient
                .Child("Events").AsObservable<AppEvent>().Subscribe(e =>
                {
                    Console.WriteLine(e.Object.Organizer + ' ' + email);
                    if (e.Object.Organizer == newEmail)
                    {
                        Console.WriteLine("found");
                        events.Add(e.Object);

                    }
                });
            await Task.Delay(TimeSpan.FromSeconds(2)); // Delay to allow time for events to be populated
            eventQuery.Dispose();
            return events.ToList();
        }

        public static async Task<List<AppEvent>> getAllEvents()
        {
            var firebaseClient = new FirebaseClient(databaseUrl);
            var eventsTask = firebaseClient.Child("Events").OnceAsync<AppEvent>();
            var locationsTask = firebaseClient.Child("Locations").OnceAsync<AppLocation>();

            await Task.WhenAll(eventsTask, locationsTask);

            var events = eventsTask.Result.Select(snapshot => snapshot.Object).ToList();
            var locations = locationsTask.Result.Select(snapshot => snapshot.Object).ToList();

            await Application.Current.MainPage.DisplayAlert("LOCATIONS COUNT", locations.Count.ToString(), "OK");

            return events;
        }

        public static async Task AddEventToFavorites(AppEvent appEvent)
        {
            try
            {
                FirebaseClient firebaseClient = new FirebaseClient(databaseUrl);
                string email = FirebaseAuthenticationService.GetLoggedUserEmail();
                var newEmail = email.Replace('.', ',');
                var response = await firebaseClient.Child("EventFavorites").PostAsync(new EventFavorite
                {
                    EventId = appEvent.Id,
                    UserEmail = newEmail,
             
                });
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("addFavoriteEvent", ex.Message, "ok");
            }
        }

        
    }
}
