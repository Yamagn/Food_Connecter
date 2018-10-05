﻿using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Food_Connecter
{
    public partial class OsusowakeDetailPage : ContentPage
    {
        public OsusowakeDetailPage()
        {
            InitializeComponent();
        }
        async void OnUketoriClicked(object sender, EventArgs e)
        {
            var result = await DisplayAlert("確認", "この食材を受け取りますか？", "受け取る", "戻る");
            if(result)
            {
                var res = await App.client.GetAsync(Constants.ApplicationURL + "/api/feed?id=" + ((osusowakeFood)this.BindingContext).userName + "&num=" + ((osusowakeFood)this.BindingContext).foodNum.ToString());
                Console.WriteLine("userName : " + ((osusowakeFood)this.BindingContext).userName);
                Console.WriteLine("foodNum : " + ((osusowakeFood)this.BindingContext).foodNum);
                Console.WriteLine(res.StatusCode);
                if(res.IsSuccessStatusCode)
                {
                    await DisplayAlert("受け取りを申請しました", "この後提供者とはチャットでの会話をお願いします。", "閉じる");
                    await App.UketoriDatabase.SaveItemAsync((osusowakeFood)this.BindingContext);
                    await Navigation.PopAsync();
                }
                else
                {
                    await DisplayAlert("失敗しました", "やり直してください", "閉じる");
                }
            }
        }
    }
}
