using ShowStopper.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ShowStopper.ViewModels
{
    internal class AddLocationPageViewModel
    {
        private INavigation _navigation;
        public Command BackBtn { get; }
        public Command PlusBtn { get; }
        public string name;
        public string description;
        public string owner;
        public string address;
        private FileResult photo;
        public event PropertyChangedEventHandler PropertyChanged;
        public Command SaveBtn { get; }
        public Command SelectPhoto { get; }
        public string Name
        {
            get => name;
            set
            {
                name = value;
                RaisePropertyChanged("Name");
            }
        }
        public string Description
        {
            get => description;
            set
            {
                description = value;
                RaisePropertyChanged("Description");
            }
        }
        public string Owner
        {
            get => owner;
            set
            {
                owner = value;
                RaisePropertyChanged("Owner");
            }
        }
        public string Address
        {
            get => address;
            set
            {
                address = value;
                RaisePropertyChanged("Address");
            }
        }
        private Action _locationSavedCallback;

        public AddLocationPageViewModel(INavigation navigation, Action locationSavedCallback)
        {
            _navigation = navigation;
            SaveBtn = new Command(SaveBtnTappedAsync);
            string userEmail = FirebaseAuthenticationService.GetLoggedUserEmail();
            SaveBtn = new Command(SaveBtnTappedAsync);
            BackBtn = new Command(BackButtonTappedAsync);
            SelectPhoto = new Command(SelectPhotoTappedAsync);
            _locationSavedCallback = locationSavedCallback;
        }

        private void RaisePropertyChanged(string v)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(v));
        }

        private async void SelectPhotoTappedAsync(object sender)
        {
            try
            {
                photo = await MediaPicker.PickPhotoAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private async void SaveBtnTappedAsync(object parameter)
        {
            try
            {
                if (photo != null)
                {
                    string photoUrl = await FirebaseStorageService.UploadPhotoToStorage(photo);
                    await LocationsService.addLocationToDatabase(Name, Description, Address, photoUrl);
                }
                _locationSavedCallback?.Invoke();
                await _navigation.PopAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
            }
        }

        private async void BackButtonTappedAsync(object parameter)
        {
            await _navigation.PopAsync();
        }
    }
}
