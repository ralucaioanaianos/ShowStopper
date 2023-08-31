using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using Microsoft.Maui.Storage;
using ShowStopper.Models;
using ShowStopper.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowStopper.ViewModels
{
    class EditProfilePageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private INavigation _navigation;

        private User _user;
        public Command BackBtn { get; }
        public Command PlusBtn { get; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string CompanyName { get; set; }
        AppUser DatabaseUser { get; set; }
        private FileResult Photo;
        public Command SelectPhoto { get; }

        public Command SaveBtn { get; }
        private ImageSource _srcImg;
        public ImageSource SrcImg
        {
            get { return _srcImg; }
            set
            {
                if (_srcImg != value)
                {
                    _srcImg = value;
                    OnPropertyChanged(nameof(SrcImg));
                }
            }
        }
        public ImageSource ProfileImageSource { get; set; }
        private Action _initializeUserAfterEdit;

        public EditProfilePageViewModel(INavigation navigation, User user, AppUser databaseUser, Action initializeUserAfterEdit)
        {
            DatabaseUser = databaseUser;
            SrcImg = databaseUser.ProfileImage;
            _navigation = navigation;
            _user = user;
            FirstName = databaseUser.FirstName;
            LastName = databaseUser.LastName;
            EmailAddress = databaseUser.Email;
            PhoneNumber = databaseUser.PhoneNumber;
            CompanyName = databaseUser.CompanyName;
            string profileImageUrl = databaseUser.ProfileImage;
            SaveBtn = new Command(SaveBtnTappedAsync);
            BackBtn = new Command(BackButtonTappedAsync);
            PlusBtn = new Command(PlusButtonTappedAsync);
            SelectPhoto = new Command(SelectPhotoTappedAsync);
            _initializeUserAfterEdit = initializeUserAfterEdit;
        }

        private async void SelectPhotoTappedAsync(object sender)
        {
            try
            {
                Photo = await MediaPicker.PickPhotoAsync();
                using (Stream stream = await Photo.OpenReadAsync())
                {
                    MemoryStream memoryStream = new MemoryStream();
                    await stream.CopyToAsync(memoryStream);
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    SrcImg = ImageSource.FromStream(() => memoryStream);
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("error", ex.Message, "ok");
            }
        }

        private async void BackButtonTappedAsync(object parameter)
        {
            await _navigation.PopAsync();
        }

        private async void PlusButtonTappedAsync(object parameter)
        {}

        private async void SaveBtnTappedAsync(object parameter)
        {
            string photoUrl = DatabaseUser.ProfileImage;
            if (Photo != null)
            {
                photoUrl = await FirebaseStorageService.UploadPhotoToStorage(Photo);
            }
            await UserService.UpdateUserData(DatabaseUser, FirstName, LastName, PhoneNumber, CompanyName, photoUrl);
            _initializeUserAfterEdit.Invoke();
            await _navigation.PopAsync();
        }
    }
}
