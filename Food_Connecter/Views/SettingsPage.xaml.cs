using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using System.Net.Http;

using Xamarin.Forms;
using System.Text;

namespace Food_Connecter
{
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        List<TownData> townDatas;

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            var assembly = typeof(SettingsPage).GetTypeInfo().Assembly;
            using (Stream stream = assembly.GetManifestResourceStream("Food_Connecter.japan_city.json"))
            using (StreamReader reader = new StreamReader(stream))
            {
                string json = await reader.ReadToEndAsync();
                townDatas = JsonConvert.DeserializeObject<List<TownData>>(json);
            }

            if (App.Authenticator.user == null)
            {
                await DisplayAlert("ログインしてください", "", "戻る");
                await Navigation.PopAsync();
                return;
            }

            foreach (var i in Constants.prefNames)
            {
                PrefPicker.Items.Add(i);
            }
        }

        async void SelectedChanged(object sender, EventArgs e)
        {
            TownPicker.Items.Clear();
            TownPicker.IsEnabled = true;
            foreach (var j in townDatas)
            {
                if (Equals(PrefPicker.SelectedItem, j.prefName))
                {
                    TownPicker.Items.Add(j.townName);
                }
            }
        }

        async void OnSubmitClicked(object sender, EventArgs e)
        {
            Console.WriteLine("{0} : {1}", PrefPicker.SelectedItem, UserNameEntry.Text);
            if(PrefPicker.SelectedItem == null || UserNameEntry.Text == null)
            {
                await DisplayAlert("すべての項目を入力してください", "", "戻る");
                return;
            }
            else
            {
                var userData = new UserModel();
                userData.UserID = App.Authenticator.user.UserId;
                userData.UserName = UserNameEntry.Text;
                userData.Pref = PrefPicker.SelectedItem.ToString();
                userData.City = TownPicker.SelectedItem.ToString();

                var json = JsonConvert.SerializeObject(userData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var serverUri = Constants.ApplicationURL + "/api/users";
                try
                {
                    var res = App.client.PostAsync(serverUri, content);
                }
                catch(Exception err)
                {
                    await DisplayAlert("エラー", err.Message, "閉じる");
                    return;
                }
                await DisplayAlert("登録が完了しました", "", "戻る");
                await Navigation.PopAsync();

            }
        }

        async void DeleteAllAction(object sender, EventArgs e)
        {
            foreach (var a in App.FoodDatabase.GetItemsAsync().Result)
            {
                await App.FoodDatabase.DeleteItemAsync(a);
            }
        }
    }
}
