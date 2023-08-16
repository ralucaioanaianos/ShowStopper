using ShowStopper.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowStopper.ViewModels
{
    internal class AddEventPageViewModel
    {
        private INavigation _navigation;
        public Command BackBtn { get; }
        public Command PlusBtn { get; }

        public string name;
        public string description;
        public string type;
        public string date;
        public string organizer;
        public string location;
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

        public string Type
        {
            get => type;
            set
            {
                type = value;
                RaisePropertyChanged("Type");
            }
        }

        public string Date
        {
            get => date;
            set
            {
                date = value;
                RaisePropertyChanged("Date");
            }
        }

        public string Organizer
        {
            get => organizer;
            set
            {
                organizer = value;
                RaisePropertyChanged("Organizer");
            }
        }

        public string Location
        {
            get => location;
            set
            {
                location = value;
                RaisePropertyChanged("Location");
            }
        }

        private void RaisePropertyChanged(string v)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(v));
        }

        public AddEventPageViewModel(INavigation navigation)
        {
            _navigation = navigation;
            SaveBtn = new Command(SaveBtnTappedAsync);
            string org = "";
            string userEmail = FirebaseAuthenticationService.GetLoggedUserEmail();
            SaveBtn = new Command(SaveBtnTappedAsync);
            BackBtn = new Command(BackButtonTappedAsync);
            SelectPhoto = new Command(SelectPhotoTappedAsync);
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

        private async void SaveBtnTappedAsync(object parameter)
        {
            try
            {
                if (photo != null)
                {
                    string photoUrl = await FirebaseStorageService.UploadPhotoToStorage(photo);
                    await EventsService.addEventToDatabase(Name, Description, Type, Date, Location, photoUrl);
                }
                await _navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Alert", ex.Message, "OK");
            }
        }

        private async void BackButtonTappedAsync(object parameter)
        {
            await _navigation.PopAsync();
        }

        private async void PlusButtonTappedAsync(object parameter)
        {
        }
    }
}
