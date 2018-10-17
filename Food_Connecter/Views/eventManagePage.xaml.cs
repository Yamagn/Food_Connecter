using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace Food_Connecter
{
    public partial class eventManagePage : ContentPage
    {
        public eventManagePage()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            var info = (eventModel)BindingContext;
            Console.WriteLine(info.Num);
            try
            {
                var res = await App.client.GetAsync(Constants.ApplicationURL + "/api/manageevent?id=" + App.Authenticator.user.UserId + "&eventnum=" + info.Num.ToString());
                var text = res.Content.ReadAsStringAsync().Result;
                Console.WriteLine(res.StatusCode);
                Console.WriteLine(text);
                var json = JsonConvert.DeserializeObject<List<ManageEvent>>(text)[0];
                eventName.Text = json.eventName;
                eventCity.Text = json.eventCity;
                eventPlace.Text = json.eventPlace;
                Date.Text = json.Date;
                menberList.ItemsSource = json.Menbers;
                wantedList.ItemsSource = json.Wanteds;
                foodList.ItemsSource = json.Foods;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }
        }

        async void Wanted_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddWantedPage
            {
                BindingContext = (eventModel)this.BindingContext
            });
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
            try
            {
                var res = await App.client.PostAsync(Constants.ApplicationURL + "/api/gatherevent", content);
                if (res.IsSuccessStatusCode)
                {
                    await DisplayAlert("成功", "食材を提供しました", "閉じる");
                    return;
                }
                await DisplayAlert("失敗", "提供に失敗しました", "閉じる");
                return;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                await DisplayAlert("失敗", "通信に失敗しました", "閉じる");
                return;
            }
           
        }

        async void DeleteAction(object sender, EventArgs e)
        {
            try
            {
                var food = (wanted)(((MenuItem)sender).CommandParameter);
                string reqUrl = Constants.ApplicationURL + "/api/deletewanted?id=" + App.Authenticator.user.UserId + "&foodname=" + food.foodName + "&eventnum=" + ((eventModel)BindingContext).Num;
                Console.WriteLine(reqUrl);
                var res = await App.client.DeleteAsync(reqUrl);
                Console.WriteLine(res.StatusCode);
                if (res.IsSuccessStatusCode)
                {
                    await DisplayAlert("削除", food.foodName + "の募集を削除しました", "閉じる");
                    OnAppearing();
                    return;
                }
                await DisplayAlert("失敗", "削除が完了しませんでした", "閉じる");
                return;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                await DisplayAlert("失敗", "通信に失敗しました", "閉じる");
                return;
            }
        }
    }
}
