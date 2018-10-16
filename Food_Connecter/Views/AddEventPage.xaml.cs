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
            if (EventNameEntry.Text == null || PrefPicker.SelectedItem == null || TownPicker.SelectedItem == null || PlaceEntry.Text == null || DatePicker.Date == null || TimePicker.Time == null)
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
            var res = await App.client.PostAsync(Constants.ApplicationURL + "/api/makeevent", content);

            await DisplayAlert("登録完了", "", "閉じる");
            await Navigation.PopAsync();

            Console.WriteLine(res.StatusCode);
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            if (App.Authenticator.user == null)
            {
                await DisplayAlert("ログインしてください", "", "閉じる");
                await Navigation.PopAsync();
            }

            var assembly = typeof(SettingsPage).GetTypeInfo().Assembly;
            using (Stream stream = assembly.GetManifestResourceStream("Food_Connecter.japan_city.json"))
            using (StreamReader reader = new StreamReader(stream))
            {
                string json = await reader.ReadToEndAsync();
                townDatas = JsonConvert.DeserializeObject<List<TownData>>(json);
            }
        }
    }
}