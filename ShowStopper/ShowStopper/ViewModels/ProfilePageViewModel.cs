using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using ShowStopper.Models;
using ShowStopper.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowStopper.ViewModels
{
    internal class ProfilePageViewModel
    {
        private INavigation _navigation;

        private User _user;

        //private FirebaseClient _firebaseClient;

        public string Name { get; set; }

        public Command EditProfileBtn { get; }

        public ImageSource ProfileImageSource { get; set; } 

        public ProfilePageViewModel(INavigation navigation, User user, AppUser databaseUser)
        {
            _navigation = navigation;
            _user = user;
            //_firebaseClient = new FirebaseClient("https://showstopper-71398-default-rtdb.europe-west1.firebasedatabase.app/");
            //FirebaseObject<AppUser> firebaseObject = await _firebaseClient.Child("Users").Child("ddd@gmail.com").OnceSingleAsync<AppUser>();

            Name = databaseUser.FirstName + " " + databaseUser.LastName;
            string profileImageUrl = databaseUser.ProfileImage;
            // ProfileImageSource = profileImageUrl;
            //ProfileImageSource = new UriImageSource { Uri = new Uri(profileImageUrl)};
            //ProfileImageSource = profileImageUrl;
            //ProfileImageSource = ImageSource.FromFile("cat.jpg");
            EditProfileBtn = new Command(EditProfileBtnTappedAsync);
        }

        private async void EditProfileBtnTappedAsync(object parameter)
        {
            await _navigation.PushAsync(new EditProfilePage());
        }

        private async void ImageButton_Clicked(object sender, EventArgs e)
        {
            await _navigation.PushAsync(new EditProfilePage());
        }

        //private static readonly BindableProperty NameProperty
        //  = BindableProperty.Create(nameof(Name), typeof(string), typeof(ProfilePageViewModel));

        //public string Name
        //{
        //    get => (string)GetValue(NameProperty);
        //    set => SetValue(NameProperty, value);
        //}
    }
}
