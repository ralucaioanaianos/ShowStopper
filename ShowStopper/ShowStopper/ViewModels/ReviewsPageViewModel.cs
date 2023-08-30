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

        private Action _locationReviewedCallback;

        public ReviewsPageViewModel(INavigation navigation, AppLocation location, Action locationReviewedCallback)
        {
            _location = location;
            LoadReviews();
            _navigation = navigation;
            BackBtn = new Command(BackButtonTappedAsync);
            PlusBtn = new Command(PlusButtonTappedAsync);
            GiveReviewBtn = new Command(GiveReviewTappedAsync);
            _locationReviewedCallback = locationReviewedCallback;
        }

        private async void GiveReviewTappedAsync()
        {
            await _navigation.PushAsync(new GiveReviewPage(_location, LoadReviewsAfterReviewAdded, _locationReviewedCallback));
            _locationReviewedCallback.Invoke();
            await LoadReviews();
        }

        private async void LoadReviewsAfterReviewAdded()
        {
            await LoadReviews();
        }

        private async Task LoadReviews()
        {
            try
            {
                _location = await LocationsService.GetLocationByName(_location.Name);
                ObservableCollection<LocationReview> collection = new ObservableCollection<LocationReview>(_location.Reviews);
               
                Reviews = collection;               
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
