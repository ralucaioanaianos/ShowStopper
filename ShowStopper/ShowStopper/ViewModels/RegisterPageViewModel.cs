using Firebase.Auth;
using Firebase.Auth.Providers;
using Firebase.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShowStopper.Models;
using Firebase.Database.Query;
using Firebase.Storage;
using System.Runtime.CompilerServices;

namespace ShowStopper.ViewModels
{
    internal class RegisterPageViewModel
    {
        public string webApiKey = "AIzaSyCBEbT1yT0WqRG6Rsts6dYdMz5OQ9dBHVM";

        private string storageUrl = "showstopper-71398.appspot.com";
        private string authDomain = "showstopper-71398.firebaseapp.com";
        private string databaseUrl = "https://showstopper-71398-default-rtdb.europe-west1.firebasedatabase.app/";
        private INavigation _navigation;
        private string email;
        private string password;
        private string firstName;
        private string lastName;
        private FileResult photo;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Email
        {
            get => email;
            set
            {
                email = value;
                RaisePropertyChanged("Email");
            }
        }

        public string FirstName
        {
            get => firstName;
            set
            {
                firstName = value;
                RaisePropertyChanged("FirstName");
            }
        }

        public string LastName
        {
            get => lastName;
            set
            {
                lastName = value;
                RaisePropertyChanged("LastName");
            }
        }

        public string Password
        {
            get => password; set
            {
                password = value;
                RaisePropertyChanged("Password");
            }
        }

        public Command RegisterUser { get; }

        public Command SelectPhoto { get; }

        private void RaisePropertyChanged(string v)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(v));
        }

        public RegisterPageViewModel(INavigation navigation)
        {
            _navigation = navigation;
            SelectPhoto = new Command(SelectPhotoTappedAsync);
            RegisterUser = new Command(RegisterUserTappedAsync);
        }

        private async Task CreateUserFirebase()
        {
            FirebaseAuthConfig authConfig = new FirebaseAuthConfig
            {
                ApiKey = webApiKey,
                AuthDomain = authDomain,
                Providers = new FirebaseAuthProvider[]
                   {
                        new GoogleProvider().AddScopes("email"),
                        new EmailProvider(),
                   }
            };
            var client = new FirebaseAuthClient(authConfig);
            var auth = await client.CreateUserWithEmailAndPasswordAsync(email, password);
        }

        private async Task  AddUserToDatabase(string photoUrl)
        {
            FirebaseClient firebaseClient = new FirebaseClient(databaseUrl);
            await firebaseClient.Child("Users").PostAsync(new AppUser
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                ProfileImage = photoUrl,
                UserType = "User",
            });
        }

        private async void RegisterUserTappedAsync(object obj)
        {
            try
            {
                await CreateUserFirebase();
                if (photo != null)
                {
                    string photoUrl = await UploadPhotoToStorage(photo);
                    //await SavePhotoToDatabase(photoUrl);
                    await AddUserToDatabase(photoUrl);
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("error", "please upload photo", "ok");
                }
                
                await _navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Alert", ex.Message, "OK");
                throw;
            }
            await _navigation.PushAsync(new RegisterPage());
        }

        private async Task<string> UploadPhotoToStorage(FileResult photo)
        {
            // Upload the photo to Firebase Storage
            string fileName = Path.GetFileName(photo.FullPath);
            string storagePath = "photos/" + fileName;

            var storage = new FirebaseStorage(storageUrl);
            var photoStream = await photo.OpenReadAsync();
            var photoUrl = await storage.Child(storagePath).PutAsync(photoStream);
            return photoUrl;
        }

        private async Task SavePhotoToDatabase(string photoUrl)
        {
            // Save the photo URL to Firebase Realtime Database
            var database = new FirebaseClient(databaseUrl);
            var photosNode = database.Child("photos");
            await photosNode.PostAsync(new { Url = photoUrl });
        }

        private async void SelectPhotoTappedAsync(object sender)
        {
            try
            {
                photo = await MediaPicker.PickPhotoAsync();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("error", ex.Message, "ok");
            }
        }

    }
}
