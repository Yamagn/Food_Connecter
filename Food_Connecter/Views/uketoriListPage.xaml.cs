using System;
using System.Collections.Generic;
using Newtonsoft.Json;

using Xamarin.Forms;

namespace Food_Connecter
{
    public partial class uketoriListPage : ContentPage
    {
        public class users
        {
            [JsonProperty("username")]
            public string userName { get; set; }
        }
        public uketoriListPage()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            Stack.IsVisible = true;
            var res = await App.client.GetAsync(Constants.ApplicationURL + "/api/getchatlist?username=" + kanriPage.userInfo.UserName);
            var json = JsonConvert.DeserializeObject<List<users>>(await res.Content.ReadAsStringAsync());
            Console.WriteLine(await res.Content.ReadAsStringAsync());
            listView.ItemsSource = json;
            Stack.IsVisible = false;
        }

        async void OnListItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            await Navigation.PushAsync(new ChatPage
            {
                BindingContext = ((users)e.SelectedItem).userName
            });
        }
    }
}
