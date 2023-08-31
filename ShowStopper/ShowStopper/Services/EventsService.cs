﻿using Firebase.Database;
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

        public static async Task addEventToDatabase(string name, string description, string type, DateTime date, string location, int price, int totalPlaces, string photo)
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
                    TotalPlaces = totalPlaces,
                    AvailablePlaces = totalPlaces
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
            try
            {
                var firebaseClient = new FirebaseClient(databaseUrl);
                var events = new List<AppEvent>();
                var newEmail = email.Replace('.', ',');
                var eventsQuery = firebaseClient
                        .Child("Events")
                        .OrderBy("Organizer")
                        .EqualTo(newEmail)
                        .OnceAsync<AppEvent>();
                var eventsSnapshot = await eventsQuery;
                foreach (var snapshot in eventsSnapshot)
                {
                    events.Add(snapshot.Object);
                }
                return events;
            } catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("get events", ex.Message, "ok");
                return null;
            }
            
        }

        public static async Task<List<AppEvent>> getAllEvents()
        {
            try
            {
                var firebaseClient = new FirebaseClient(databaseUrl);
                var events = new List<AppEvent>();
                var eventsQuery = firebaseClient
                        .Child("Events")
                        .OnceAsync<AppEvent>();
                var eventsSnapshot = await eventsQuery;
                foreach (var snapshot in eventsSnapshot)
                {
                    events.Add(snapshot.Object);
                }
                return events;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("get events", ex.Message, "ok");
                return null;
            }
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
                    EventName = appEvent.Name,
                    UserEmail = newEmail,
                });
                await Application.Current.MainPage.DisplayAlert("added favorite event", "yey", "ok");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("addFavoriteEvent", ex.Message, "ok");
            }
        }

        public static async Task<bool> IsEventInFavorites(AppEvent appEvent)
        {
            try
            {
                bool isAdded = false;
                var firebaseClient = new FirebaseClient(databaseUrl);
                var events = new ConcurrentBag<AppEvent>();
                string email = FirebaseAuthenticationService.GetLoggedUserEmail();
                var newEmail = email.Replace('.', ',');
                var eventFavorites = await firebaseClient
            .Child("EventFavorites")
            .OrderBy("EventName")
            .EqualTo(appEvent.Name)
            .OnceAsync<EventFavorite>();
                foreach (var favorite in eventFavorites)
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

        public static async Task RemoveEventFromFavorites(AppEvent appEvent)
        {
            try
            {
                var firebaseClient = new FirebaseClient(databaseUrl);
                var events = new ConcurrentBag<AppEvent>();
                string email = FirebaseAuthenticationService.GetLoggedUserEmail();
                var newEmail = email.Replace('.', ',');
                var eventFavorites = await firebaseClient
            .Child("EventFavorites")
            .OrderBy("EventName")
            .EqualTo(appEvent.Name)
            .OnceAsync<EventFavorite>();
                foreach (var favorite in eventFavorites)
                {
                    if (favorite.Object.UserEmail == newEmail)
                    {
                        await firebaseClient.Child("EventFavorites").Child(favorite.Key).DeleteAsync();
                    }
                }
                await Application.Current.MainPage.DisplayAlert("removed favorite location", "yey", "ok");

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("delete location favorite", ex.Message, "ok");
            }
        }

        public static async Task<List<EventFavorite>> GetFavoriteEventsByEmail(string email)
        {
            try
            {
                var firebaseClient = new FirebaseClient(databaseUrl);
                var favoriteEvents = new ConcurrentBag<EventFavorite>();
                var newEmail = email.Replace('.', ',');
                var eventsQuery = firebaseClient
                    .Child("EventFavorites").AsObservable<EventFavorite>().Subscribe(async (e) =>
                    {
                        Console.WriteLine(e.Object.UserEmail + ' ' + email);
                        if (e.Object.UserEmail == newEmail)
                        {
                            favoriteEvents.Add(e.Object);
                        }
                    });
                await Task.Delay(TimeSpan.FromSeconds(2));
                eventsQuery.Dispose();
                return favoriteEvents.ToList();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("getLocationfavorite", ex.Message, "ok");
                return null;
            }
        }

        public static async Task<AppEvent> GetEventByName(string name)
        {
            try
            {
                var firebaseClient = new FirebaseClient(databaseUrl);
                var eventQuery = firebaseClient
                    .Child("Events")
                    .OrderBy("Name")
                    .EqualTo(name)
                    .LimitToFirst(1);
                var eventSnapshot = await eventQuery.OnceAsync<AppEvent>();
                var appEvent = eventSnapshot.FirstOrDefault()?.Object;
                return appEvent;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("getlocation by name", ex.Message, "ok");
                return null;
            }
        }
    }
}
