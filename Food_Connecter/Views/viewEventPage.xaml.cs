using System;
using System.Collections.Generic;
using Newtonsoft.Json;

using Xamarin.Forms;

namespace Food_Connecter
{
    public partial class viewEventPage : ContentPage
    {
        public viewEventPage()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                var view = await App.client.GetAsync(Constants.ApplicationURL + "/api/join_event_list?id=" + App.Authenticator.user.UserId);
                var jsonText = await view.Content.ReadAsStringAsync();
                Console.WriteLine(jsonText);
                var json = JsonConvert.DeserializeObject<List<eventModel>>(jsonText);
                listView.ItemsSource = json;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }

        }

        async void OnListItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
            {
                return;
            }

            var info = e.SelectedItem as eventModel;
            if (info.Userid == App.Authenticator.user.UserId)
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
    }
}
