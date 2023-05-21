using Android.App;
using ShowStopper.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowStopper.ViewModels
{
    internal class ProfilePageViewModel
    {
        private INavigation _navigation;

        public Command EditProfileBtn { get; }

        public ProfilePageViewModel(INavigation navigation)
        {
            _navigation = navigation;
            EditProfileBtn = new Command(EditProfileBtnTappedAsync);
        }

        private async void EditProfileBtnTappedAsync(object parameter)
        {
            await _navigation.PushAsync(new EditProfilePage());
        }

        private async void ImageButton_Clicked(object sender, EventArgs e)
        {
            await _navigation.PushAsync(new EditProfilePage());
        }
    }
}
