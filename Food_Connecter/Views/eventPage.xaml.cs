using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Android.App;
using Microsoft.WindowsAzure.MobileServices;
using Plugin.Media;
using Xamarin.Forms;

namespace Food_Connecter
{
    public partial class eventPage : ContentPage
    {
        public eventPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (App.Authenticator.user == null)
            {
                var res = await DisplayAlert("ログインしてください", "", "ログインする", "閉じる");
                if (res)
                {
                    kanriPage.authenticated = await App.Authenticator.Authenticate(MobileServiceAuthenticationProvider.Google);
                }
                else
                {
                    return;
                }
            }
            else
            {

            }
        }

        async void manageButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddEventPage());
        }

        void OnListItemSelected(object sender, EventArgs e)
        {

        }
    }
}
