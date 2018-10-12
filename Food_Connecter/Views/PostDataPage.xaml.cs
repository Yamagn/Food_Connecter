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
                var userid = new StringContent(App.Authenticator.user.UserId);
                content.Add(userid, "userid");
                var food = new StringContent(ClassName.Text);
                content.Add(food, "food");
                var fooddate = new StringContent(s2);
                content.Add(fooddate, "fooddate");
                var info = new StringContent(Info.Text);
                content.Add(info, "info");
                var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Path.GetFileName(photoUrl.Path));
                var imageContent = new StreamContent(File.OpenRead(filePath));
                imageContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                content.Add(imageContent, "image");
                Console.WriteLine(await content.ReadAsStringAsync());
                var res = App.client.PostAsync(Constants.ApplicationURL + "/api/foodpost", content).Result;
                Console.WriteLine(await res.Content.ReadAsStringAsync());
                if (res.IsSuccessStatusCode)
                {
                    await DisplayAlert("おすそ分けに投稿しました", "受け取り相手が現れるまで待ちましょう", "戻る");
                    ((ClassData)this.BindingContext).IsOsusowake = true;
                    await App.FoodDatabase.SaveItemAsync((ClassData)BindingContext);
                    await Navigation.PopAsync();
                    return;
                }
                else
                {
                    await DisplayAlert("失敗", "おすそわけに失敗しました", "閉じる");
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
