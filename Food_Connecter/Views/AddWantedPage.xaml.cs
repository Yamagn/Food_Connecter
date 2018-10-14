using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Net.Http;

using Xamarin.Forms;
using System.Text;

namespace Food_Connecter
{
    public partial class AddWantedPage : ContentPage
    {
        public AddWantedPage()
        {
            InitializeComponent();
        }

        async void Submit_Clicked(object sender, EventArgs e)
        {
            if (WantedFood == null)
            {
                await DisplayAlert("入力エラー", "募集する食材の名前を入力してください", "閉じる");
                return;
            }

            var wanted = new WantedFoodModel();
            wanted.userId = App.Authenticator.user.UserId;
            wanted.eventNum = ((eventModel)BindingContext).Num;
            wanted.wanteds = WantedFood.Text;
            var json = JsonConvert.SerializeObject(wanted);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            Console.WriteLine(content.ReadAsStringAsync());
            var res = await App.client.PostAsync(Constants.ApplicationURL + "/api/eventwanted", content);
            if (res.IsSuccessStatusCode)
            {
                var result = await DisplayAlert("成功", "食材を要求しました", "追加を続ける", "閉じる");
                if (result)
                {
                    WantedFood.Text = null;
                    return;
                }
                else
                {
                    await Navigation.PopAsync();
                }
            }
            else
            {
                await DisplayAlert("失敗", "投稿に失敗しました", "閉じる");
                return;
            }
        }
    }
}
