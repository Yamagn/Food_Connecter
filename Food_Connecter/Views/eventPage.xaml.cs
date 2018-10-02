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

        protected override void OnAppearing()
        {
            base.OnAppearing();
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
