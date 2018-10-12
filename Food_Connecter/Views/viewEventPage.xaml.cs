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

            var view = await App.client.GetAsync(Constants.ApplicationURL + "eventview?pref=" + kanriPage.userInfo.Pref);
            var jsonText = await view.Content.ReadAsStringAsync();
            Console.WriteLine(jsonText);
            var json = JsonConvert.DeserializeObject<List<viewEventModel>>(jsonText);
            listView.ItemsSource = json;
        }

        async void OnListItemSelected(object sender, ItemTappedEventArgs e)
        {

        }
    }
}
