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

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        async void takePhoto (object sender, EventArgs e)
        {
            var photoUrl = await PhotoClient.TakePhotoAsync();
            var result = await CognitiveAPIClient.AnalizeAsync(photoUrl);
            //await CognitiveAPIClient.AnalizeAsync(photoUrl);
            if (result == null)
                return;
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
