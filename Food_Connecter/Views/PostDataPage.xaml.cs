using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
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

            try
            {
                await DisplayAlert("食材撮影", "今の状態を撮影してください", "OK");
                var photoUrl = await PhotoClient.TakePhotoAsync();
                if (photoUrl == null)
                {
                    return;
                }
                var num = await postImage(photoUrl.Path);

                await FoodPost(num);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                await DisplayAlert("失敗", "通信に失敗しました", "閉じる");
                return;
            }
            return;
        }

        async Task<int> postImage(string photoUrl)
        {
            try
            {
                var serverUri = Constants.ApplicationURL + "/api/photopost";
                var content = new StreamContent(File.OpenRead(photoUrl));
                var res = App.client.PostAsync(serverUri, content).Result;
                var jsonText = await res.Content.ReadAsStringAsync();
                Console.WriteLine(jsonText);
                var ps = JsonConvert.DeserializeObject<foodNum>(jsonText);
                return int.Parse(ps.FoodNum);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                await DisplayAlert("失敗", "通信に失敗しました", "閉じる");
                return 0;
            }
        }

        async Task FoodPost(int foodNum)
        {
            try
            {
                var s1 = DateTime.Now.ToString();
                var vs2 = s1.Split('/');
                string s2 = String.Format("{0}-{1}-{2}", vs2[0], vs2[1], vs2[2]);
                Console.WriteLine(s2);
                postFood post = new postFood();
                post.userId = App.Authenticator.user.UserId;
                post.foodName = ClassName.Text;
                post.foodDate = s2;
                post.Info = Info.Text;
                post.foodNum = foodNum;
                var json = JsonConvert.SerializeObject(post);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                Console.WriteLine(await content.ReadAsStringAsync());
                var res = await App.client.PostAsync(Constants.ApplicationURL + "/api/foodpost", content);
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
                Console.WriteLine("エラー : " + err.Message);
            }
        }
    }
}
