using System;
using Microsoft.WindowsAzure.MobileServices;
using Xamarin.Forms;

namespace Food_Connecter
{
    public partial class kanriPage : ContentPage
    {
        // Track whether the user has authenticated.
        bool authenticated = false;

        public kanriPage()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            listView.IsRefreshing = true;
            listView.ItemsSource = await App.Database.GetItemsAsync();
            listView.IsRefreshing = false;

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
            this.IsBusy = true;
            var photoUrl = await PhotoClient.TakePhotoAsync();
            if (photoUrl == null)
            {
                this.IsBusy = false;
                return;
            }
            var ps = await CognitiveAPIClient.AnalizeAsync(photoUrl);
            foreach (var v in ps.Images[0].Classifiers[0].Classes)
            {
                var transData = App.client.GetAsync("https://script.google.com/macros/s/AKfycbwIMB_gK-VENxT4-BqKAtgOI779dL1TnOyR-qR1wAzjWmXta0W5/exec?text=" + v.Class + "&source=en&target=ja").Result;
                v.Class = await transData.Content.ReadAsStringAsync();
                Console.WriteLine("{0}:{1}:{2}", v.ID, v.Class, v.Score);
                float num = float.Parse(v.Score) * 100;
                v.Score = num.ToString() + "%";
                var accepted = await DisplayAlert(v.Class, "スコア: " + v.Score, "追加する", "スキップ");
                if (accepted)
                {
                    await App.Database.SaveItemAsync(v);
                }
            }
            listView.ItemsSource = await App.Database.GetItemsAsync();
            this.IsBusy = false;
        }

        async void OnListItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                await Navigation.PushAsync(new FoodItemPage
                {
                    BindingContext = e.SelectedItem as ClassData
                });
            }
        }

        async void DeleteAction(object sender, EventArgs e)
        {
            var mi = (MenuItem)sender;
            await App.Database.DeleteItemAsync(mi.CommandParameter as ClassData);
            listView.ItemsSource = null;
            listView.ItemsSource = await App.Database.GetItemsAsync();
        }

        async void DeleteAllAction(object sender, EventArgs e)
        {
            listView.IsRefreshing = true;
            foreach(var a in listView.ItemsSource)
            {
                await App.Database.DeleteItemAsync(a as ClassData);
            }
            listView.ItemsSource = await App.Database.GetItemsAsync();
            listView.IsRefreshing = false;
        }

        async void OnSettingsClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SettingsPage());
        }
    }
}
