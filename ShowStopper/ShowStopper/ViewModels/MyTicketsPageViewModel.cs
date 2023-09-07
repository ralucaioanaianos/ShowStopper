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
    internal class MyTicketsPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool IsListEmpty { get; set; }
        public bool IsDataLoaded { get; set; } = false;
        public Command TicketTapped { get; }
        private ObservableCollection<Ticket> _tickets;
        public ObservableCollection<Ticket> Tickets
        {
            get { return _tickets; }
            set 
            { 
                _tickets = value;
                OnPropertyChanged(nameof(Tickets));
            }
        }
        private INavigation _navigation;
        public Command BackBtn { get; }
        public Command PlusBtn { get; }

        public MyTicketsPageViewModel(INavigation navigation)
        {
            LoadTickets();
            _navigation = navigation;
            BackBtn = new Command(BackButtonTappedAsync);
            PlusBtn = new Command(PlusButtonTappedAsync);
            TicketTapped = new Command(TicketTappedAsync);
        }

        private AppEvent _selectedTicket;
        public AppEvent SelectedTicket
        {
            get { return _selectedTicket; }
            set
            {
                _selectedTicket = value;
                OnPropertyChanged(nameof(SelectedTicket));
                //OnTicketSelected();
            }
        }

        private async void OnTicketSelected()
        {
            if (SelectedTicket != null)
            {
                await _navigation.PushAsync(new EventPage(SelectedTicket));
                SelectedTicket = null;
            }
        }

        private async void LoadTickets()
        {
            string email = FirebaseAuthenticationService.GetLoggedUserEmail();
            List<Ticket> tickets = await UserService.GetTicketsForUser();
            ObservableCollection<Ticket> collection = new ObservableCollection<Ticket>(tickets);
            Tickets = collection;
            IsDataLoaded = true;
        }

        private async void TicketTappedAsync(object parameter)
        {
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
