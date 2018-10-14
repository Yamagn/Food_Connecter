using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

using Xamarin.Forms;

namespace Food_Connecter
{
    public partial class eventDetailpage : ContentPage
    {
        public eventDetailpage()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            var info = (eventModel)BindingContext;
            Console.WriteLine(info.Num);

            var res = await App.client.GetAsync(Constants.ApplicationURL + "/api/detailevent?eventnum=" + info.Num.ToString());
            Console.WriteLine(res.StatusCode);
            var text = res.Content.ReadAsStringAsync().Result;
            Console.WriteLine(text);
            try
            {
                var json = JsonConvert.DeserializeObject<List<eventDetail>>(text)[0];
                eventName.Text = json.eventName;
                eventCity.Text = json.eventCity;
                eventPlace.Text = json.eventPlace;
                Date.Text = json.Date;
                wantedList.ItemsSource = json.Wanteds;
                foodList.ItemsSource = json.Foods;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }
        }

        async void join_Clicked(object sender, EventArgs e)
        {
            eventJoinModel eventJoin = new eventJoinModel();
            eventJoin.userId = App.Authenticator.user.UserId;
            eventJoin.eventNum = ((eventModel)BindingContext).Num;
            var json = JsonConvert.SerializeObject(eventJoin);
            Console.WriteLine(json);
            var content = new StringContent(json);
            content.Headers.ContentType.MediaType = "application/json";
            var response = App.client.PostAsync(Constants.ApplicationURL + "/api/joinevent", content).Result;
            if(response.IsSuccessStatusCode)
            {
                await DisplayAlert("成功", "このイベントに参加しました", "閉じる");
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("失敗", "参加に失敗しました", "閉じる");
                return;
            }
        }
        async void OnListItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var foodname = ((wanted)e.SelectedItem).foodName;
            bool result = await DisplayAlert("この食材を提供しますか", foodname, "はい", "いいえ");
            if (!result)
            {
                return;
            }
            var info = (eventModel)BindingContext;
            WantedFoodModel wanted = new WantedFoodModel();
            wanted.userId = App.Authenticator.user.UserId;
            wanted.eventNum = info.Num;
            wanted.wanteds = foodname;
            var json = JsonConvert.SerializeObject(wanted);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            var res = await App.client.PostAsync(Constants.ApplicationURL + "/api/gatherevent", content);
            if (res.IsSuccessStatusCode)
            {
                await DisplayAlert("成功", "食材を提供しました", "閉じる");
                return;
            }
            else
            {
                await DisplayAlert("失敗", "提供に失敗しました", "閉じる");
                return;
            }
        }
    }
}
