using System;
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

            var data = (ClassData)BindingContext;
        }

        async void EditClicked(object sender, EventArgs e)
        {
            ClassData cd = new ClassData();
            try
            {
                cd = this.BindingContext as ClassData;
                cd.Date = DateTime.Parse(date.Text);
                cd.Class = className.Text;
                cd.Quantity = quantiry.Text;
            }
            catch
            {
                await DisplayAlert("エラー", "日時を正しく入力してください", "閉じる");
                return;
            }

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
