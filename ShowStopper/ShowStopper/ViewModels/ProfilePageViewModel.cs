using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Storage;
using ShowStopper.Models;
using ShowStopper.Services;
using ShowStopper.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace ShowStopper.ViewModels
{
    internal class ProfilePageViewModel : INotifyPropertyChanged
    {
        public string ImageName { get; set; }
        public ImageSource SrcImg { get; set; }
        public Command BackBtn { get; }
        public Command PlusBtn { get; }
        private INavigation _navigation;
        private User _user;
        private AppUser _databaseUser;
        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name)); // Raise the PropertyChanged event
                }
            }
        }

        public Command EditProfileBtn { get; }
        public ImageSource ProfileImageSource { get; set; } 

        public ProfilePageViewModel(INavigation navigation)
        {
            //SrcImg = "https://firebasestorage.googleapis.com/v0/b/showstopper-71398.appspot.com/o/profile_images%2Fcat.jpg?alt=media&token=82358ea8-ab06-4942-bd23-cef6151358bf";
            _navigation = navigation;
            Initialize();
            BackBtn = new Command(BackButtonTappedAsync);
            PlusBtn = new Command(PlusButtonTappedAsync);
            EditProfileBtn = new Command(EditProfileBtnTappedAsync);
        }

        private async void Initialize()
        {
            await GetUser();


            //var webClient = new WebClient();
            //string stroageImage = await new FirebaseStorage("showstopper-71398.appspot.com")
            //    .Child("profile_images")
            //    .Child("cat.jpg")
            //    .GetDownloadUrlAsync();
            //string imgurl = stroageImage;
            //byte[] imgBytes = webClient.DownloadData(imgurl);
            //string img = Convert.ToBase64String(imgBytes);
            // var img = ImageSource.FromStream(() => new MemoryStream(imgBytes));
            // await Application.Current.MainPage.DisplayAlert("image", img.ToString(), "ok");
            //SrcImg = img;
            var firebaseStorage = new FirebaseStorage("showstopper-71398.appspot.com");
            var downloadUrl = await firebaseStorage
                .Child("photos")
                .Child(ImageName)
                .GetDownloadUrlAsync();
            int tokenIndex = downloadUrl.IndexOf("token=");

            if (tokenIndex >= 0)
            {
                downloadUrl = downloadUrl.Substring(tokenIndex + 6);
                //Console.WriteLine(token);
            }
            //SrcImg = downloadUrl;
            //SrcImg = "/data/user/0/com.companyname.showstopper/cache/2203693cc04e0be7f4f024d5f9499e13/f7023d0b8cbd4a4ab495d4c99414263a/IMG_20230611_131105.jpg";
            SrcImg = "59c19f7e-2433-4669-87f4-653b5d55a930";
            //await Application.Current.MainPage.DisplayAlert("image", downloadUrl, "ok");


            //var httpClient = new HttpClient();
            //var imageBytes = await httpClient.GetByteArrayAsync(downloadUrl);

            //Stream imageStream = new MemoryStream(imageBytes);
            //var imageSource = ImageSource.FromStream(() => imageStream);
            //SrcImg = imageSource;
            OnPropertyChanged(nameof(Name)); // Notify the UI about the updated Name value
           // SrcImg = await FirebaseStorageService.LoadImages();
        }

        private async void BackButtonTappedAsync(object parameter)
        {
        }

        private async void PlusButtonTappedAsync(object parameter)
        {
        }

        private async Task GetUser()
        {
            string currentEmail = FirebaseAuthenticationService.GetLoggedUserEmail();
            _databaseUser = await FirebaseDatabaseService.GetUserByEmail(currentEmail);
            if (_databaseUser.FirstName == null) {
                _databaseUser = new AppUser
                {
                    FirstName = "nullfirstname",
                    LastName = "nulllastname",
                    Email = currentEmail,
                    UserType = "User",
                };
            }
            Name = _databaseUser.FirstName + " " + _databaseUser.LastName;
            string profileImageUrl = _databaseUser.ProfileImage;
            int startIndex = profileImageUrl.IndexOf("photos%2F") + 9;
            int endIndex = profileImageUrl.LastIndexOf("?alt");

            if (startIndex >= 0 && endIndex >= 0 && endIndex > startIndex)
            {
                ImageName = profileImageUrl.Substring(startIndex, endIndex - startIndex);
            }
        }

        private async void EditProfileBtnTappedAsync(object parameter)
        {
            await _navigation.PushAsync(new EditProfilePage(_user, _databaseUser));
        }

        private async void ImageButton_Clicked(object sender, EventArgs e)
        {
            await _navigation.PushAsync(new EditProfilePage(_user, _databaseUser));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
