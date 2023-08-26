﻿using ShowStopper.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowStopper.ViewModels
{
    internal class GiveReviewPageViewModel : INotifyPropertyChanged
    {
        private decimal _rating;
        public decimal Rating
        {
            get { return _rating; }
            set
            {
                if (_rating != value)
                {
                    _rating = value;
                    OnPropertyChanged(nameof(Rating));
                }
            }
        }

        public Command SaveBtn { get; }
        public Command BackBtn { get; }
        public Command PlusBtn {  get; }

        public Command Star1Btn { get; set; }
        public Command Star2Btn { get; set; }
        public Command Star3Btn { get; set; }
        public Command Star4Btn { get; set; }
        public Command Star5Btn { get; set; }
        public ImageSource Star1 { get; set; }
        public ImageSource Star2 { get; set; }
        public ImageSource Star3 { get; set; }
        public ImageSource Star4 { get; set; }
        public ImageSource Star5 { get; set; }



        public string Message { get; set; }

        private AppLocation _location { get; set; }

        private INavigation _navigation { get ; set; }

        public GiveReviewPageViewModel(INavigation navigation, AppLocation appLocation)
        {
            _navigation = navigation;
            _location = appLocation;
            Star1 = "empty_star_second.png";
            Star2 = "empty_star_second.png";
            Star3 = "empty_star_second.png";
            Star4 = "empty_star_second.png";
            Star5 = "empty_star_second.png";
            Star1Btn = new Command(Star1BtnTappedAsync);
            Star2Btn = new Command(Star2BtnTappedAsync);
            Star3Btn = new Command(Star3BtnTappedAsync);
            Star4Btn = new Command(Star4BtnTappedAsync);
            Star5Btn = new Command(Star5BtnTappedAsync);


        }

        public  async void Star1BtnTappedAsync()
        {
            Rating = 1;
            Star1 = "star_icon.png";
            OnPropertyChanged(nameof(Star1));
            Star2 = "empty_star_second.png";
            OnPropertyChanged(nameof(Star2));
            Star3 = "empty_star_second.png";
            OnPropertyChanged(nameof(Star3));
            Star4 = "empty_star_second.png";
            OnPropertyChanged(nameof(Star4));
            Star5 = "empty_star_second.png";
            OnPropertyChanged(nameof(Star5));
            await Application.Current.MainPage.DisplayAlert("1", Rating.ToString() + " " + Star1, "ok");
        }
        public  void Star2BtnTappedAsync()
        {
            Rating = 2;
            Star1 = "star_icon.png";
            OnPropertyChanged(nameof(Star1));
            Star2 = "star_icon.png";
            OnPropertyChanged(nameof(Star2));
            Star3 = "empty_star_second.png";
            OnPropertyChanged(nameof(Star3));
            Star4 = "empty_star_second.png";
            OnPropertyChanged(nameof(Star4));
            Star5 = "empty_star_second.png";
            OnPropertyChanged(nameof(Star5));
        }

        public  void Star3BtnTappedAsync()
        {
            Rating = 3;
            Star1 = "star_icon.png";
            OnPropertyChanged(nameof(Star1));
            Star2 = "star_icon.png";
            OnPropertyChanged(nameof(Star2));
            Star3 = "star_icon.png";
            OnPropertyChanged(nameof(Star3));
            Star4 = "empty_star_second.png";
            OnPropertyChanged(nameof(Star4));
            Star5 = "empty_star_second.png";
            OnPropertyChanged(nameof(Star5));
        }

        public  void Star4BtnTappedAsync()
        {
            Rating = 4;
            Star1 = "star_icon.png";
            OnPropertyChanged(nameof(Star1));
            Star2 = "star_icon.png";
            OnPropertyChanged(nameof(Star2));
            Star3 = "star_icon.png";
            OnPropertyChanged(nameof(Star3));
            Star4 = "star_icon.png";
            OnPropertyChanged(nameof(Star4));
            Star5 = "empty_star_second.png";
            OnPropertyChanged(nameof(Star5));
        }

        public void Star5BtnTappedAsync()
        {
            Rating = 5;
            Star1 = "star_icon.png";
            OnPropertyChanged(nameof(Star1));
            Star2 = "star_icon.png";
            OnPropertyChanged(nameof(Star2));
            Star3 = "star_icon.png";
            OnPropertyChanged(nameof(Star3));
            Star4 = "star_icon.png";
            OnPropertyChanged(nameof(Star4));
            Star5 = "star_icon.png";
            OnPropertyChanged(nameof(Star5));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}