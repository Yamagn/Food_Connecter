using System;
using Microsoft.WindowsAzure.MobileServices;
using Xamarin.Forms;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Food_Connecter
{
    public partial class kanriPage : ContentPage
    {
        // Track whether the user has authenticated.
        public static bool authenticated;
        public static UserModel userInfo;


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
                loginButton.Text = "logout";
            }
        }

        async void refleshList(object sender, EventArgs e)
        {
            listView.ItemsSource = await App.FoodDatabase.GetItemsAsync();
            listView.IsRefreshing = false;
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
                        loginButton.Text = "logout";
                        var res = App.client.GetAsync(Constants.ApplicationURL + "/api/getuser?userid=" + App.Authenticator.user.UserId).Result;
                        var json = res.Content.ReadAsStringAsync().Result;
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
                    loginButton.Text = "login";
                }
            }

        }

        async void takePhoto (object sender, EventArgs e)
        {
            Stack.IsVisible = true;
            var photo = await PhotoClient.TakePhotoAsync();
            List<ClassData> classDatas = new List<ClassData>();
            string[] vs = {};
            if (photo == null)
            {
                Stack.IsVisible = false;
                return;
            }
            var ps = await CognitiveAPIClient.AnalizeAsync(photo.Path);
            foreach (var v in ps.Images[0].Classifiers[0].Classes)
            {
                var transData = App.client.GetAsync("https://script.google.com/macros/s/AKfycbwIMB_gK-VENxT4-BqKAtgOI779dL1TnOyR-qR1wAzjWmXta0W5/exec?text=" + v.Class + "&source=en&target=ja").Result;
                v.Class = await transData.Content.ReadAsStringAsync();
                Console.WriteLine("{0}:{1}:{2}", v.ID, v.Class, v.Score);
                float num = float.Parse(v.Score) * 100;
                v.Score = num.ToString() + "%";
                var limit = v.Date - DateTime.Now;
                v.Limit = String.Format("残り : {0}日", limit.Days.ToString());
                v.image = photo.AlbumPath;
                v.Quantity = "1個";
                classDatas.Add(v);
            }
            foreach(var i in classDatas)
            {
                vs.CopyTo(vs = new string[vs.Length + 1], 0);
                vs[vs.Length - 1] = i.Class;
            }
            var res = await DisplayActionSheet("あなたの食品はもしかして...", "閉じる", "破棄する", vs);
            foreach (var i in classDatas)
            {
                if(res == i.Class)
                {
                    Console.WriteLine("photoUrl : " + i.image);
                    await App.FoodDatabase.SaveItemAsync(i);
                    break;
                }
            }
            listView.ItemsSource = await App.FoodDatabase.GetItemsAsync();
            Stack.IsVisible = false;
        }

        async void OnListItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            await Navigation.PushAsync(new FoodItemPage
            {
                BindingContext = e.SelectedItem as ClassData
            });
        }

        async void DeleteAction(object sender, EventArgs e)
        {
            var mi = (MenuItem)sender;
            await App.FoodDatabase.DeleteItemAsync(mi.CommandParameter as ClassData);
            listView.ItemsSource = null;
            listView.ItemsSource = await App.FoodDatabase.GetItemsAsync();
        }

        async void OnSettingsClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SettingsPage());
        }
    }
}
