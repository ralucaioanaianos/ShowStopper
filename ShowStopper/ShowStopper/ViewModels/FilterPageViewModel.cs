using ShowStopper.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Xml.Linq;

namespace ShowStopper.ViewModels
{
    internal class FilterPageViewModel
    {
        private INavigation _navigation;
        public Command ExitBtn { get; }
        public DateTime TodayDate { get; set; } 
        public DateTime FromTime { get; set; }
        public DateTime ToTime { get; set; }
        public Command SaveBtn { get; }

        public FilterPageViewModel(INavigation navigation)
        {
            _navigation = navigation;
            TodayDate = DateTime.Now;
            ExitBtn = new Command(ExitBtnTappedAsync);
            SaveBtn = new Command(SaveBtnTappedAsync);

        }

        private async void SaveBtnTappedAsync(object parameter)
        {
            
        }

        private async void ExitBtnTappedAsync(object parameter)
        {
            await _navigation.PopAsync();
        }
    }
}
