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

            // Reset the 'resume' id, since we just want to re-start here
            ((App)App.Current).ResumeAtFoodId = -1;
            listView.ItemsSource = await App.Database.GetItemsAsync();

        }

        async void takePhoto (object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                DisplayAlert("No Camera", ":( No camera available.", "OK");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                    Directory = "Sample",
                    Name = "test.jpg"
            });
                                                               
            if (file == null)
            return;

            await DisplayAlert("File Location", file.Path, "OK");
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
