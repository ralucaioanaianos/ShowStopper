using Firebase.Auth;
using ShowStopper.CustomComponents;
using ShowStopper.Models;
using ShowStopper.Services;
using ShowStopper.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ShowStopper.ViewModels
{
    internal class MyEventsPageViewModel : INotifyPropertyChanged
    {
        public Command BackBtn { get; }
        public Command PlusBtn { get; }


        private ObservableCollection<AppEvent> _events;
        public ObservableCollection<AppEvent> Events
        {
            get { return _events; }
            set
            {
                _events = value;
                OnPropertyChanged(nameof(Events));
            }
        }

        private INavigation _navigation;

        private async void BackButtonTappedAsync(object parameter)
        {
            await _navigation.PopAsync();
        }

        private async void PlusButtonTappedAsync(object parameter)
        {
            await _navigation.PushAsync(new AddEventPage());
        }

        public MyEventsPageViewModel(INavigation navigation)
        {
            _navigation = navigation;
            BackBtn = new Command(BackButtonTappedAsync);
            PlusBtn = new Command(PlusButtonTappedAsync);
            LoadEvents();
            // Step 3: Create a list view
            //ListView listView = new ListView();

            //// Step 4: Iterate over the event data
            //foreach (AppEvent eventItem in Events)
            //{
            ////    // Step 2: Create a custom component for event data
            //    EventElement eventComponent = new EventElement();

            ////    // Step 5: Add the custom component to the list view
            //    listView.Children.Add(eventComponent);
            //    listView.ch
            //}

            // Add the list view to your main layout or page
            //Content = new StackLayout
            //{
            //    Children = { listView }
            //};
        }

        private async void LoadEvents()
        {
            List<AppEvent> list = await FirebaseDatabaseService.getAllEvents();
            ObservableCollection<AppEvent> collection = new ObservableCollection<AppEvent>(list);
            await FirebaseAuthenticationService.GetLoggedUser();
            Events = collection;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
