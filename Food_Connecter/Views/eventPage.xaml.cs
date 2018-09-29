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

        protected async override void OnAppearing()
        {
            base.OnAppearing();
        }

        async void takePhoto(object sender, EventArgs e)
        {
            this.IsBusy = true;
            var photoUrl = await PhotoClient.TakePhotoAsync();
            if (photoUrl == null)
            {
                this.IsBusy = true;
                return;
            }
            var ps = await CognitiveAPIClient.AnalizeAsync(photoUrl);
            imagePreView.Source = photoUrl;
            var transData = App.client.GetAsync("https://script.google.com/macros/s/AKfycbwIMB_gK-VENxT4-BqKAtgOI779dL1TnOyR-qR1wAzjWmXta0W5/exec?text=" + ps.Images[0].Classifiers[0].Classes[0].Class + "&source=en&target=ja").Result;
            Result.Text = await transData.Content.ReadAsStringAsync();
            float num = float.Parse(ps.Images[0].Classifiers[0].Classes[0].Score) * 100;
            var ScoreResult = num.ToString() + "%";
            ResultScore.Text = ScoreResult;
            if (ps == null)
                return;
            this.IsBusy = false;
        }

        void InitDataAction(object sender, EventArgs e)
        {
            imagePreView.Source = null;
            Result.Text = "右上のカメラマークを押してね！";
            ResultScore.Text = "";
        }
    }
}
