using System;
using System.Collections.Generic;
using System.Net.Http;
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
                foreach (var i in json.Wanteds)
                {
                    TextCell cell = new TextCell();
                    cell.Text = i.foodName;
                    cell.Tapped += async (sender, e) => 
                    {
                        var result = await DisplayAlert(cell.Text, "この食材を提供しますか？", "提供する", "戻る");
                        if(result)
                        {
                            provideFood provide = new provideFood();
                            provide.userId = App.Authenticator.user.UserId;
                            provide.Num = info.Num;
                            provide.Food = cell.Text;

                            var jsonText = JsonConvert.SerializeObject(provide);
                            Console.WriteLine(json);
                            var content = new StringContent(jsonText);
                            content.Headers.ContentType.MediaType = "application/json";
                            var response = await App.client.PostAsync(Constants.ApplicationURL + "/api/gatherevent", content);
                            if(response.IsSuccessStatusCode)
                            {
                                await DisplayAlert("成功", "食材を提供します", "閉じる");
                                await Navigation.PopAsync();
                            }
                            else
                            {
                                await DisplayAlert("失敗", "食材の提供に失敗しました", "閉じる");
                                return;
                            }
                        }
                    };
                    wanted.Add(cell);
                }
                foreach (var i in json.Foods)
                {
                    EntryCell cell = new EntryCell();
                    cell.Label = i.foodName;
                    cell.Text = i.userName;
                    cell.IsEnabled = false;
                    food.Add(cell);
                }
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
            var res = App.client.PostAsync(Constants.ApplicationURL + "/api/joinevent", content).Result;
            if(res.IsSuccessStatusCode)
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
    }
}
