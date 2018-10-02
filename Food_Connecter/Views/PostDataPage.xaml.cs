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
                var content = new MultipartFormDataContent();
                var userid = new StringContent(App.Authenticator.user.UserId, Encoding.UTF8);
                Console.WriteLine(userid.ReadAsStringAsync().Result);
                content.Add(userid, "userid");
                var food = new StringContent(ClassName.Text, Encoding.UTF8);
                content.Add(food, "food");
                Console.WriteLine(food.ReadAsStringAsync().Result);
                var fooddate = new StringContent(DateTime.Now.ToString(), Encoding.UTF8);
                Console.WriteLine(fooddate.ReadAsStringAsync().Result);
                content.Add(fooddate, "fooddate");
                var info = new StringContent(Info.Text, Encoding.UTF8);
                Console.WriteLine(info.ReadAsStringAsync().Result);
                content.Add(info, "info");
                var image = new StreamContent(File.OpenRead(photoUrl));
                Console.WriteLine(image.ReadAsStringAsync().Result);
                content.Add(image, "image");
                Console.WriteLine("Content : " + content.ReadAsStringAsync().Result);
                HttpClient client = new HttpClient();
                await content.ReadAsStringAsync();
                client.DefaultRequestHeaders.ExpectContinue = false;
                var res = client.PostAsync(Constants.ApplicationURL + "/api/foodlearn", content).Result;
                Console.WriteLine("ResponseStatusCode : " +  res.StatusCode);
                Console.WriteLine("Response Result : " + res.Content.ReadAsStringAsync().Result);
                if (res.IsSuccessStatusCode)
                {
                    await DisplayAlert("おすそ分けに投稿しました", "受け取り相手が現れるまで待ちましょう", "戻る");
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
