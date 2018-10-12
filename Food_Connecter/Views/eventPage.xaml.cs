using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Android.App;
using Microsoft.WindowsAzure.MobileServices;
using Plugin.Media;
using Xamarin.Forms;
using Newtonsoft.Json;

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
                listView.IsPullToRefreshEnabled = false;
                await DisplayAlert("ログインしてください", "", "閉じる");
                return;
            }
            listView.IsRefreshing = true;
            var res = await App.client.GetAsync("https://samplefood2.azurewebsites.net/api/eventview?pref=" + kanriPage.userInfo.Pref);
            var json = await res.Content.ReadAsStringAsync();
            Console.WriteLine(json);
            var eventList = new List<eventModel>();
            try
            {
                eventList = JsonConvert.DeserializeObject<List<eventModel>>(json);
            }
            catch
            {
                return;
            }
            
            listView.ItemsSource = eventList;
            listView.IsRefreshing = false;
        }

        async void refreshList(object sender, EventArgs e)
        {
            var res = await App.client.GetAsync("https://samplefood2.azurewebsites.net/api/eventview?pref=" + kanriPage.userInfo.Pref);
            var json = await res.Content.ReadAsStringAsync();
            var eventList = new List<eventModel>();
            try
            {
                eventList = JsonConvert.DeserializeObject<List<eventModel>>(json);
            }
            catch
            {
                return;
            }
            listView.ItemsSource = eventList;

            listView.EndRefresh();
        }

        async void manageButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddEventPage());
        }

        async void OnListItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if(e.SelectedItem == null)
            {
                return;
            }

            var info = (eventModel)e.SelectedItem;
            if(info.Userid == App.Authenticator.user.UserId)
            {
                await Navigation.PushAsync(new eventManagePage
                {
                    BindingContext = info
                });
            }
            else
            {
                await Navigation.PushAsync(new eventDetailpage
                {
                    BindingContext = info
                });
            }
        }

        async void viewEvent_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new viewEventPage());
        }
    }
}
