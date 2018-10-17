using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Newtonsoft.Json;
using System.Net;

namespace Food_Connecter
{
    public partial class osusowakePage : ContentPage
    {
        class ImageList
        {
            [JsonProperty("photos")]
            public List<string> Photos;
        }

        public osusowakePage()
        {
            InitializeComponent();
        }
        

        async void LoadBitmapCollection()
        {
            int imageDimension = Device.RuntimePlatform == Device.iOS || Device.RuntimePlatform == Device.Android ? 80 : 40;
            string urlSuffix = String.Format("?width={0}&height{0}&mode=max", imageDimension);

            using (WebClient webClient = new WebClient())
            {
                try
                {
                    Console.WriteLine(kanriPage.userInfo.City);
                    Uri uri = new Uri(Constants.ApplicationURL + "/api/view?id=" + App.Authenticator.user.UserId + "&city=" + kanriPage.userInfo.City);
                    var stream = App.client.GetStringAsync(uri).Result;
                    Console.WriteLine(stream);

                    var js = JsonConvert.DeserializeObject<List<osusowakeFood>>(stream);

                    foreach (var filepath in js)
                    {
                        var tgr = new TapGestureRecognizer();
                        tgr.Tapped += async (sender, e) =>
                        {
                            await Navigation.PushAsync(new OsusowakeDetailPage
                            {
                                BindingContext = filepath
                            });
                        };
                        Image image = new Image();
                        image.Source = ImageSource.FromUri(new Uri(filepath.imageUrl));
                        image.HeightRequest = imageDimension;
                        image.WidthRequest = imageDimension;
                        image.GestureRecognizers.Add(tgr);
                        flexLayout.Children.Add(image);
                    }
                }
                catch(Exception ex)
                {
                    flexLayout.Children.Add(new Label
                    {
                        Text = "Cannot Access list of bitmap files"
                    });
                    Console.WriteLine(ex.Message);
                    await DisplayAlert("失敗", "通信に失敗しました", "閉じる");
                    return;
                }
            }
            activity_indicator.IsRunning = false;
            activity_indicator.IsVisible = false;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            flexLayout.Children.Clear();

            if (App.Authenticator.user == null)
            {
                await DisplayAlert("ログインしてください", "", "閉じる");
                flexLayout.Children.Add(new Label
                {
                    Text = "ログインしてください"
                });

                return;
            }
            LoadBitmapCollection();
        }
    }
}
