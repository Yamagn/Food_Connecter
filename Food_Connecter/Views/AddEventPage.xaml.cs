using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Reflection;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace Food_Connecter
{

    public partial class AddEventPage : ContentPage
    {
        public AddEventPage()
        {
            InitializeComponent();

            PrefPicker.ItemsSource = Constants.prefNames;
        }

        List<TownData> townDatas;

        void SelectedChanged(object sender, EventArgs e)
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
            if (EventNameEntry.Text == null || PrefPicker.SelectedItem == null || TownPicker.SelectedItem == null || PlaceEntry.Text == null)
            {
                await DisplayAlert("すべての項目を入力してください", "", "閉じる");
                return;
            }

            postEvent pe = new postEvent();
            pe.Id = App.Authenticator.user.UserId;
            pe.eventName = EventNameEntry.Text;
            pe.Pref = PrefPicker.SelectedItem.ToString();
            pe.City = TownPicker.SelectedItem.ToString();
            pe.Place = PlaceEntry.Text;
            pe.eventDate = String.Format("{0} {1}", DatePicker.Date.ToString("yyyy-MM-dd"), TimePicker.Time.ToString("c"));
            var json = JsonConvert.SerializeObject(pe);
            Console.WriteLine(json);
            var content = new StringContent(json);
            content.Headers.ContentType.MediaType = "application/json";
            try
            {
                var res = await App.client.PostAsync(Constants.ApplicationURL + "/api/makeevent", content);
                Console.WriteLine(res.StatusCode);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                await DisplayAlert("失敗", "通信に失敗しました", "閉じる");
                return;
            }


            await DisplayAlert("登録完了", "", "閉じる");
            await Navigation.PopAsync();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            if (App.Authenticator.user == null)
            {
                await DisplayAlert("ログインしてください", "", "閉じる");
                await Navigation.PopAsync();
            }

            try
            {
                var assembly = typeof(SettingsPage).GetTypeInfo().Assembly;
                using (Stream stream = assembly.GetManifestResourceStream("Food_Connecter.japan_city.json"))
                using (StreamReader reader = new StreamReader(stream))
                {
                    string json = await reader.ReadToEndAsync();
                    townDatas = JsonConvert.DeserializeObject<List<TownData>>(json);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                await DisplayAlert("エラー", "ファイルが破損している可能性があります", "閉じる");
                return;
            }

        }
    }
}