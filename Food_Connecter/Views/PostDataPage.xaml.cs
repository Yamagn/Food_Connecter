using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Xamarin.Forms;

namespace Food_Connecter
{
    public partial class PostDataPage : ContentPage
    {
        public PostDataPage()
        {
            InitializeComponent();
        }

        async void OnSendClicked(object sender, EventArgs e)
        {
            if (App.Authenticator.user == null)
            {
                await DisplayAlert("ログインしてください", "", "戻る");
                await Navigation.PopAsync();
                return;
            }

            if (Info.Text == null)
            {
                await DisplayAlert("情報を入れてください", "", "閉じる");
                return;
            }

            await DisplayAlert("食材撮影", "今の状態を撮影してください", "OK");
            var photoUrl = await PhotoClient.TakePhotoAsync();
            if (photoUrl == null)
            {
                return;
            }
            try
            {
                var s1 = DateTime.Now.ToString();
                var vs2 = s1.Split('/');
                string s2 = String.Format("{0}-{1}-{2}", vs2[0], vs2[1], vs2[2]);
                Console.WriteLine(s2);
                var content = new MultipartFormDataContent();
                var userid = new StringContent(App.Authenticator.user.UserId, Encoding.UTF8);
                content.Add(userid, "userid");
                var food = new StringContent(ClassName.Text, Encoding.UTF8);
                content.Add(food, "food");
                var fooddate = new StringContent(s2, Encoding.UTF8);
                content.Add(fooddate, "fooddate");
                var info = new StringContent(Info.Text, Encoding.UTF8);
                content.Add(info, "info");
                var image = new StreamContent(File.OpenRead(photoUrl));
                content.Add(image, "image");
                HttpClient client = new HttpClient();
                Console.WriteLine(await content.ReadAsStringAsync());
                client.DefaultRequestHeaders.ExpectContinue = false;
                var res = client.PostAsync(Constants.ApplicationURL + "/api/foodlearn", content).Result;
                if (res.IsSuccessStatusCode)
                {
                    await DisplayAlert("おすそ分けに投稿しました", "受け取り相手が現れるまで待ちましょう", "戻る");
                    ((ClassData)this.BindingContext).IsOsusowake = true;
                    await App.FoodDatabase.SaveItemAsync((ClassData)this.BindingContext);
                    await Navigation.PopAsync();
                    return;
                }
            }

            catch (Exception err)
            {
                Console.WriteLine("エラー : " +  err.Message);
            }
        }
    }
}
