using System;
using Microsoft.WindowsAzure.MobileServices;
using Xamarin.Forms;
using Newtonsoft.Json;

namespace Food_Connecter
{
    public partial class kanriPage : ContentPage
    {
        // Track whether the user has authenticated.
        public static bool authenticated = false;
        public static UserModel userInfo = null;

        public kanriPage()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            Stack.IsVisible = true;
            foreach(var i in App.FoodDatabase.GetItemsAsync().Result)
            {
                var limit = i.Date - DateTime.Now;
                i.Limit = String.Format("残り : {0}日", limit.Days.ToString());
                Console.WriteLine(i.Limit);
                await App.FoodDatabase.SaveItemAsync(i);
            }
            listView.ItemsSource = await App.FoodDatabase.GetItemsAsync();
            Stack.IsVisible = false;

            if(authenticated == true) 
            {
                this.loginButton.Text = "logout";
            }
        }

        async void loginButton_Clicked(object sender, EventArgs e)
        {
            Stack.IsVisible = true;
            if(authenticated == false)
            {
                if (App.Authenticator != null)
                {
                    authenticated = await App.Authenticator.Authenticate(MobileServiceAuthenticationProvider.Google);
                    if (authenticated == true)
                    {
                        this.loginButton.Text = "logout";
                        var res = App.client.GetAsync(Constants.ApplicationURL + "/api/getuser?userid=" + App.Authenticator.user.UserId);
                        var json = await res.Result.Content.ReadAsStringAsync();
                        Console.WriteLine(json);
                        userInfo = JsonConvert.DeserializeObject<UserModel>(json);
                        await DisplayAlert("ログイン成功", App.Authenticator.user.UserId, "閉じる");
                        Stack.IsVisible = false;
                        if (userInfo.City == null)
                        {
                            await Navigation.PushAsync(new SettingsPage());
                        }
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
            Stack.IsVisible = true;
            var photoUrl = await PhotoClient.TakePhotoAsync();
            if (photoUrl == null)
            {
                Stack.IsVisible = false;
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
                var limit = v.Date - DateTime.Now;
                v.Limit = String.Format("残り : {0}日", limit.Days.ToString());
                var accepted = await DisplayAlert(v.Class, "スコア: " + v.Score, "追加する", "スキップ");
                if (accepted)
                {
                    await App.FoodDatabase.SaveItemAsync(v);
                }
            }
            listView.ItemsSource = await App.FoodDatabase.GetItemsAsync();
            Stack.IsVisible = false;
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
            await App.FoodDatabase.DeleteItemAsync(mi.CommandParameter as ClassData);
            listView.ItemsSource = null;
            listView.ItemsSource = await App.FoodDatabase.GetItemsAsync();
        }

        async void DeleteAllAction(object sender, EventArgs e)
        {
            listView.IsRefreshing = true;
            foreach(var a in listView.ItemsSource)
            {
                await App.FoodDatabase.DeleteItemAsync(a as ClassData);
            }
            listView.ItemsSource = await App.FoodDatabase.GetItemsAsync();
            listView.IsRefreshing = false;
        }

        async void OnSettingsClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SettingsPage());
        }
    }
}
