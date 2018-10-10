﻿using System;
using System.Net.Http;
using System.Collections.Generic;
using System.IO;

using Xamarin.Forms;

namespace Food_Connecter
{
    public partial class FoodItemPage : ContentPage
    {
        public FoodItemPage()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
        }

        async void EditClicked(object sender, EventArgs e)
        {
            ClassData cd = new ClassData();
            try
            {
                cd = this.BindingContext as ClassData;
            }
            catch
            {
                await DisplayAlert("エラー", "日時を正しく入力してください", "閉じる");
                return;
            }
            
            cd.Date = DateTime.Parse(date.Text);
            cd.Class = className.Text;
            cd.Quantity = quantiry.Text;

            await App.FoodDatabase.SaveItemAsync(cd);
            await DisplayAlert("成功", "内容を編集しました", "戻る");
            return;
        }

            async void Go_Osusowake(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PostDataPage
            {
                BindingContext = this.BindingContext
            });
        }
    }
}
