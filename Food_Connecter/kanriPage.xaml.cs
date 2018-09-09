using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Plugin.Media;
using Xamarin.Forms;

namespace Food_Connecter
{
    public partial class kanriPage : ContentPage
    {
        bool authenticated = false;

        public kanriPage()
        {
            InitializeComponent();
        }

        private async Task<String> Post(Dictionary<string, string> post_data, string url)
        {

            var httpClient = new HttpClient();
            var content = new FormUrlEncodedContent(post_data);

            var response = await httpClient.PostAsync(url, content);
            var result = response.Content.ReadAsStringAsync();
            return await result;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Refresh items only when authenticated.
            if (authenticated == true)
            {
                // Set syncItems to true in order to synchronize the data
                // on startup when running in offline mode.
                await RefreshItems(true, syncItems: false);
            }
        }

        async void takePhoto (object sender, EventArgs e)
        {
            var photoUrl = await PhotoClient.TakePhotoAsync();

            var result = await CognitiveAPIClient.AnalizeAsync(photoUrl);
        }

        async void signin (object sender, EventArgs e)
        {
            if (App.Authenticator != null)
                authenticated = await App.Authenticator.Authenticate();

            // Set syncItems to true to synchronize the data on startup when offline is enabled.
            if (authenticated == true)
                await RefreshItems(true, syncItems: false);
            }

        async void OnListItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            //((App)App.Current).ResumeAtTodoId = (e.SelectedItem as TodoItem).ID;
            //Debug.WriteLine("setting ResumeAtTodoId = " + (e.SelectedItem as TodoItem).ID);
            if (e.SelectedItem != null)
            {
                await Navigation.PushAsync(new FoodItemPage
                {
                    BindingContext = e.SelectedItem as FoodItem
                });
            }
        }
    }
}
