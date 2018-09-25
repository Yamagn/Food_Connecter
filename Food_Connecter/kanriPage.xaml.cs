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
    public partial class kanriPage : ContentPage
    {
        public int Count = 0;
        public short Counter = 0;
        public int SlidePosition = 0;
        string heightList;
        int heightRowsList = 90;

        // Track whether the user has authenticated.
        bool authenticated = false;

        public kanriPage()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            //listView.ItemsSource = await App.Database.GetItemsAsync();

            if(authenticated == true) 
            {
                this.loginButton.Text = "logout";
            }
        }

        async void loginButton_Clicked(object sender, EventArgs e)
        {
            if(authenticated == false)
            {
                if (App.Authenticator != null)
                {
                    authenticated = await App.Authenticator.Authenticate(MobileServiceAuthenticationProvider.Google);
                    if (authenticated == true)
                    {
                        this.loginButton.Text = "logout";
                    }
                }
                    
            }
            else 
            {
                if (App.Authenticator != null)
                {
                    authenticated = false;
                    await App.Authenticator.ReleaseAuth();
                    this.loginButton.Text = "logout";
                }
            }

        }

        async void takePhoto (object sender, EventArgs e)
        {
            var photoUrl = await PhotoClient.TakePhotoAsync();
            //activity_indicator.IsRunning = true;
            var result = await CognitiveAPIClient.AnalizeAsync(photoUrl);
            if (result == null)
                return;
            listView.ItemsSource = await App.Database.GetItemsAsync();
            //int i = result.Count;
            //if(i > 0)
            //{
            //    activity_indicator.IsRunning = false;
            //}
            //i = (result.Count * heightRowsList);
            //activity_indicator.HeightRequest = i;

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
