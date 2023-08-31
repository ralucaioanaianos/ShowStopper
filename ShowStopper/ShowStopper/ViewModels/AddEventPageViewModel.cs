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
        public DateTime TodayDate { get; set; }
        public string name;
        public string description;
        public string type;
        public DateTime date;
        public string organizer;
        public string location;
        private FileResult photo;
        private int price;
        private int totalPlaces;
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
        public int Price
        {
            get => price;
            set
            {
                price = value;
                RaisePropertyChanged("Price");
            }
        }
        public int TotalPlaces
        {
            get => totalPlaces;
            set
            {
                totalPlaces = value;
                RaisePropertyChanged("TotalPlaces");
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
        public DateTime Date
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
        private Action _reviewSavedCallback;

        public AddEventPageViewModel(INavigation navigation, Action reviewSavedCallback)
        {
            _navigation = navigation;
            SaveBtn = new Command(SaveBtnTappedAsync);
            string org = "";
            string userEmail = FirebaseAuthenticationService.GetLoggedUserEmail();
            SaveBtn = new Command(SaveBtnTappedAsync);
            BackBtn = new Command(BackButtonTappedAsync);
            SelectPhoto = new Command(SelectPhotoTappedAsync);
            TodayDate = DateTime.Now;
            _reviewSavedCallback = reviewSavedCallback;

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
                    await EventsService.addEventToDatabase(Name, Description, Type, Date, Location, Price, TotalPlaces, photoUrl);
                }
                _reviewSavedCallback?.Invoke();
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

        private async void PlusButtonTappedAsync(object parameter)
        {}
    }
}
