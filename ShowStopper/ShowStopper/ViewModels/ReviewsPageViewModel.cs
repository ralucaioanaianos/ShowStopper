using Microsoft.Maui.Devices.Sensors;
using ShowStopper.Models;
using ShowStopper.Services;
using ShowStopper.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowStopper.ViewModels
{
    internal class ReviewsPageViewModel : INotifyPropertyChanged
    {
        private AppLocation _location;

        public Command BackBtn { get; }
        public Command PlusBtn { get; }
        public Command GiveReviewBtn { get; }

        private ObservableCollection<LocationReview> _reviews;
        public ObservableCollection<LocationReview> Reviews
        {
            get { return _reviews; }
            set
            {
                _reviews = value;
                OnPropertyChanged(nameof(Reviews));
            }
        }

        private INavigation _navigation;

        private async void BackButtonTappedAsync(object parameter)
        {
            await _navigation.PopAsync();
        }

        private async void PlusButtonTappedAsync(object parameter)
        {
            
        }

        public ReviewsPageViewModel(INavigation navigation, AppLocation location)
        {
            LoadReviews();
            _navigation = navigation;
            _location = location;
            BackBtn = new Command(BackButtonTappedAsync);
            PlusBtn = new Command(PlusButtonTappedAsync);
            GiveReviewBtn = new Command(GiveReviewTappedAsync);
        }

        private async void GiveReviewTappedAsync()
        {
            await _navigation.PushAsync(new GiveReviewPage(_location));
        }

        private async void LoadReviews()
        {
            try
            {
                List<LocationReview> list = await LocationsService.GetLocationReviews(_location);
                ObservableCollection<LocationReview> collection = new ObservableCollection<LocationReview>(list);

                Reviews = collection;
                await Task.Delay(1000);
                if (Reviews.Count == 0)
                {
                    await Application.Current.MainPage.DisplayAlert("prolr", "norev", "ok");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("LoadReviews", ex.Message, "ok");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
